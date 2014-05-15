using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace TcmCDService.Remoting.Configuration
{
	/// <summary>
	/// <see cref="Config" /> exposes TcmCDService.Remoting configuration options
	/// </summary>
	public class Config : System.Configuration.ConfigurationSection
	{
		private static Config mConfigurationSection = null;

		/// <summary>
		/// Retrieves the <see cref="Config" /> instance
		/// </summary>
		/// <value>
		/// <see cref="Config" /> instance
		/// </value>
		public static Config Instance
		{
			get
			{
				if (mConfigurationSection == null)
					mConfigurationSection = ConfigurationManager.GetSection("TcmCDService.Remoting") as Config;

				return mConfigurationSection;
			}
		}

		[ConfigurationProperty("endpoint", IsRequired = true)]
		public String Endpoint
		{
			get
			{
				return (String)base["endpoint"];
			}
		}
	}
}
