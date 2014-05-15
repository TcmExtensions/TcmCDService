using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TcmCDService.Contracts;

namespace TcmCDService.Remoting
{
	/// <summary>
	/// <see cref="RemoteAPI" /> handles all TcmCDService.Remoting calls and validates their parameters.
	/// </summary>
	/// <remarks>Note that no exception handling is done, so clients are aware of the service connectivity.</remarks>
	internal static class RemoteAPI
	{
		private static Regex mTcmUriExpression = new Regex(@"tcm:\d+-\d+-?\d*-?v?\d*");

		/// <summary>
		/// Validates the given uri <see cref="T:System.String" /> values as valid Tridion content manager uris.
		/// </summary>
		/// <param name="uris">uri <see cref="T:System.String" /> values</param>
		/// <returns><c>true</c>, if all uris are valid Tridion content manager uris or null, otherwise <c>false</c></returns>
		private static Boolean Validate(params String[] uris)
		{
			if (uris != null)
				return uris.All(uri => uri == null || mTcmUriExpression.IsMatch(uri));

			return false;
		}

		/// <summary>
		/// Validates the given id <see cref="T:System.Int" /> values as valid Tridion content manager ids.
		/// </summary>
		/// <param name="uris">id <see cref="T:System.Int" /> values</param>
		/// <returns><c>true</c>, if all ids are valid Tridion content manager uris or null, otherwise <c>false</c></returns>
		private static Boolean Validate(params int[] ids)
		{
			if (ids != null)
				return ids.All(id => id > 0);

			return false;
		}

		/// <summary>
		/// Executes the <see cref="P:remoteCall" /> against the <see cref="TcmCDService" />
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="remoteCall">Remote function call</param>
		/// <param name="uris">Uri parameters to validate</param>
		/// <returns>Remote function call result</returns>
		internal static T Execute<T>(Func<Client<IService>, T> remoteCall, params String[] uris) where T : class
		{
			if (Validate(uris))
			{
				using (Client<IService> client = new Client<IService>())
				{
					return remoteCall(client);
				}
			}

			return null;
		}

		/// <summary>
		/// Executes the <see cref="P:remoteCall" /> against the <see cref="TcmCDService" />
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="remoteCall">Remote function call</param>
		/// <param name="ids">Id parameters to validate</param>
		/// <returns>Remote function call result</returns>
		internal static T Execute<T>(Func<Client<IService>, T> remoteCall, params int[] ids) where T : class
		{
			if (Validate(ids))
			{
				using (Client<IService> client = new Client<IService>())
				{
					return remoteCall(client);
				}
			}

			return null;
		}
	}
}
