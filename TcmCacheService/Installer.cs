#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Installer
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
using System.ComponentModel;
using System.ServiceProcess;

namespace TcmCacheService
{
	/// <summary>
	/// <see cref="Installer" /> allows installation of the <see cref="TcmCacheService" />
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
				Account =  ServiceAccount.NetworkService
			};

			ServiceInstaller serviceInstaller = new ServiceInstaller()
			{
				ServiceName = "TcmCacheService",
				Description = "Tridion Cache Channel Service",
				DisplayName = "TcmCacheService",
				StartType = ServiceStartMode.Automatic
			};

			Installers.Add(serviceProcessInstaller);
			Installers.Add(serviceInstaller);			
		}
	}
}
