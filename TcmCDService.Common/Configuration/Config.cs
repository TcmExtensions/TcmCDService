#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Config
// ---------------------------------------------------------------------------------
//	Date Created	: April 3, 2014
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
using System.Configuration;
using System.IO;

namespace TcmCDService.Configuration
{
	/// <summary>
	/// <see cref="Config" /> exposes TcmCDService.CacheClient configuration options
	/// </summary>
	public class Config : System.Configuration.ConfigurationSection
	{
		private static Config mConfigurationSection = null;

		/// <summary>
		/// Determines whether <see cref="P:tridionPath" /> is a valid Tridion home directory
		/// </summary>
		/// <param name="tridionPath">Path to a tridion installation</param>
		/// <returns><c>true</c> if a valid other; otherwise <c>false</c></returns>
		private static Boolean IsValidTridionHome(String tridionPath)
		{
			if (String.IsNullOrEmpty(tridionPath) || !Directory.Exists(tridionPath))
				return false;

			bool hasCore = File.Exists(Path.Combine(tridionPath, @"lib\cd_core.jar"));
			bool hasModel = File.Exists(Path.Combine(tridionPath, @"lib\cd_model.jar"));

			if (!hasCore || !hasModel)
				return false;

			bool hasBroker = File.Exists(Path.Combine(tridionPath, @"config\cd_broker_conf.xml"));
			bool hasStorage = File.Exists(Path.Combine(tridionPath, @"config\cd_storage_conf.xml"));

			return (hasBroker || hasStorage);
		}

		/// <summary>
		/// Determines whether <see cref="P:tridionPath" /> is a valid Tridion home directory
		/// </summary>
		/// <param name="configurationFolder">Path to a tridion configuration folder</param>
		/// <returns><c>true</c> if a valid other; otherwise <c>false</c></returns>
		private static Boolean IsValidTridionConfig(String configurationFolder)
		{
			if (String.IsNullOrEmpty(configurationFolder) || !Directory.Exists(configurationFolder))
				return false;

			bool hasBroker = File.Exists(Path.Combine(configurationFolder, "cd_broker_conf.xml"));
			bool hasStorage = File.Exists(Path.Combine(configurationFolder, "cd_storage_conf.xml"));

			return (hasBroker || hasStorage);
		}

		/// <summary>
		/// Retrieves the <see cref="ConfigurationSection" /> instance
		/// </summary>
		/// <value>
		/// <see cref="ConfigurationSection" /> instance
		/// </value>
		public static Config Instance
		{
			get
			{
				if (mConfigurationSection == null)
					mConfigurationSection = ConfigurationManager.GetSection("TcmCDService") as Config;

				return mConfigurationSection;
			}
		}

		/// <summary>
		/// Retrieves the configured Tridion home folder
		/// </summary>
		/// <value>
		/// Tridion home folder or <see cref="P:String.Empty" /> if no valid Tridion home could be found.
		/// </value>
		[ConfigurationProperty("tridionHome", DefaultValue = "", IsRequired = false)]
		public String TridionHome
		{
			get
			{
				String tridionHome = (String)base["tridionHome"];

				if (IsValidTridionHome(tridionHome))
					return Path.GetFullPath(tridionHome);

				return String.Empty;
			}
		}

		/// <summary>
		/// Retrieves the Tridion configuration folder
		/// </summary>
		/// <value>
		/// Tridion configuration folder or <see cref="P:String.Empty" /> if no valid Tridion configuration folder could be found.
		/// </value>
		[ConfigurationProperty("tridionConfiguration", DefaultValue = "", IsRequired = false)]
		public String TridionConfiguration
		{
			get
			{
				String tridionConfiguration = (String)base["tridionConfiguration"];

				if (IsValidTridionConfig(tridionConfiguration))
					return Path.GetFullPath(tridionConfiguration);

				return String.Empty;
			}
		}

		/// <summary>
		/// Gets the default cache expiry in minutes if none is provided by the configured <see cref="T:TcmCDService.CacheTypes.CacheType" />
		/// </summary>
		/// <value>
		/// The default cache expiry in minutes if none is provided by the configured <see cref="T:TcmCDService.CacheTypes.CacheType" />
		/// </value>
		[ConfigurationProperty("defaultCacheExpiry", DefaultValue = 5, IsRequired = false)]
		public int DefaultCacheExpiry
		{
			get
			{
				return (int)base["defaultCacheExpiry"];
			}
		}

		/// <summary>
		/// Gets a value indicating whether timestamps in logs and status files are local time or UTC
		/// Defaults to UTC
		/// </summary>
		/// <value>
		///   <c>true</c> if timestamps are to be in local time; otherwise, <c>false</c>.
		/// </value>
		[ConfigurationProperty("useLocalTime", DefaultValue = false, IsRequired = false)]
		public Boolean LocalTime
		{
			get
			{
				return (Boolean)base["useLocalTime"];
			}
		}

		/// <summary>
		/// Gets the configured collection of healthcheck types configured as part of the healthcheck service
		/// </summary>
		/// <value>
		/// Healthcheck types to configured as part of the healthcheck service
		/// </value>
		[ConfigurationProperty("healthCheckTypes")]
		[ConfigurationCollection(typeof(HealthCheckTypeElement), AddItemName = "healthCheckType")]
		internal ConfigurationCollection<HealthCheckTypeElement> HealthCheckTypes
		{
			get
			{
				return (ConfigurationCollection<HealthCheckTypeElement>)this["healthCheckTypes"];
			}
		}
		
		[ConfigurationProperty("cacheType", IsRequired = false, DefaultValue = null)]
		internal CacheTypeElement CacheType
		{
			get
			{
				return base["cacheType"] as CacheTypeElement;
			}
		}
	}

	/// <summary>
	/// <see cref="CacheTypeElement" /> defines the configuration for a <see cref="TcmCDService.CacheTypes.CacheType" />
	/// </summary>
	public class CacheTypeElement : ConfigurationElement
	{
		[ConfigurationProperty("type", IsRequired = true, IsKey = true)]
		internal String CacheType
		{
			get
			{
				return base["type"] as String;
			}
		}

		[ConfigurationProperty("", IsDefaultCollection = true)]
		[ConfigurationCollection(typeof(ConfigurationKeyValueElement), AddItemName = "setting")]
		internal ConfigurationCollection<ConfigurationKeyValueElement> Settings
		{
			get
			{
				return (ConfigurationCollection<ConfigurationKeyValueElement>)this[""];
			}
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public override String ToString()
		{
			return this.CacheType;
		}
	}

	/// <summary>
	/// <see cref="HealthCheckTypeElement" /> allows configuration of a single <see cref="HealthCheckTypeElement" /> derived class
	/// </summary>
	public class HealthCheckTypeElement : ConfigurationElement
	{
		/// <summary>
		/// Gets the type of the <see cref="HealthCheckTypeElement" /> 
		/// </summary>
		/// <value>
		/// Type of the <see cref="HealthCheckTypeElement" /> 
		/// </value>
		[ConfigurationProperty("type", IsRequired = true, IsKey = true)]
		internal String HealthCheckType
		{
			get
			{
				return (String)base["type"];
			}
		}

		/// <summary>
		/// Gets <see cref="T:TcmCDService.Configuration.ConfigurationCollection{TcmCDService.Configuration.ConfigurationKeyValueElement}" /> settings
		/// </summary>
		/// <value>
		/// <see cref="T:TcmCDService.Configuration.ConfigurationCollection{TcmCDService.Configuration.ConfigurationKeyValueElement}" /> settings
		/// </value>
		[ConfigurationProperty("settings", IsDefaultCollection = true)]
		[ConfigurationCollection(typeof(ConfigurationKeyValueElement), AddItemName = "setting")]
		internal ConfigurationCollection<ConfigurationKeyValueElement> Settings
		{
			get
			{
				return (ConfigurationCollection<ConfigurationKeyValueElement>)this["settings"];
			}
		}

		/// <summary>
		/// Gets the configured collection of healthcheck types configured as part of the healthcheck service
		/// </summary>
		/// <value>
		/// Healthcheck types to configured as part of the healthcheck service
		/// </value>
		[ConfigurationProperty("healthChecks")]
		[ConfigurationCollection(typeof(HealthCheckElement), AddItemName = "healthCheck")]
		internal ConfigurationCollection<HealthCheckElement> HealthChecks
		{
			get
			{
				return (ConfigurationCollection<HealthCheckElement>)this["healthChecks"];
			}
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public override String ToString()
		{
			return this.HealthCheckType;
		}
	}

	/// <summary>
	/// <see cref="HealthCheckElement" /> defines the configuration for healthchecks to execute
	/// </summary>
	public class HealthCheckElement : ConfigurationElement
	{
		/// <summary>
		/// Gets the name of the <see cref="HealthCheckElement" /> 
		/// </summary>
		/// <value>
		/// Name of the <see cref="HealthCheckElement" /> 
		/// </value>
		[ConfigurationProperty("name", IsRequired = true, IsKey = true)]
		public String Name
		{
			get
			{
				return (String)base["name"];
			}
		}

		/// <summary>
		/// Gets the <see cref="T:TcmCDService.Comon.Configuration.HealthCheckType" /> to execute
		/// </summary>
		/// <value>
		/// <see cref="T:TcmCDService.Comon.Configuration.HealthCheckType" /> to execute
		/// </value>
		[ConfigurationProperty("type", IsRequired = true)]
		public HealthCheckType HealthCheckType
		{
			get
			{
				return (HealthCheckType)base["type"];
			}
		}

		/// <summary>
		/// Gets the Tridion content manager uri to execute the healthcheck against
		/// </summary>
		/// <value>
		/// Tridion content manager uri to execute the healthcheck against
		/// </value>
		[ConfigurationProperty("uri", IsRequired = true)]
		public String Uri
		{
			get
			{
				return base["uri"] as String;
			}
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public override String ToString()
		{
			return String.Format("{0} (Uri {1})", this.Name, this.Uri);
		}
	}
}
