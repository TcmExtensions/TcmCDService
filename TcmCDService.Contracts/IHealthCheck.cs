#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: IHealthCheck
// ---------------------------------------------------------------------------------
//	Date Created	: May 16, 2014
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
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;

namespace TcmCDService.Contracts
{
	/// <summary>
	/// <see cref="IHealthCheck" /> defines the healthcheck service contract for <see cref="T:TcmCDService.Service" />
	/// </summary>
	[ServiceContract(Namespace = "urn:TcmCDService", Name = "TcmCDService-HealthCheck")]
	public interface IHealthCheck
	{
		/// <summary>
		/// Executes a healthcheck using the configured healthcheck parameters
		/// </summary>
		/// <returns>text/plain content, indicating "Success" or "Failure"</returns>
		[WebGet(UriTemplate = "/")]
		[OperationContract] 
		Message HealthCheck();
	}
}
