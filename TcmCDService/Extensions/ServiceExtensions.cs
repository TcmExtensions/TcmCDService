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
		/// <param name="serviceHost"><see cref="T:System.ServiceModel.ServiceHost" /></param>
		internal static void PrintEndpoints(this ServiceHost serviceHost)
		{
			if (serviceHost != null)
			{
				StringBuilder stringBuilder = new StringBuilder();

				stringBuilder.AppendLine("Service listening on...");
				
				foreach (ChannelDispatcher channelDispatcher in serviceHost.ChannelDispatchers)
				{
					foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints)
					{
						stringBuilder.AppendFormat("\t{0}\n", endpointDispatcher.EndpointAddress.Uri);
					}
				}

				String output = stringBuilder.ToString();

				if (Environment.UserInteractive)
					Console.Write(output);

				Logger.Info(output);
			}
		}
	}
}
