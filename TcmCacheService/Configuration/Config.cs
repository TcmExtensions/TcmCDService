#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Config
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
using System;
using System.Collections.Generic;
using System.Configuration;
using TcmCDService.Configuration;

namespace TcmCacheService.Configuration
{
	/// <summary>
	/// <see cref="Config" /> exposes TcmCacheService configuration options
	/// </summary>
	public class Config : System.Configuration.ConfigurationSection
	{
		private static Config mConfigurationSection = null;

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
					mConfigurationSection = ConfigurationManager.GetSection("TcmCacheService") as Config;

				return mConfigurationSection;
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
