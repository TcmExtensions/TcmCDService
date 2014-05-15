#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Windows Service
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
using System.ServiceProcess;
using TcmCacheService.Configuration;
using TcmCDService.CacheTypes;
using TcmCDService.Logging;

namespace TcmCacheService
{
	/// <summary>
	/// <see cref="WindowsService" /> provides a windows hosted service around <see cref="T:TcmCDService.Service" />
	/// </summary>
	public partial class WindowsService : ServiceBase
	{
		private ZeroMQBroker mZeroMQBroker;

		/// <summary>
		/// Initializes a new instance of the <see cref="WindowsService"/> class.
		/// </summary>
		public WindowsService()
		{
			InitializeComponent();
			mZeroMQBroker = new ZeroMQBroker(new TcmCDService.Configuration.Settings(Config.Instance.Settings));
			
		}

		/// <summary>
		/// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
		/// </summary>
		/// <param name="args">Data passed by the start command.</param>
		protected override void OnStart(String[] args)
		{
			Logger.Info("TcmCacheService starting...");
			mZeroMQBroker.Connect();
		}

		/// <summary>
		/// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
		/// </summary>
		protected override void OnStop()
		{
			Logger.Info("TcmCacheService stopping...");

			if (mZeroMQBroker != null)
			{
				mZeroMQBroker.Disconnect();
				mZeroMQBroker.Dispose();
			}
		}
	}
}
