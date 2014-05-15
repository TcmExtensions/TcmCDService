#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: ZeroMQ Cache
// ---------------------------------------------------------------------------------
//	Date Created	: April 19, 2014
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using TcmCDService.Configuration;
using TcmCDService.Logging;

namespace TcmCDService.CacheTypes
{
	/// <summary>
	/// <see cref="ZeroMQCache" /> exposes a <see cref="T:TcmCDService.CacheTypes.CacheType" /> using ZeroMQ to provide a message broker in order to invalidate cache items.
	/// </summary>
	public class ZeroMQCache : TcmCDService.CacheTypes.CacheType, IDisposable
	{
		private String mIdentifier;
		private String mTopic;
		private BlockingCollection<ZeroMQMessage> mQueue;		
		private CancellationTokenSource mCancellationTokenSource = null;
		private Task mSender;
		private Task mReceiver;

		private class ZeroMQMessage
		{
			public String Topic { get; set; }
			public String Client { get; set; }
			public String Content { get; set; }

			public override String ToString()
			{
				return String.Format("Topic \"{0}\", Client \"{1}\", Message \"{2}\".", Topic, Client, Content);
			}
		}

		/// <summary>
		/// Broadcasts the <paramref name="xmlCacheEvent" />
		/// </summary>
		/// <param name="xmlCacheEvent"><see cref="T:TcmCDService.CacheTypes.XmlCacheEvent" /></param>
		private void BroadcastEvent(XmlCacheEvent xmlCacheEvent)
		{
			ZeroMQMessage message = new ZeroMQMessage()
			{
				Topic = mTopic,
				Client = mIdentifier,
				Content = xmlCacheEvent.ToXml()
			};

			mQueue.Add(message);
		}

		private void SendEvents(String submissionUri, CancellationToken cancellationToken)
		{
			using (NetMQContext context = NetMQContext.Create())
			{
				using (NetMQSocket pushSocket = context.CreatePushSocket())
				{
					pushSocket.IgnoreErrors = true;
					pushSocket.Connect(submissionUri);

					Logger.Info("ZeroMQCache: Connected to submissionUri \"{0}\".", submissionUri);

					while (!cancellationToken.IsCancellationRequested)
					{
						try
						{
							ZeroMQMessage message;

							if (mQueue.TryTake(out message, TimeSpan.FromSeconds(1)))
							{
								Logger.Debug("ZeroMQCache: Sending -> {0}", message.ToString());

								pushSocket.SendMore(message.Topic);
								pushSocket.SendMore(message.Client);
								pushSocket.Send(message.Content);
							}
						}
						catch (OperationCanceledException)
						{
							// We have been asked to cancel operation
							break;
						}
						catch (Exception ex)
						{
							Logger.Error("ZeroMQCache: Error sending message.", ex);
						}
					}

					// Close socket
					pushSocket.Close();
				}

				context.Terminate();
			}
		}
	
		private void ReceiveEvents(String subscriptionUri, String topic, CancellationToken cancellationToken)
		{
			using (NetMQContext context = NetMQContext.Create())
			{
				using (NetMQSocket subscriberSocket = context.CreateSubscriberSocket())
				{
					subscriberSocket.IgnoreErrors = true;
					subscriberSocket.Connect(subscriptionUri);
					subscriberSocket.Subscribe(topic);

					Logger.Info("ZeroMQCache: Connected to subscriptionUri \"{0}\".", subscriptionUri);

					// Eventhandler delegate to handle receiving messages
					subscriberSocket.ReceiveReady += (sender, args) =>
					{
						try
						{
							if (args.ReceiveReady)
							{
								// Recieve and relay
								NetMQMessage netMQmessage = args.Socket.ReceiveMessage();
								
								// Recieve the message
								ZeroMQMessage message = new ZeroMQMessage()
								{
									Topic = netMQmessage.Pop().ConvertToString(),
									Client = netMQmessage.Pop().ConvertToString(),
									Content = netMQmessage.Pop().ConvertToString()
								};

								Logger.Debug("ZeroMQCache: Received -> {0}", message.ToString());

								XmlCacheEvent cacheEvent = XmlCacheEvent.FromXml(message.Content);

								if (cacheEvent != null)
									OnCacheEvent(cacheEvent.CacheRegion, cacheEvent.Key, cacheEvent.EventType);
							}
						}
						catch (OperationCanceledException)
						{
							// We have been asked to cancel operation
							return;
						}
						catch (Exception ex)
						{
							Logger.Error("ZeroMQCache: Error receiving message.", ex);
						}
					};

					while (!cancellationToken.IsCancellationRequested)
					{
						try
						{
							subscriberSocket.Poll(TimeSpan.FromSeconds(1));
						}
						catch (OperationCanceledException)
						{
							// We have been asked to cancel operation
							break;
						}
						catch (Exception ex)
						{
							Logger.Error("ZeroMQCache: Error polling for messages.", ex);
						}
					}

					// Close socket
					subscriberSocket.Close();

				}

				context.Terminate();
			}
		}

		/// <summary>
		/// Gets the unique client identifier of this <see cref="ZeroMQCache" />
		/// </summary>
		/// <value>
		/// The unique client identifier of this <see cref="ZeroMQCache" />
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
		/// Initializes a new instance of the <see cref="ZeroMQCache"/> class.
		/// </summary>
		/// <param name="settings"><see cref="T:TcmCDService.Configuration.Settings" /></param>
		public ZeroMQCache(Settings settings): base(settings)
		{
			mIdentifier = "ZeroMQ-" + Guid.NewGuid().ToString("D");
			mCancellationTokenSource = new CancellationTokenSource();
			mQueue = new BlockingCollection<ZeroMQMessage>();

			String subscriptionUri = settings.Get<String>("subscriptionUri");

			if (String.IsNullOrEmpty(subscriptionUri))
				throw new ConfigurationErrorsException("ZeroMQCache: SubscriptionUri is unconfigured.");

			String submissionUri = settings.Get<String>("submissionUri");

			if (String.IsNullOrEmpty(submissionUri))
				throw new ConfigurationErrorsException("ZeroMQCache: SubmissionUri is unconfigured.");

			mTopic = settings.Get<String>("topic");

			if (String.IsNullOrEmpty(mTopic))
			{
				Logger.Info("ZeroMQCache: No topic configured, defaulting to \"TridionCacheChannel\".");
				mTopic = "TridionCacheChannel";
			}

			mSender = new Task(() =>
				{
					SendEvents(submissionUri, mCancellationTokenSource.Token);
				}, mCancellationTokenSource.Token,
				TaskCreationOptions.LongRunning);

			mReceiver = new Task(() =>
				{
					ReceiveEvents(subscriptionUri, mTopic, mCancellationTokenSource.Token);
				}, mCancellationTokenSource.Token,
				TaskCreationOptions.LongRunning);
		}

		/// <summary>
		/// Instruct this <see cref="ZeroMQCache" /> to connect to the remote cache system if required.
		/// </summary>
		public override void Connect()
		{
			if (mSender != null && mSender.Status != TaskStatus.Running)
				mSender.Start();

			if (mReceiver != null && mReceiver.Status != TaskStatus.Running)
				mReceiver.Start();
		}

		/// <summary>
		/// Instruct this <see cref="ZeroMQCache" /> to disconnect from a remote cache system if required.
		/// </summary>
		public override void Disconnect()
		{	
			// Request all server tasks to cancel and wait for their cancellation
			mCancellationTokenSource.Cancel();

			if (mReceiver != null && mReceiver.Status == TaskStatus.Running)
				Task.WaitAll(mReceiver);

			if (mSender != null && mSender.Status == TaskStatus.Running)
				Task.WaitAll(mSender);
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
					Logger.Error("ZeroMQCache", ex);
				}
			}
		}
	}
}
