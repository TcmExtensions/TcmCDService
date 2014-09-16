#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: RMI Cache Channel Connector
// ---------------------------------------------------------------------------------
//	Date Created	: April 15, 2014
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
using Codemesh.JuggerNET;

namespace Com.Tridion.Cache
{
	/// <summary>
	/// <see cref="RMICacheChannelConnector" /> wraps around com.tridion.cache.RMICacheChannelConnector
	/// </summary>
	public class RMICacheChannelConnector : JuggerNETProxyObject
	{
		// JuggerNet requires an internal JavaClass field of "_cmj_theClass" as per CodeMesh.JuggerNET.JvmLoader.ProcessType
		private static JavaClass _cmj_theClass = JavaClass.RegisterClass("com.tridion.cache.RMICacheChannelConnector", typeof(RMICacheChannelConnector));

		private static JavaMethod mGetGUID = new JavaMethod(_cmj_theClass, typeof(string), "getGUID", "()Ljava/lang/String;", false);

		private static JavaMethod mConstructor1 = new JavaMethod(_cmj_theClass, typeof(RMICacheChannelConnector), "<init>", "()V", false);
		private static JavaMethod mConstructor2 = new JavaMethod(_cmj_theClass, typeof(RMICacheChannelConnector), "<init>", "(Ljava/lang/String;ILjava/lang/String;)V", false);
		private static JavaMethod mBroadcastEvent = new JavaMethod(_cmj_theClass, typeof(void), "broadcastEvent", "(Lcom/tridion/cache/CacheEvent;)V", false);
		private static JavaMethod mClose = new JavaMethod(_cmj_theClass, typeof(void), "close", "()V", false);
		private static JavaMethod mSetListener = new JavaMethod(_cmj_theClass, typeof(void), "setListener", "(Lcom/tridion/cache/CacheChannelEventListener;)V", false);
		private static JavaMethod mValidate = new JavaMethod(_cmj_theClass, typeof(void), "validate", "()V", false);

		/// <summary>
		/// Gets the <see cref="RMICacheChannelConnector" /> unique identifier
		/// </summary>
		/// <value>
		/// <see cref="RMICacheChannelConnector" /> unique identifier.
		/// </value>
		public String Identifier
		{
			get
			{
				return mGetGUID.CallString(this);
			}
		}

		/// <summary>
		/// Assign a <see cref="I:Com.Tridion.Cache.ICacheChannelEventListener" /> to this <see cref="RMICacheChannelConnector" />
		/// </summary>
		/// <value>
		/// <see cref="I:Com.Tridion.Cache.ICacheChannelEventListener" /> listener
		/// </value>
		public ICacheChannelEventListener Listener
		{
			set
			{

				mSetListener.CallVoid(this, value, typeof(ICacheChannelEventListener));
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RMICacheChannelConnector"/> class.
		/// </summary>
		/// <param name="objectHandle"><see cref="T:CodeMesh.JuggerNET.JNIHandle" /></param>
		public RMICacheChannelConnector(JNIHandle objectHandle): base(objectHandle)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RMICacheChannelConnector"/> class.
		/// </summary>
		public RMICacheChannelConnector(): base(JNIHandle.NULL)
		{
			base.JObject = (long)mConstructor1.CallObject(this);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="RMICacheChannelConnector"/> class.
		/// </summary>
		/// <param name="host">Tridion Cache Channel RMI Host</param>
		/// <param name="port">Tridion Cache Channel RMI Port</param>
		/// <param name="instanceIdentifier">Tridion Instance identifier.</param>
		public RMICacheChannelConnector(String host, int port, String instanceIdentifier): base(JNIHandle.NULL)
		{
			jvalue[] args = new jvalue[3];

			using (JavaMethodArguments arguments = new JavaMethodArguments(args).Add(host).Add(port).Add(instanceIdentifier))
			{
				base.JObject = (long)mConstructor2.CallObject(this, arguments);
			}
		}

		/// <summary>
		/// Validates the current <see cref="RMICacheChannelConnector" /> connection
		/// </summary>
		/// <remarks><see cref="RMICacheChannelConnector" /> will reconnect as required.</remarks>
		public void Validate()
		{
			mValidate.CallVoid(this);
		}

		/// <summary>
		/// Close this <see cref="RMICacheChannelConnector" /> instance.
		/// </summary>
		public void Close()
		{
			mClose.CallVoid(this);
		}

		/// <summary>
		/// Broadcasts the given <see cref="T:Com.Tridion.Cache.CacheEvent" /> to all other connected clients
		/// </summary>
		/// <param name="cacheEvent"><see cref="T:Com.Tridion.Cache.CacheEvent" /></param>
		public void BroadcastEvent(CacheEvent cacheEvent)
		{
			mBroadcastEvent.CallVoid(this, cacheEvent);
		}
	}
}
