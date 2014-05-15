#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Windows Service
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
using System.ServiceModel;
using System.ServiceProcess;
using TcmCDService.Logging;
using TcmCDService.Extensions;

namespace TcmCDService
{
	/// <summary>
	/// <see cref="WindowsService" /> provides a windows hosted service around <see cref="T:TcmCDService.Service" />
	/// </summary>
	public partial class WindowsService : ServiceBase
	{
		private Service mService;
		private ServiceHost mServiceHost;

		/// <summary>
		/// Initializes a new instance of the <see cref="WindowsService"/> class.
		/// </summary>
		public WindowsService()
		{
			InitializeComponent();
			mService = new Service();
			mServiceHost = new ServiceHost(mService);
		}

		/// <summary>
		/// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
		/// </summary>
		/// <param name="args">Data passed by the start command.</param>
		protected override void OnStart(String[] args)
		{
			Logger.Info("TcmCDService starting...");

			mServiceHost.Open();
			mServiceHost.PrintEndpoints();
		}

		/// <summary>
		/// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
		/// </summary>
		protected override void OnStop()
		{
			Logger.Info("TcmCDService stopping...");

			mServiceHost.Close();
			mService.Dispose();
		}
	}
}
