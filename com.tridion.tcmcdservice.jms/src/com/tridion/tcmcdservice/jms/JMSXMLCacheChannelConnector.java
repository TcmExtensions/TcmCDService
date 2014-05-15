/**
 *   @description JMS XML Cache Channel Connector
 *   @created April 10, 2014  
 *	 @author Rob van Oostenrijk
 */
package com.tridion.tcmcdservice.jms;

import java.io.IOException;
import java.io.Serializable;
import java.io.StringReader;
import java.io.StringWriter;
import java.util.List;
import java.util.Properties;
import java.util.UUID;

import javax.jms.DeliveryMode;
import javax.jms.ExceptionListener;
import javax.jms.JMSException;
import javax.jms.Message;
import javax.jms.MessageListener;
import javax.jms.Session;
import javax.jms.TextMessage;
import javax.jms.Topic;
import javax.jms.TopicConnection;
import javax.jms.TopicConnectionFactory;
import javax.jms.TopicPublisher;
import javax.jms.TopicSession;
import javax.jms.TopicSubscriber;
import javax.naming.Context;
import javax.naming.InitialContext;
import javax.naming.NamingException;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.transform.OutputKeys;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerException;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.xml.sax.InputSource;
import org.xml.sax.SAXException;

import com.tridion.cache.CacheChannelEventListener;
import com.tridion.cache.CacheEvent;
import com.tridion.cache.JMSCacheChannelConnector;
import com.tridion.cache.KeyGenerator;
import com.tridion.configuration.Configuration;
import com.tridion.configuration.ConfigurationException;

/**
 * JMSXMLCacheConnector overrides the default com.tridion.cache.JMSCacheChannelConnector
 * and provides XML formatted cache event messages over JMS.
 */
public class JMSXMLCacheChannelConnector extends JMSCacheChannelConnector 
{
	private static Logger mLog = LoggerFactory.getLogger(JMSXMLCacheChannelConnector.class);
	private static DocumentBuilderFactory mDocumentFactory = DocumentBuilderFactory.newInstance();
	
	private static CacheChannelEventListener mEmptyListener = new CacheChannelEventListener()
	{
		public void handleRemoteEvent(CacheEvent event) {}	    
	    public void handleDisconnect() {}	    
	    public void handleConnect() {}
	};
	
	private CacheChannelEventListener mListener = mEmptyListener;
	
	/**
	 * Apply configuration to the JMSXMLCacheChannelConnector
	 */
	@Override
	public void configure(Configuration configuration) 
		throws ConfigurationException 
	{
		Properties contextProperties = null;
		
		if (configuration.hasChild("JndiContext"))
		{
			Configuration jndiConfig = configuration.getChild("JndiContext");
			contextProperties = new Properties();
	      
			List<Configuration> configs = jndiConfig.getChildrenByName("Property");
			
			for (Configuration config : configs)
			{				
				String propertyKey = config.getAttribute("Name");
				String propertyValue = config.getAttribute("Value");
				contextProperties.setProperty(propertyKey, propertyValue);
				
				mLog.debug("JMS XML Connector JNDI Property: '" + propertyKey + "', value: '" + propertyValue + "'");
			}
	    }
		
		String topic = configuration.getAttribute("Topic", "TridionCacheChannel");
	    String factory = configuration.getAttribute("TopicConnectionFactory", "TopicConnectionFactory");
	    
	    this.client = new JMSXMLClient(contextProperties, factory, topic);
    		    
	    mLog.info("JMS XML configuration TopicConnectionFactory: " + factory + " topic: " + topic);		
	}
	
	@Override
	protected void handleJmsMessage(Message message)
	{
		if (message instanceof TextMessage) 
		{
			try
			{
				String text = ((TextMessage)message).getText();
				
				if (text != null)
				{
		            DocumentBuilder documentBuilder = mDocumentFactory.newDocumentBuilder();		            
		            InputSource inputSource = new InputSource(new StringReader(text));		            
		            Document document = documentBuilder.parse(inputSource);
		            
		            Element element = document.getDocumentElement();
		            
		            if (element != null && element.getTagName() == "cacheEvent")
		            {
		            	String regionPath = element.getAttribute("regionPath");
		            	String key = element.getAttribute("key");
		            	int eventType = Integer.parseInt(element.getAttribute("type"));
		            	
				    	// Convert to the original serializable value as per com.tridion.cache.KeyGenerator
				    	Serializable serializableKey = key.contains(KeyGenerator.KEY_DELIMITER) ? key : Integer.valueOf(key);
		            	
	            		mListener.handleRemoteEvent(new CacheEvent(regionPath, serializableKey, eventType));		            	
		            }
				}				
				else
					mLog.warn("Ignoring unexpected message data received on topic, data: " + (text != null ? text : "null"));
			}
			catch (JMSException ex)
			{
				mLog.error("handleJmsMessage: JMS error.", ex);
			}
			catch (ParserConfigurationException ex)
			{
				mLog.error("handleJmsMessage: Error initializing XML parser.", ex);
			}
			catch (SAXException ex) 
			{
				mLog.error("handleJmsMessage: Error reading input XML text.", ex);				
			}
			catch (IOException ex) 
			{
				mLog.error("handleJmsMessage: Error reading input XML text.", ex);
			}
			catch (IllegalArgumentException ex)
			{
				mLog.error("handleJmsMessage: Error reading input XML text.", ex);		
			}			
	    }
		else
			mLog.warn("Ignoring unexpected message, type: " + (message != null ? message.getClass().getName() : "null"));		
	}	
	
	/**
	 * Configure the {@link CacheChannelEventListener} for this CacheChannelConnector
	 * 
	 * @param listener Event listener to pass events to, a value of null assigns an empty listener
	 */	
	@Override
	public void setListener(CacheChannelEventListener listener)
	{
		mListener = (listener != null ? listener : mEmptyListener);
	}
	
	protected static class JMSXMLClient
	    implements JMSCacheChannelConnector.JMSClient
    {
		private static Logger mLog = LoggerFactory.getLogger(JMSXMLClient.class);
		
		private Properties mProperties;
		private String mFactoryName;
		private String mTopicName;
		
		private String mIdentifier = "JMSXML-" + UUID.randomUUID().toString();
		
		private Topic mTopic;	
		private TopicConnection mConnection;
		private TopicSession mSession;
		private TopicSubscriber mSubscriber;
		private TopicPublisher mPublisher;
		
		/**
		 * 
		 * 
		 * @param jndiProperties JNDI context properties
		 * @param factoryName JMS factory class name
		 * @param topicName JMS subscription topic
		 */
		public JMSXMLClient(Properties jndiProperties, String factoryName, String topicName)
		{
			mProperties = jndiProperties;
			mFactoryName = factoryName;
			mTopicName = topicName;
			
			mLog.info("Initializing: Client [" + mIdentifier + "], Factory [" + factoryName + "] Topic [" + topicName + "]");
		}		
		
		@Override
		public void connect(MessageListener messageListener, ExceptionListener exceptionListener)
			throws JMSException, NamingException 
		{
			Context context = mProperties != null ? new InitialContext(mProperties) : new InitialContext();
			  
			TopicConnectionFactory factory = (TopicConnectionFactory)context.lookup(mFactoryName);
			mConnection = factory.createTopicConnection();
			mConnection.setClientID(mIdentifier);
			mConnection.setExceptionListener(exceptionListener);
			
			mTopic = (Topic)context.lookup(mTopicName);
			
			mSession = mConnection.createTopicSession(false, TopicSession.AUTO_ACKNOWLEDGE);
			
			mSession = mConnection.createTopicSession(false, Session.AUTO_ACKNOWLEDGE);
			
			mConnection.start();
			
			mPublisher = mSession.createPublisher(mTopic);
			mPublisher.setDeliveryMode(DeliveryMode.NON_PERSISTENT);
		
			//mSubscriber = mSession.createSubscriber(mTopic, "Client <> '" + mUniqueIdentifier + "'", true);
			mSubscriber = mSession.createSubscriber(mTopic, null, true);
			mSubscriber.setMessageListener(messageListener);
		}
		
		@Override
		public void broadcastEvent(CacheEvent cacheEvent)
			throws JMSException 
		{
			try
			{		
				DocumentBuilder builder = mDocumentFactory.newDocumentBuilder(); 
				Document document = builder.newDocument();
		
				Element element = document.createElement("cacheEvent");
				document.appendChild(element);		
				
				element.setAttribute("regionPath", cacheEvent.getRegionPath());
				element.setAttribute("key", cacheEvent.getKey().toString());
				element.setAttribute("type", Integer.toString(cacheEvent.getType()));
		
				DOMSource domSource = new DOMSource(document);
				StringWriter writer = new StringWriter();
				StreamResult result = new StreamResult(writer);
				TransformerFactory transformerFactory = TransformerFactory.newInstance();
				Transformer transformer = transformerFactory.newTransformer();
				transformer.setOutputProperty(OutputKeys.OMIT_XML_DECLARATION, "yes");
				
				transformer.transform(domSource, result);
		       
				TextMessage message = mSession.createTextMessage(writer.toString());
				message.setStringProperty("Client", mIdentifier);
				mPublisher.send(message);
			}
			catch (JMSException ex)
			{			
				mLog.warn("sendMessage: Remote connection error.", ex);
				throw ex;
			}
			catch (TransformerException ex) 
			{
				mLog.error("sendMessage: Error creating XML.", ex);				
			}
			catch (ParserConfigurationException ex) 
			{
				mLog.error("sendMessage: Error initializing XML parser.", ex);				
			}
		}
		
		@Override
		public void cleanupIgnoringErrors() 
		{
			try
			{
				if (mSubscriber != null)
					mSubscriber.close();
			}
			catch (JMSException ex) {}
			
			mSubscriber = null;
			
			try
			{
				if (mPublisher != null)
					mPublisher.close();
			}
			catch (JMSException ex) {}
			
			mPublisher = null;
			
			try		
			{
				if (mSession != null)
					mSession.close();
			}
			catch (JMSException ex) {}
			
			mSession = null;

			try
			{
				if (mConnection != null)
				{
					mConnection.stop();
					mConnection.close();
				}
			}
			catch (JMSException ex) {}
			
			mConnection = null;			
		}		
    }
}
