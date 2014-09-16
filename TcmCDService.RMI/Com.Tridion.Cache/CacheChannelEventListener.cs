#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Cache Channel Event Listener
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
using Codemesh.JuggerNET;

namespace Com.Tridion.Cache
{
	/// <summary>
	/// <see cref="CacheChannelEvenListener" /> is the callback class for <see cref="I:Com.Tridion.Cache.ICacheChannelEventListener" /> from Java com.tridion.cache.CacheChannelEventListener
	/// <list type="bullet">
	/// <item>
	///		<term>Interface Type:</term>
	///		<description><see cref="I:Com.Tridion.Cache.ICacheChannelEventListener" /></description>
	/// </item>
	/// <item>
	///		<term>Implementation Type:</term>
	///		<description><see cref="T:Com.Tridion.Cache.CacheChannelEventListener" /></description>
	/// </item>
	/// <item>
	///		<term>Callback Type:</term>
	///		<description><see cref="T:Com.Tridion.Cache.CacheChannelEventListenerCallback" /></description>
	/// </item>
	/// </list>
	/// </summary>
	public class CacheChannelEventListener : JuggerNETProxyObject, ICacheChannelEventListener
	{
		// JuggerNet requires an internal JavaClass field of "_cmj_theClass" as per CodeMesh.JuggerNET.JvmLoader.ProcessType
		private static JavaClass _cmj_theClass = JavaClass.RegisterClass("com.tridion.cache.CacheChannelEventListener", typeof(ICacheChannelEventListener), typeof(CacheChannelEventListener), typeof(CacheChannelEventListenerCallback));
		
		private static JavaMethod mHandleConnect = new JavaMethod(_cmj_theClass, typeof(void), "handleConnect", "()V", false);
		private static JavaMethod mHandleDisconnect = new JavaMethod(_cmj_theClass, typeof(void), "handleDisconnect", "()V", false);
		private static JavaMethod mHandleRemoteEvent = new JavaMethod(_cmj_theClass, typeof(void), "handleRemoteEvent", "(Lcom/tridion/cache/CacheEvent;)V", false);

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheChannelEventListener" /> class.
		/// </summary>
		/// <param name="objectHandle"><see cref="T:CodeMesh.JuggerNET.JNIHandle" /></param>
		public CacheChannelEventListener(JNIHandle objectHandle): base(objectHandle)
		{
		}

		/// <summary>
		/// Process "HandleConnect" callback from Java
		/// </summary>
		void ICacheChannelEventListener.HandleConnect()
		{
			mHandleConnect.CallVoid(this);
		}

		/// <summary>
		/// Process "HandleDisconnect" callback from Java
		/// </summary>
		void ICacheChannelEventListener.HandleDisconnect()
		{
			mHandleDisconnect.CallVoid(this);
		}

		/// <summary>
		/// Process "HandleRemoteEvent" callback from Java
		/// </summary>
		void ICacheChannelEventListener.HandleRemoteEvent(CacheEvent cacheEvent)
		{
			mHandleRemoteEvent.CallVoid(this, cacheEvent);
		}
	}
}
