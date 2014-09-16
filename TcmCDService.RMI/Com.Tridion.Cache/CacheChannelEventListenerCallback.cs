#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Cache Channel Event Listener Callback
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
using System.IO;
using System.Reflection;
using Codemesh.JuggerNET;

namespace Com.Tridion.Cache
{
	/// <summary>
	/// <see cref="CacheChannelEventListenerCallback" /> implements a callback class for JuggerNET to call back from Java into .NET
	/// </summary>
	public class CacheChannelEventListenerCallback : JuggerNETProxyObject
	{
		// JuggerNet requires an internal JavaClass field of "_cmj_theClass" as per CodeMesh.JuggerNET.JvmLoader.ProcessType
		private static JavaClass _cmj_theClass = new JavaClass("com/tridion/tcmcdservice/nativeadapters/CacheChannelEventListenerCallback", typeof(CacheChannelEventListenerCallback));
		
		private static JavaMethod mConstructor;

		// GenericCallbacks need to be in memory for as long as the CacheChannelEventListenerCallback is alive
		private GenericCallback mConnectCallback;
		private GenericCallback mDisconnectCallback;
		private GenericCallback mRemoteEventCallback;
		
		private ICacheChannelEventListener mCacheChannelEventListener;

		/// <summary>
		/// Initializes the <see cref="CacheChannelEventListenerCallback" /> class.
		/// </summary>
		static CacheChannelEventListenerCallback()
		{
			// Load the Java callback class bytecode from Com.Tridion.Cache.CacheChannelEventListenerCallback
			using (Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(CacheChannelEventListenerCallback), "CacheChannelEventListenerCallback"))
			{
				Byte[] buffer = new Byte[resourceStream.Length];
				resourceStream.Read(buffer, 0, (int)resourceStream.Length);

				_cmj_theClass.ByteCode = buffer;
			}

			mConstructor = new JavaMethod(_cmj_theClass, null, "<init>", "(J[J)V", false);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheChannelEventListenerCallback"/> class.
		/// </summary>
		/// <param name="cacheChannelEventListener">The associated <see cref="I:Com.Tridion.Cache.ICacheChannelEventListener" /></param>
		public CacheChannelEventListenerCallback(ICacheChannelEventListener cacheChannelEventListener)
		{
			mCacheChannelEventListener = cacheChannelEventListener;

			mConnectCallback = new GenericCallback((out int return_type, out jvalue return_value, IntPtr input) =>
			{
				try
				{
					mCacheChannelEventListener.HandleConnect();

					// Void
					return_value = new jvalue();
					return_type = 0;
				}
				catch (Exception exception)
				{
					// Wrap the exception for Java
					return_value = jvalue.CreateCBRetVal(exception);

					// Object
					return_type = 1;
				}

				return 0;
			});

			mDisconnectCallback = new GenericCallback((out int return_type, out jvalue return_value, IntPtr input) =>
			{
				try
				{
					mCacheChannelEventListener.HandleConnect();

					// Void
					return_value = new jvalue();
					return_type = 0;
				}
				catch (Exception exception)
				{
					// Wrap the exception for Java
					return_value = jvalue.CreateCBRetVal(exception);

					// Object
					return_type = 1;
				}

				return 0;
			});

			mRemoteEventCallback = new GenericCallback((out int return_type, out jvalue return_value, IntPtr input) =>
			{
				try
				{
					CacheEvent typedInstance = (CacheEvent)JavaClass.GetTypedInstance(typeof(CacheEvent), jvalue.From(input).l);

					mCacheChannelEventListener.HandleRemoteEvent(typedInstance);

					// Void
					return_value = new jvalue();
					return_type = 0;
				}
				catch (Exception exception)
				{
					// Wrap the exception for Java
					return_value = jvalue.CreateCBRetVal(exception);

					// Object
					return_type = 1;
				}

				return 0;
			});

			base.JObject = mConstructor.construct(0, mConnectCallback, mDisconnectCallback, mRemoteEventCallback);
		}
	}
}

