using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace TcmCDService.Configuration
{
	/// <summary>
	/// <see cref="ConfigurationCollection{T}" /> is a generic class to handle configuration collections
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ConfigurationCollection<T> : ConfigurationElementCollection, IEnumerable<T> where T : ConfigurationElement, new()
	{
		private readonly List<T> mElements;

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationCollection{T}" /> class.
		/// </summary>
		internal ConfigurationCollection()
		{
			mElements = new List<T>();
		}

		/// <summary>
		/// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement" />.
		/// </summary>
		/// <returns>
		/// A new <see cref="T:System.Configuration.ConfigurationElement" />.
		/// </returns>
		protected override ConfigurationElement CreateNewElement()
		{
			T element = new T();
			mElements.Add(element);

			return element;
		}

		/// <summary>
		/// Gets the element key for a specified configuration element when overridden in a derived class.
		/// </summary>
		/// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement" /> to return the key for.</param>
		/// <returns>
		/// An <see cref="T:System.Object" /> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement" />.
		/// </returns>
		protected override Object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
				throw new ArgumentNullException("element");

			return element.ToString();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
		/// </returns>
		public new IEnumerator<T> GetEnumerator()
		{
			return mElements.GetEnumerator();
		}
	}
}
