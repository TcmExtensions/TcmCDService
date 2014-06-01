#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Base HealthCheck
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
using System.IO;
using System.Linq;
using System.Text;
using TcmCDService.Configuration;
using Tridion.ContentDelivery.DynamicContent;

namespace TcmCDService.HealthChecks
{
	/// <summary>
	/// <see cref="BaseHealthCheck" /> integrates <see cref="T:TcmCDService.HealthChecks.HealthCheckType" /> with the Tridion Content Delivery healthcheck logic.
	/// </summary>
	public abstract class BaseHealthCheck : HealthCheckType
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseHealthCheck" /> class.
		/// </summary>
		/// <param name="configuration"><see cref="T:TcmCDService.Configuration.HealthCheckTypeElement" /></param>
		protected BaseHealthCheck(HealthCheckTypeElement configuration): base(configuration)
		{
		}

		protected void ExecuteHealthChecks(TextWriter textWriter)
		{
			foreach (HealthCheckElement healthCheck in HealthChecks)
			{
				textWriter.Write("{0,-19:dd-MM HH:mm:ss.ff}{1}: ", Config.Instance.LocalTime ? DateTime.Now : DateTime.UtcNow, healthCheck.ToString());

				switch (healthCheck.HealthCheckType)
				{
					case Configuration.HealthCheckType.ComponentLink:
						
						if (!String.IsNullOrEmpty(ContentDelivery.Web.Linking.ComponentLinkCache.ResolveComponentLink(healthCheck.Uri)))
							textWriter.WriteLine("Success");
						else
							textWriter.WriteLine("Failure");
						break;
					case Configuration.HealthCheckType.ComponentPresentation:
						

						using (ComponentPresentation componentPresentation = ContentDelivery.DynamicContent.ComponentPresentationCache.GetComponentPresentationWithHighestPriority(healthCheck.Uri))
						{
							if (componentPresentation != null && !String.IsNullOrEmpty(componentPresentation.Content))
								textWriter.WriteLine("Success");
							else
								textWriter.WriteLine("Failure");
						}
						break;
					default:
						textWriter.WriteLine("Unknown healthcheck type");
						break;
				}
			}
		}
	}
}
