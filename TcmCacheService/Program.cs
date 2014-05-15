#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Program
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
using System.Collections.Generic;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using TcmCacheService.Configuration;
using TcmCDService.CacheTypes;
using TcmCDService.Logging;

namespace TcmCacheService
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
					TcmCDService.Configuration.Installer.Install(Assembly.GetExecutingAssembly().Location);
					return;
				}

				if (args != null && args.Length == 1 && args[0].StartsWith("-u", StringComparison.OrdinalIgnoreCase))
				{
					TcmCDService.Configuration.Installer.Uninstall(Assembly.GetExecutingAssembly().Location);
					return;
				}

				Console.WriteLine("[i] Starting TcmCacheService");
				Logger.Info("TcmCacheService starting...");

				using (ZeroMQBroker cache = new ZeroMQBroker(new TcmCDService.Configuration.Settings(Config.Instance.Settings)))
				{
					cache.Connect();

					Console.WriteLine("[i] Press any key to exit.");
					Console.ReadKey();

					Logger.Info("TcmCacheService stopping...");
				}

				Console.WriteLine("[i] Stopping TcmCacheService");
			}
			else
				ServiceBase.Run(new WindowsService());
		}
	}
}
