#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Program
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
using System.Configuration.Install;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceProcess;
using TcmCDService.Logging;
using TcmCDService.Extensions;
using System.Reflection;

namespace TcmCDService
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(String[] args)
		{
			if (Environment.UserInteractive)
			{
				if (args != null && args.Length == 1 && args[0].StartsWith("-i", StringComparison.OrdinalIgnoreCase))
				{
					Configuration.Installer.Install(Assembly.GetExecutingAssembly().Location);
					return;
				}

				if (args != null && args.Length == 1 && args[0].StartsWith("-u", StringComparison.OrdinalIgnoreCase))
				{
					Configuration.Installer.Uninstall(Assembly.GetExecutingAssembly().Location);
					return;
				}

				Console.WriteLine("[i] Starting TcmCDService");

				using (Service service = new Service())
				{
					using (ServiceHost serviceHost = new ServiceHost(service))
					{
						Logger.Info("TcmCDService starting...");

						serviceHost.Open();
						serviceHost.PrintEndpoints();

						Console.WriteLine("\n[i] Press any key to exit.");
						Console.ReadKey();

						Logger.Info("TcmCDService stopping...");

						serviceHost.Close();
					}
				}
			}
			else
				ServiceBase.Run(new WindowsService());
		}
	}
}
