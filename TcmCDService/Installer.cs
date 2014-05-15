#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Installer
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
using System.ComponentModel;
using System.ServiceProcess;

namespace TcmCDService
{
	/// <summary>
	/// <see cref="Installer" /> allows installation of the <see cref="TcmCDService" />
	/// </summary>
	[RunInstaller(true)]
	public partial class Installer : System.Configuration.Install.Installer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Installer"/> class.
		/// </summary>
		public Installer()
		{
			InitializeComponent();

			ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller()
			{
				Account =  ServiceAccount.LocalSystem
			};

			ServiceInstaller serviceInstaller = new ServiceInstaller()
			{
				ServiceName = "TcmCDService",
				Description = "Tridion Content Delivery Service",
				DisplayName = "TcmCDService",
				StartType = ServiceStartMode.Automatic,
				DelayedAutoStart = true
			};

			Installers.Add(serviceProcessInstaller);
			Installers.Add(serviceInstaller);			
		}
	}
}
