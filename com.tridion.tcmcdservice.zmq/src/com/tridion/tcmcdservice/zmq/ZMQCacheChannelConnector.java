/**
 *   @description ZeroMQ Cache Channel Connector
 *   @created April 14, 2014  
 *	 @author Rob van Oostenrijk
 */
package com.tridion.tcmcdservice.zmq;

import java.io.IOException;
import java.io.Serializable;
import java.io.StringReader;
import java.io.StringWriter;
import java.util.UUID;
import java.util.concurrent.LinkedBlockingQueue;
import java.util.concurrent.TimeUnit;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.transform.OutputKeys;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerException;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;

import org.jeromq.ZMQ;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.xml.sax.InputSource;
import org.xml.sax.SAXException;

import com.tridion.cache.CacheChannelConnector;
import com.tridion.cache.CacheChannelEventListener;
import com.tridion.cache.CacheEvent;
import com.tridion.cache.CacheException;
import com.tridion.cache.KeyGenerator;
import com.tridion.configuration.Configuration;
import com.tridion.configuration.ConfigurationException;

/**
 * ZMQCacheChannelConnector implements a {@link CacheChannelConnector} communicating with ZeroMQ
 * (http://zeromq.org/) using the Java ZeroMQ library (https://github.com/zeromq/jeromq).
 */
public class ZMQCacheChannelConnector implements CacheChannelConnector
{
	private static Logger mLog = LoggerFactory.getLogger(ZMQCacheChannelConnector.class);
	private static DocumentBuilderFactory mDocumentFactory = DocumentBuilderFactory.newInstance();	
	
	private static CacheChannelEventListener mEmptyListener = new CacheChannelEventListener()
	{
	    public void handleRemoteEvent(CacheEvent event) {}
	    
	    public void handleDisconnect() {}
	    
	    public void handleConnect() {}
	};
	
	private String mIdentifier = "ZeroMQ-" + UUID.randomUUID().toString();
	private String mSubscriptionUri = "tcp://localhost:5556";
	private String mSubmissionUri = "tcp://localhost:5557";
	private String mTopic = "Tridion";
	
	private CacheChannelEventListener mListener = mEmptyListener;
	private Boolean mIsClosed = false;
	
	private Thread mSender = null;
	private Thread mReceiver = null;
	private LinkedBlockingQueue<String> mQueue = new LinkedBlockingQueue<String>();
	
	/**
	 *  Get the client identifier for this {@link ZMQCacheChannelConnector}  
	 */
	public String getIdentifier()
	{
		return mIdentifier;
	}
	
	/**
	 *  Get the subscription uri for this {@link ZMQCacheChannelConnector}  
	 */
	private String getSubscriptionUri()
	{
		return mSubscriptionUri;				
	}
	
	/**
	 *  Get the submission uri for this {@link ZMQCacheChannelConnector}  
	 */
	private String getSubmissionUri()
	{
		return mSubmissionUri;				
	}
	
	/**
	 *  Get the subscription topic for this {@link ZMQCacheChannelConnector}  
	 */
	private String getTopic()
	{
		return mTopic;				
	}
		
	/**
	 * Verify if the service is available
	 */	
	private void verifyOpenState()
	{
		if (mIsClosed) 
			throw new IllegalStateException("Method was called on closed instance");
	}
	
	/**
	 * Initialize a new instance of ZMQCacheChannelConnector
	 */
	public ZMQCacheChannelConnector()
	{
		mReceiver = new Thread(new MessageReceiver(this));
		mReceiver.setDaemon(true);
		
		mSender = new Thread(new MessageSender(this));
		mSender.setDaemon(true);
	}
	
	@Override
	public void configure(Configuration configuration) throws ConfigurationException 
	{
		mSubscriptionUri = configuration.getParameterValue("subscriptionUri");
	
		if (mSubscriptionUri == null)
			throw new ConfigurationException("subscriptionUri is not configured.");
		
		mSubmissionUri = configuration.getParameterValue("submissionUri");
		
		if (mSubmissionUri == null)
			throw new ConfigurationException("submissionUri is not configured.");
		
		mTopic = configuration.getParameterValue("topic", "TridionCacheChannel");
		
		mLog.info("Configuration: Client [" + mIdentifier + "] SubscriptionUri [" + mSubscriptionUri + "] SubmissionUri [" + mSubmissionUri + "] Topic [" + mTopic + "]");		
	}
	
	@Override
	public void validate() throws CacheException
	{
		verifyOpenState();
		
		if (!mReceiver.isAlive())
			mReceiver.start();			

		if (!mSender.isAlive())
			mSender.start();
				
		mListener.handleConnect();
	}	
	
	@Override
	public void broadcastEvent(CacheEvent cacheEvent) throws CacheException 
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
			
			mQueue.add(writer.toString());
		}
		catch (IllegalStateException ex)
		{
			mLog.error("broadcastEvent: Error inserting into outgoing message queue.", ex);		
		}
		catch (TransformerException ex) 
		{
			mLog.error("broadcastEvent: Error creating XML.", ex);				
		}
		catch (ParserConfigurationException ex) 
		{
			mLog.error("broadcastEvent: Error initializing XML parser.", ex);				
		}
	}
	
	private void handleSubscriptionMessage(String message) 
	{
		try
		{		
			DocumentBuilder documentBuilder = mDocumentFactory.newDocumentBuilder();		            
		    InputSource inputSource = new InputSource(new StringReader(message));		            
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
		catch (ParserConfigurationException ex)
		{
			mLog.error("handleSubscriptionMessage: Error initializing XML parser.", ex);
		}
		catch (SAXException ex) 
		{
			mLog.error("handleSubscriptionMessage: Error reading input XML text.", ex);				
		}
		catch (IOException ex) 
		{
			mLog.error("handleSubscriptionMessage: Error reading input XML text.", ex);
		}
		catch (IllegalArgumentException ex)
		{
			mLog.error("handleSubscriptionMessage: Error reading input XML text.", ex);		
		}
	}
	
	@Override
	/**
	 * Close the current ZMQCacheChannelConnector
	 */
	public void close() 
	{
		if (!mIsClosed)
		{
			mIsClosed = true;
						
			// Gracefully close the current processors
			if (mReceiver != null && mReceiver.isAlive())
			{
				mReceiver.interrupt();
				
				try 
				{
					mReceiver.join();
				} 
				catch (InterruptedException e) 
				{
					//
				}
			}
			
			if (mSender != null && mSender.isAlive())
			{
				mSender.interrupt();
				
				try 
				{
					mSender.join();
				} 
				catch (InterruptedException e) 
				{
					//
				}
			}			
			
			mListener.handleDisconnect();
		}
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
	
	/**
	 * MessageSender implements a Java runnable which sends outgoing ZeroMQ messages
	 */
	private static class MessageSender implements Runnable 
	{
		private ZMQCacheChannelConnector mConnector;
		
		public MessageSender(final ZMQCacheChannelConnector connector)
		{
			mConnector = connector;
		}		
		
		/**
		 * Execute the receive events thread
		 */
        public void run() 
        {	
        	String connectorTopic = mConnector.getTopic();
        	String connectorIdentifier = mConnector.getIdentifier();
        	
        	ZMQ.Context context = ZMQ.context();
        	
        	ZMQ.Socket pushSocket = context.socket(ZMQ.PUSH);
        	pushSocket.setIdentity(connectorIdentifier + "-PUSH");
        	pushSocket.connect(mConnector.getSubmissionUri());
        	
        	try 
        	{   	
	        	while (!Thread.currentThread().isInterrupted()) 
	        	{   
	    			String message = mConnector.mQueue.poll(1000, TimeUnit.MILLISECONDS);
				    			
	    			if (message != null)
					{
	        			mLog.debug("Sending message: Topic [" + connectorTopic + "], Client [" + connectorIdentifier + "], Message [" + message + "].");
	            		            			
	        			pushSocket.sendMore(connectorTopic);
	        			pushSocket.sendMore(connectorIdentifier);
	        			pushSocket.send(message);
					}        		
	        	}
        	}
        	catch (InterruptedException e)
        	{
        		mLog.info("MessageSender has been interrupted.");
			}
        	
        	pushSocket.close();
        	context.term();
        }
    }	

	/**
	 * MessageReceiver implements a Java runnable which polls and processes incoming ZeroMQ subscription messages
	 */
	private static class MessageReceiver implements Runnable 
	{
		private ZMQCacheChannelConnector mConnector;
		
		public MessageReceiver(final ZMQCacheChannelConnector connector)
		{
			mConnector = connector;
		}
			
		/**
		 * Execute the receive events thread
		 */
        public void run() 
        {	
        	String connectorIdentifier = mConnector.getIdentifier();
        	
        	ZMQ.Context context = ZMQ.context();
        	
        	ZMQ.Socket subscribeSocket = context.socket(ZMQ.SUB);
        	
        	subscribeSocket.setIdentity(connectorIdentifier + "-SUB");
        	subscribeSocket.connect(mConnector.getSubscriptionUri());
        	
        	subscribeSocket.subscribe(mConnector.getTopic());
        	
        	ZMQ.Poller items = context.poller(1);
            items.register(subscribeSocket, ZMQ.Poller.POLLIN);
        	
        	while (!Thread.currentThread().isInterrupted()) 
        	{
        		items.poll(1000);
        		
        		if (items.pollin(0)) 
        		{
                    String topic = subscribeSocket.recvStr();
                    String identifier = subscribeSocket.recvStr();
                    String content = subscribeSocket.recvStr();
                    
                    mLog.debug("Received message: Topic [" + topic + "], Client [" + identifier + "], Message [" + content + "].");
                    
                    // Only process messages from other clients on the same topic                    
                    if (!identifier.equalsIgnoreCase(connectorIdentifier))
                    	mConnector.handleSubscriptionMessage(content);                    	
                }
        	}
        	
        	subscribeSocket.close();
        	context.term();
        }
    }
}
