#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Service Extensions
// ---------------------------------------------------------------------------------
//	Date Created	: March 30, 2014
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
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.Text;
using TcmCDService.Logging;

namespace TcmCDService.Extensions
{
	/// <summary>
	/// <see cref="ServiceExtensions" /> provides functionality to output <see cref="T:System.ServiceModel.ServiceHost" /> information
	/// </summary>
	internal static class ServiceExtensions
	{
		/// <summary>
		/// Outputs endpoint information for a <see cref="T:System.ServiceModel.ServiceHost" />.
		/// </summary>
		/// <param name="serviceHost"><see cref="T:System.ServiceModel.ServiceHostBase" /></param>
		/// <param name="textWriter"><see cref="T:System.IO.TextWriter" /> to write output to.</param>
		/// <exception cref="System.ArgumentNullException">textWriter</exception>
		internal static void PrintEndpoints(this ServiceHostBase serviceHost, TextWriter textWriter)
		{
			if (textWriter == null)
				throw new ArgumentNullException("textWriter");

			if (serviceHost != null)
			{
				textWriter.WriteLine("Service listening on...");
				
				foreach (ChannelDispatcher channelDispatcher in serviceHost.ChannelDispatchers)
				{
					foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints)
					{
						textWriter.WriteLine("\t{0}", endpointDispatcher.EndpointAddress.Uri);
					}
				}
			}
		}

		/// <summary>
		/// Outputs endpoint information for a <see cref="T:System.ServiceModel.ServiceHost" />.
		/// </summary>
		/// <param name="serviceHost"><see cref="T:System.ServiceModel.ServiceHostBase" /></param>		
		/// <exception cref="System.ArgumentNullException">textWriter</exception>
		internal static void PrintEndpoints(this ServiceHostBase serviceHost)
		{
			using (StringWriter stringWriter = new StringWriter())
			{
				serviceHost.PrintEndpoints(stringWriter);

				if (Environment.UserInteractive)
					Console.Write(stringWriter.ToString());

				Logger.Info(stringWriter.ToString());
			}
		}
	}
}
