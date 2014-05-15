using System;
using System.Collections.Generic;
using System.Text;

namespace TcmCDService.Configuration
{
	/// <summary>
	/// <see cref="Settings" /> is a class to store and pass module specific settings from the base configuration
	/// </summary>
	public class Settings
	{
		private Dictionary<String, String> mSettings;

		/// <summary>
		/// Initializes a new instance of the <see cref="Settings"/> class.
		/// </summary>
		/// <param name="queueSettings"><see cref="T:EKIT.Configuration.ConfigurationCollection{KeyValueElement}" /></param>
		/// <exception cref="System.ArgumentNullException">configuration</exception>
		public Settings(ConfigurationCollection<ConfigurationKeyValueElement> settings)
		{
			if (settings == null)
				throw new ArgumentNullException("settings");

			/*
			mSettings = settings
				.ToDictionary(setting => setting.Key,
							  setting => setting.Value,
							  StringComparer.OrdinalIgnoreCase);
			 */

			mSettings = new Dictionary<String, String>(StringComparer.OrdinalIgnoreCase);

			foreach (ConfigurationKeyValueElement setting in settings)
			{
				mSettings.Add(setting.Key, setting.Value);
			}
		}

		/// <summary>
		/// Retrieves the setting for the specified <see cref="P:key"/>
		/// </summary>
		/// <param name="key">Setting key</param>
		/// <param name="defaultValue">Default value</param>
		/// <returns>Setting value or default value</returns>
		public T Get<T>(String key, T defaultValue)
		{
			String value;

			if (mSettings.TryGetValue(key, out value))
				return (T)Convert.ChangeType(value, typeof(T));

			return defaultValue;
		}

		/// <summary>
		/// Retrieves the setting for the specified <see cref="P:key"/>
		/// </summary>
		/// <param name="key">Setting key</param>
		/// <returns>Setting value or default value</returns>
		public T Get<T>(String key)
		{
			return Get<T>(key, default(T));
		}
	}
}
