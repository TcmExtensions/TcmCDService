#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: ActiveMQ Cache
// ---------------------------------------------------------------------------------
//	Date Created	: April 5, 2014
//	Author			: Rob van Oostenrijk
// ---------------------------------------------------------------------------------
// 	Change History
//	Date Modified       : 
//	Changed By          : 
//	Change Description  : 
//
////////////////////////////////////////////////////////////////////////////////////
#endregion
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using TcmCDService.Configuration;
using TcmCDService.Logging;

namespace TcmCDService.CacheTypes
{
	/// <summary>
	/// <see cref="ActiveMQCache" /> exposes a <see cref="T:TcmCDService.CacheTypes.CacheType" /> using Apache ActiveMQ message broker in order to invalidate cache items.
	/// </summary>
	public class ActiveMQCache : TcmCDService.CacheTypes.CacheType
	{
		private String mIdentifier;
		private IConnection mConnector;
		private ISession mSession;
		private IMessageConsumer mConsumer;
		private IMessageProducer mProducer;

		/// <summary>
		/// Broadcasts the <paramref name="xmlCacheEvent" />
		/// </summary>
		/// <param name="xmlCacheEvent"><see cref="T:TcmCDService.CacheTypes.XmlCacheEvent" /></param>
		private void BroadcastEvent(XmlCacheEvent xmlCacheEvent)
		{
			if (mSession != null && mProducer != null)
			{
				ITextMessage message = mSession.CreateTextMessage(xmlCacheEvent.ToXml());

				message.Properties.SetString("Client", mIdentifier);
				mProducer.Send(message);
			}
		}

		/// <summary>
		/// Gets the unique client identifier of this <see cref="CacheType" />
		/// </summary>
		/// <value>
		/// The unique client identifier of this <see cref="CacheType" />
		/// </value>
		public override String Identifier
		{
			get
			{
				return mIdentifier;
			}
		}

		/// <summary>
		/// Gets the expiration of cache items in minutes
		/// </summary>
		/// <value>
		/// Expiration of cache items in minutes
		/// </value>
		/// <remarks>-1 means no caching applies</remarks>
		public override int Expiration
		{
			get
			{
				return Config.Instance.DefaultCacheExpiry;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ActiveMQCache"/> class.
		/// </summary>
		/// <param name="settings"><see cref="T:TcmCDService.Configuration.Settings" /></param>
		public ActiveMQCache(Settings settings): base(settings)
		{
			mIdentifier = "ActiveMQ-" + Guid.NewGuid().ToString("D");

			String brokerUrl = settings.Get<String>("brokerUrl");

			if (String.IsNullOrEmpty(brokerUrl))
				throw new ConfigurationErrorsException("ActiveMQCache: BrokerUrl is unconfigured.");

			String topic = settings.Get<String>("topic");

			if (String.IsNullOrEmpty(topic))
				throw new ConfigurationErrorsException("ActiveMQCache: Topic is unconfigured.");

			try
			{
				String username = settings.Get<String>("username");
				String password = settings.Get<String>("password");

				ConnectionFactory connectionFactory = new ConnectionFactory(brokerUrl)
				{
					ClientId = Identifier,
					UserName = username,
					Password = password
				};
				
				mConnector = connectionFactory.CreateConnection();
				mConnector.ExceptionListener += (Exception exception) =>
				{
					Logger.Error("ActiveMQCache: Exception while listening.", exception);
				};

				mSession = mConnector.CreateSession(AcknowledgementMode.DupsOkAcknowledge);
				mConsumer = mSession.CreateConsumer(new ActiveMQTopic(topic), null, true);
				mProducer = mSession.CreateProducer(new ActiveMQTopic(topic));

				mConsumer.Listener += (IMessage message) => 
				{
					ITextMessage textMessage = message as ITextMessage;

					if (textMessage != null)
					{
						XmlCacheEvent xmlCacheEvent = XmlCacheEvent.FromXml(textMessage.Text);

						if (xmlCacheEvent != null)
							base.OnCacheEvent(xmlCacheEvent.CacheRegion, xmlCacheEvent.Key, xmlCacheEvent.EventType);
					}
				};
			}
			catch (Exception ex)
			{
				Logger.Error("ActiveMQCache", ex);
			}
		}

		/// <summary>
		/// Instruct this <see cref="ActiveMQCache" /> to connect to the remote cache system if required.
		/// </summary>
		public override void Connect()
		{
			if (mConnector != null && !mConnector.IsStarted)
				mConnector.Start();
		}

		/// <summary>
		/// Instruct this <see cref="ActiveMQCache" /> to disconnect from a remote cache system if required.
		/// </summary>
		public override void Disconnect()
		{
			if (mConsumer != null)
			{
				mConsumer.Close();
				mConsumer.Dispose();
			}

			if (mProducer != null)
			{
				mProducer.Close();
				mProducer.Dispose();
			}

			if (mSession != null)
			{
				mSession.Close();
				mSession.Dispose();
			}

			if (mConnector != null)
			{
				mConnector.Stop();
				mConnector.Close();
				mConnector.Dispose();
			}
		}

		/// <summary>
		/// Broadcasts a cache event to all other connected clients
		/// </summary>
		/// <param name="cacheRegion"><see cref="T:TcmCDService.CacheTypes.CacheRegion" /></param>
		/// <param name="key">Cache key as <see cref="T:System.String" /></param>
		/// <param name="eventType"><see cref="T:TcmCDService.CacheTypes.CacheEventType" /></param>
		public override void BroadcastEvent(CacheRegion cacheRegion, String key, CacheEventType eventType)
		{
			XmlCacheEvent cacheEvent = new XmlCacheEvent(cacheRegion, key, eventType);
			BroadcastEvent(cacheEvent);
		}

		/// <summary>
		/// Broadcasts a cache event to all other connected clients
		/// </summary>
		/// <param name="cacheRegion"><see cref="T:TcmCDService.CacheTypes.CacheRegion" /></param>
		/// <param name="key">Cache key as <see cref="T:System.Int32" /></param>
		/// <param name="eventType"><see cref="T:TcmCDService.CacheTypes.CacheEventType" /></param>
		public override void BroadcastEvent(CacheRegion cacheRegion, int key, CacheEventType eventType)
		{
			XmlCacheEvent cacheEvent = new XmlCacheEvent(cacheRegion, key, eventType);
			BroadcastEvent(cacheEvent);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected override void Dispose(Boolean disposing)
		{
			base.Dispose(disposing);

			if (disposing)
			{
				try
				{
					Disconnect();
				}
				catch (Exception ex)
				{
					Logger.Error("ActiveMQCache", ex);
				}
			}
		}
	}
}
