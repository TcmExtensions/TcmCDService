#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: ZeroMQ Broker
// ---------------------------------------------------------------------------------
//	Date Created	: April 14, 2014
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
	/// <see cref="ZeroMQBroker" /> exposes a <see cref="T:TcmCDService.CacheTypes.CacheType" /> using ZeroMQ to provide a message broker in order to invalidate cache items.
	/// </summary>
	/// <remarks>In addition of a CacheType, ZeroMQBroker also provides broker functionality.</remarks>
	public class ZeroMQBroker : TcmCDService.CacheTypes.CacheType, IDisposable
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

		private void SendEvents(String subscriptionUri, String topic, CancellationToken cancellationToken)
		{
			using (NetMQContext context = NetMQContext.Create())
			{
				using (NetMQSocket publishSocket = context.CreatePublisherSocket())
				{
					publishSocket.IgnoreErrors = true;
					publishSocket.Bind(subscriptionUri);

					Logger.Info("ZeroMQBroker: Bound to subscriptionUri \"{0}\".", subscriptionUri);

					while (!cancellationToken.IsCancellationRequested)
					{
						try
						{
							ZeroMQMessage message;

							if (mQueue.TryTake(out message, TimeSpan.FromSeconds(1)))
							{
								Logger.Debug("ZeroMQBroker: Sending -> {0}", message.ToString());

								publishSocket.SendMore(message.Topic);
								publishSocket.SendMore(message.Client);
								publishSocket.Send(message.Content);
							}
						}
						catch (OperationCanceledException)
						{
							// We have been asked to cancel operation
							break;
						}
						catch (Exception ex)
						{
							Logger.Error("ZeroMQBroker: Error sending message.", ex);
						}
					}

					// Close socket
					publishSocket.Close();
				}

				context.Terminate();
			}
		}
	
		private void ReceiveEvents(String submissionUri, String topic, CancellationToken cancellationToken)
		{
			using (NetMQContext context = NetMQContext.Create())
			{
				using (NetMQSocket pullSocket = context.CreatePullSocket())
				{
					pullSocket.IgnoreErrors = true;
					pullSocket.Bind(submissionUri);

					Logger.Info("ZeroMQBroker: Bound to submissionUri \"{0}\".", submissionUri);

					// Eventhandler delegate to handle receiving messages
					pullSocket.ReceiveReady += (sender, args) =>
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

								Logger.Debug("ZeroMQBroker: Received -> {0}", message.ToString());

								mQueue.Add(message);

								// ZeroMQBroker relays multiple topics, verify if the message was meant for the current client
								if (String.Equals(topic, message.Topic, StringComparison.OrdinalIgnoreCase))
								{
									XmlCacheEvent cacheEvent = XmlCacheEvent.FromXml(message.Content);

									if (cacheEvent != null)
										OnCacheEvent(cacheEvent.CacheRegion, cacheEvent.Key, cacheEvent.EventType);
								}
							}
						}
						catch (OperationCanceledException)
						{
							// We have been asked to cancel operation
							return;
						}
						catch (Exception ex)
						{
							Logger.Error("ZeroMQBroker: Error receiving message.", ex);
						}
					};

					while (!cancellationToken.IsCancellationRequested)
					{
						try
						{
							pullSocket.Poll(TimeSpan.FromSeconds(1));
						}
						catch (OperationCanceledException)
						{
							// We have been asked to cancel operation
							break;
						}
						catch (Exception ex)
						{
							Logger.Error("ZeroMQBroker: Error polling for messages.", ex);
						}
					}

					// Close socket
					pullSocket.Close();

				}

				context.Terminate();
			}
		}

		/// <summary>
		/// Gets the unique client identifier of this <see cref="ZeroMQBroker" />
		/// </summary>
		/// <value>
		/// The unique client identifier of this <see cref="ZeroMQBroker" />
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
		/// Initializes a new instance of the <see cref="ZeroMQBroker"/> class.
		/// </summary>
		/// <param name="settings"><see cref="T:TcmCDService.Configuration.Settings" /></param>
		public ZeroMQBroker(Settings settings): base(settings)
		{
			mIdentifier = "ZeroMQ-" + Guid.NewGuid().ToString("D");
			mCancellationTokenSource = new CancellationTokenSource();
			mQueue = new BlockingCollection<ZeroMQMessage>();

			String subscriptionUri = settings.Get<String>("subscriptionUri");

			if (String.IsNullOrEmpty(subscriptionUri))
				throw new ConfigurationErrorsException("ZeroMQBroker: SubscriptionUri is unconfigured.");

			String submissionUri = settings.Get<String>("submissionUri");

			if (String.IsNullOrEmpty(submissionUri))
				throw new ConfigurationErrorsException("ZeroMQBroker: SubmissionUri is unconfigured.");

			mTopic = settings.Get<String>("topic");

			if (String.IsNullOrEmpty(mTopic))
			{
				Logger.Info("ZeroMQBroker: No topic configured, defaulting to \"TridionCacheChannel\".");
				mTopic = "TridionCacheChannel";
			}

			mSender = new Task(() =>
				{
					SendEvents(subscriptionUri, mTopic, mCancellationTokenSource.Token);
				}, mCancellationTokenSource.Token,
				TaskCreationOptions.LongRunning);

			mReceiver = new Task(() =>
				{
					ReceiveEvents(submissionUri, mTopic, mCancellationTokenSource.Token);
				}, mCancellationTokenSource.Token,
				TaskCreationOptions.LongRunning);
		}

		/// <summary>
		/// Instruct this <see cref="ZeroMQBroker" /> to connect to the remote cache system if required.
		/// </summary>
		public override void Connect()
		{
			if (mSender != null && mSender.Status != TaskStatus.Running)
				mSender.Start();

			if (mReceiver != null && mReceiver.Status != TaskStatus.Running)
				mReceiver.Start();
		}

		/// <summary>
		/// Instruct this <see cref="ZeroMQBroker" /> to disconnect from a remote cache system if required.
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
					Logger.Error("ZeroMQBroker", ex);
				}
			}
		}
	}
}
