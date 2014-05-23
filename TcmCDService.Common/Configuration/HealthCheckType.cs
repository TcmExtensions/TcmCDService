#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Healthcheck Type
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
using System.Text;

namespace TcmCDService.Configuration
{
	/// <summary>
	/// <see cref="HealthCheckType" /> determines the type of healthcheck to execute when requested
	/// </summary>
	public enum HealthCheckType
	{
		/// <summary>
		/// Resolve a component link
		/// </summary>
		ComponentLink,
		/// <summary>
		/// Retrieve a dynamic component presentation
		/// </summary>
		ComponentPresentation
	}
}
