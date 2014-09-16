#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Cache Event
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
using Codemesh.JuggerNET;
using TcmCDService.CacheTypes;

namespace Com.Tridion.Cache
{
	/// <summary>
	/// <see cref="CacheEvent" /> is a proxy class for com.tridion.cache.CacheEvents and receives all Tridion cache event information
	/// </summary>
	public class CacheEvent : JuggerNETProxyObject
	{
		// JuggerNet requires an internal JavaClass field of "_cmj_theClass" as per CodeMesh.JuggerNET.JvmLoader.ProcessType
		private static JavaClass _cmj_theClass = JavaClass.RegisterClass("com.tridion.cache.CacheEvent", typeof(CacheEvent));

		private static JavaMethod mConstructor = new JavaMethod(_cmj_theClass, typeof(CacheEvent), "<init>", "(Ljava/lang/String;Ljava/io/Serializable;I)V", false);
		private static JavaMethod mGetKey = new JavaMethod(_cmj_theClass, typeof(string), "getKey", "()Ljava/io/Serializable;", false);
		private static JavaMethod mGetRegionPath = new JavaMethod(_cmj_theClass, typeof(string), "getRegionPath", "()Ljava/lang/String;", false);
		private static JavaMethod mGetType = new JavaMethod(_cmj_theClass, typeof(int), "getType", "()I", false);

		/// <summary>
		/// Gets <see cref="CacheEvent" /> key as <see cref="T:System.String" />
		/// </summary>
		/// <value>
		/// <see cref="CacheEvent" /> key as <see cref="T:System.String" /> or null
		/// </value>
		public string KeyString
		{
			get
			{
				return mGetKey.CallString(this);
			}
		}

		/// <summary>
		/// Gets <see cref="CacheEvent" /> key as <see cref="T:System.Int32" />
		/// </summary>
		/// <value>
		/// <see cref="CacheEvent" /> key as <see cref="T:System.Int32" /> or 0
		/// </value>
		public int KeyInt
		{
			get
			{
				Java.Lang.Integer valueInt = (Java.Lang.Integer)mGetKey.CallObject(this, typeof(Java.Lang.Integer), false);

				if (valueInt != null)
					return valueInt.IntValue();

				return 0;
			}
		}

		/// <summary>
		/// Gets <see cref="CacheEvent" /> key converted to <see cref="T:System.String" />
		/// </summary>
		/// <value>
		/// <see cref="CacheEvent" /> key converted to <see cref="T:System.String" /> or null
		/// </value>
		public string Key
		{
			get
			{
				string valueString = KeyString;

				if (!string.IsNullOrEmpty(valueString))
					return valueString;

				int valueInt = KeyInt;

				if (valueInt != 0)
					return valueInt.ToString();

				return null;
			}
		}

		/// <summary>
		/// Gets the <see cref="CacheEvent" /> <see cref="T:TcmCDService.CacheTypes.CacheRegion" />
		/// </summary>
		/// <value>
		/// <see cref="CacheEvent" /> <see cref="T:TcmCDService.CacheTypes.CacheRegion" />
		/// </value>
		public CacheRegion Region
		{
			get
			{
				return CacheRegionExtensions.ToCacheRegion(RegionPath);
			}
		}

		/// <summary>
		/// Gets the <see cref="CacheEvent" /> region path
		/// </summary>
		/// <value>
		/// <see cref="CacheEvent" /> region path
		public string RegionPath
		{
			get
			{
				return mGetRegionPath.CallString(this);
			}
		}

		/// <summary>
		/// Gets the <see cref="CacheEvent" /> <see cref="T:TcmCDService.CacheTypes.CacheEventType" />
		/// </summary>
		/// <value>
		/// <see cref="CacheEvent" /> <see cref="T:TcmCDService.CacheTypes.CacheEventType" />
		/// </value>
		public CacheEventType EventType
		{
			get
			{
				return (CacheEventType)mGetType.CallInt(this);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheEvent" /> class.
		/// </summary>
		/// <param name="objectHandle"><see cref="T:CodeMesh.JuggerNET.JNIHandle" /></param>
		public CacheEvent(JNIHandle objectHandle): base(objectHandle)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheEvent" /> class.
		/// </summary>
		/// <param name="regionPath">Cache region path.</param>
		/// <param name="key">Cache key as <see cref="T:System.String" /></param>
		/// <param name="eventType"><see cref="T:TcmCDService.CacheTypes.CacheEventType" /></param>
		public CacheEvent(string regionPath, string key, CacheEventType eventType): base(JNIHandle.NULL)
		{
			jvalue[] args = new jvalue[3];

			using (JavaMethodArguments arguments = new JavaMethodArguments(args).Add(regionPath).Add(key).Add((int)eventType))
			{
				base.JObject = (long)mConstructor.CallObject(this, arguments);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheEvent" /> class.
		/// </summary>
		/// <param name="regionPath">Cache region path.</param>
		/// <param name="key">Cache key as <see cref="T:System.Int32" /></param>
		/// <param name="eventType"><see cref="T:TcmCDService.CacheTypes.CacheEventType" /></param>
		public CacheEvent(string regionPath, int key, CacheEventType eventType): base(JNIHandle.NULL)
		{
			jvalue[] args = new jvalue[3];

			using (JavaMethodArguments arguments = new JavaMethodArguments(args).Add(regionPath).Add(new Java.Lang.Integer(key)).Add((int)eventType))
			{
				base.JObject = (long)mConstructor.CallObject(this, arguments);
			}
		}
	}
}
