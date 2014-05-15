#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Unicast Remote Object
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

namespace Java.Rmi.Server
{
	/// <summary>
	/// <see cref="UnicastRemoteObject" /> wraps around java.rmi.server.UnicastRemoteObject
	/// </summary>
	/// <remarks>
	/// Only the "unexportObject" functionality is exposed.
	/// </remarks>
	public class UnicastRemoteObject : JuggerNETProxyObject
	{
		// JuggerNet requires an internal JavaClass field of "_cmj_theClass" as per CodeMesh.JuggerNET.JvmLoader.ProcessType
		private static JavaClass _cmj_theClass = JavaClass.RegisterClass("java.rmi.server.UnicastRemoteObject", typeof(UnicastRemoteObject));

		private static JavaMethod mUnexportObject = new JavaMethod(_cmj_theClass, typeof(bool), "unexportObject", "(Ljava/rmi/Remote;Z)Z", true);

		/// <summary>
		/// Prevents a default instance of the <see cref="UnicastRemoteObject" /> class from being created.
		/// </summary>
		private UnicastRemoteObject()
		{
		}

		/// <summary>
		/// Unexports a previously exported object
		/// </summary>
		/// <param name="obj"><see cref="T:Java.Lang.Object" /></param>
		/// <param name="force">if set to <c>true</c> [force unexport].</param>
		/// <returns><c>true</c> if unexported, otherwise <c>false</c>.</returns>
		public static bool UnexportObject(JuggerNETProxyObject javaProxyObject, bool force)
		{
			jvalue[] args = new jvalue[2];

			using (JavaMethodArguments arguments = new JavaMethodArguments(args).Add(javaProxyObject).Add(force))
			{
				return mUnexportObject.CallBool(null, arguments);
			}
		}
	}
}
