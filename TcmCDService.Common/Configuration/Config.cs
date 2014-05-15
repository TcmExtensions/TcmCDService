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

		[ConfigurationProperty("cacheType", IsRequired = false, DefaultValue = null)]
		internal CacheTypeElement CacheType
		{
			get
			{
				return base["cacheType"] as CacheTypeElement;
			}
		}
	}

	public class CacheTypeElement : ConfigurationElement
	{
		[ConfigurationProperty("type", IsRequired = true)]
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
	}
}
