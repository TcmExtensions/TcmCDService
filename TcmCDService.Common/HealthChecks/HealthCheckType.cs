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
using TcmCDService.Configuration;
using TcmCDService.Logging;

namespace TcmCDService.HealthChecks
{
	/// <summary>
	/// <see cref="HealthCheckType" /> is an abstract base class for all healthchecks
	/// </summary>
	public abstract class HealthCheckType : IDisposable
	{
		private Settings mSettings;
		private IEnumerable<HealthCheckElement> mHealthChecks;

		/// <summary>
		/// Gets the <see cref="T:TcmCDService.Configuration.Settings" />
		/// </summary>
		/// <value>
		/// The <see cref="T:TcmCDService.Configuration.Settings" />
		/// </value>
		protected Settings Settings
		{
			get
			{
				return mSettings;
			}
		}

		/// <summary>
		/// Gets the <see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Configuration.HealthCheckElement}" />
		/// </summary>
		/// <value>
		/// The <see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Configuration.HealthCheckElement}" />
		/// </value>
		protected IEnumerable<HealthCheckElement> HealthChecks
		{
			get
			{
				return mHealthChecks;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HealthCheckType" /> class.
		/// </summary>
		/// <param name="configuration"><see cref="T:TcmCDService.Configuration.HealthCheckTypeElement" /></param>
		protected HealthCheckType(HealthCheckTypeElement configuration)
		{
			mHealthChecks = configuration.HealthChecks;
			mSettings = new Settings(configuration.Settings);
		}

		/// <summary>
		/// Load all configured <see cref="HealthCheckType" />
		/// </summary>
		public static IEnumerable<HealthCheckType> LoadHealthCheckTypes()
		{
			List<HealthCheckType> loadedTypes = new List<HealthCheckType>();

			if (Config.Instance != null)
			{
				foreach (HealthCheckTypeElement healthCheckTypeConfig in Config.Instance.HealthCheckTypes)
				{
					try
					{
						Type healthCheckType = Type.GetType(healthCheckTypeConfig.HealthCheckType);

						if (healthCheckType != null)
							loadedTypes.Add((HealthCheckType)Activator.CreateInstance(healthCheckType, healthCheckTypeConfig));
					}
					catch (Exception ex)
					{
						Logger.Error("Error initializing HealthCheckType \"{0}\".", ex, healthCheckTypeConfig.HealthCheckType);
					}
				}
			}

			return loadedTypes;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(Boolean disposing)
		{
			if (disposing)
			{
			}
		}
	}
}
