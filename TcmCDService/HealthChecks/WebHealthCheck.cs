#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Web HealthCheck
// ---------------------------------------------------------------------------------
//	Date Created	: May 23, 2014
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
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using TcmCDService.Configuration;
using TcmCDService.Contracts;
using TcmCDService.Extensions;

namespace TcmCDService.HealthChecks
{
	/// <summary>
	/// <see cref="WebHealthCheck" /> exposes healthcheck functionality over HTTP
	/// </summary>
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
	public class WebHealthCheck : BaseHealthCheck, IHealthCheck
	{
		private ServiceHost mServicehost;

		/// <summary>
		/// Initializes a new instance of the <see cref="WebHealthCheck" /> class.
		/// </summary>
		/// <param name="configuration"><see cref="T:TcmCDService.Configuration.HealthCheckTypeElement" /></param>
		public WebHealthCheck(HealthCheckTypeElement configuration): base(configuration)
		{
			mServicehost = new ServiceHost(this);
			mServicehost.Open();
		}

		/// <summary>
		/// Executes a healthcheck using the configured healthcheck parameters
		/// </summary>
		/// <returns>text/plain content, indicating "Success" or "Failure"</returns>
		public Message HealthCheck()
		{
			WebOperationContext.Current.OutgoingResponse.Headers[HttpResponseHeader.CacheControl] = "private, max-age=0, no-cache";
			WebOperationContext.Current.OutgoingResponse.Headers[HttpResponseHeader.Pragma] = "no-cache";
			WebOperationContext.Current.OutgoingResponse.Headers[HttpResponseHeader.Expires] = "0";

			return WebOperationContext.Current.CreateTextResponse(
				(textWriter) =>
				{
					textWriter.WriteLine("TcmCDService");
					textWriter.WriteLine();

					OperationContext.Current.Host.PrintEndpoints(textWriter);

					textWriter.WriteLine();
					textWriter.WriteLine(new String('-', 80));
					textWriter.WriteLine();

					base.ExecuteHealthChecks(textWriter);
				},
				"text/plain",
				Encoding.UTF8);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected override void Dispose(Boolean disposing)
		{
			if (disposing)
			{
				mServicehost.Close();
			}

			base.Dispose(disposing);
		}
	}
}
