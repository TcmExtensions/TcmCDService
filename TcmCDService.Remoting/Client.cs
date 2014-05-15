#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Client
// ---------------------------------------------------------------------------------
//	Date Created	: April 6, 2014
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
using System.Linq;
using System.ServiceModel;
using System.Text;
using TcmCDService.Remoting.Configuration;

namespace TcmCDService.Remoting
{
	/// <summary>
	/// <see cref="Client{T}" /> is a wrapper around <see cref="I:System.ServiceModel.IClientChannel" />
	/// </summary>
	/// <typeparam name="T">Webservice interface class</typeparam>
	internal class Client<T> : IDisposable where T : class
	{
		private static ChannelFactory<T> mChannelFactory = null;
		private T mService;

		/// <summary>
		/// Initializes a new instance of the <see cref="Client{T}" /> class.
		/// </summary>
		public Client()
		{
			// Create a cached channel factory
			if (mChannelFactory == null)
				mChannelFactory = new ChannelFactory<T>(Config.Instance.Endpoint);

			mService = mChannelFactory.CreateChannel();
		}

		/// <summary>
		/// Returns the wrapped <see cref="I:System.ServiceModel.IClientChannel" />
		/// </summary>
		/// <value>
		/// The wrapped <see cref="I:System.ServiceModel.IClientChannel" />
		/// </value>
		public T Service
		{
			get
			{
				return mService;
			}
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
				if (mService != null)
				{
					IClientChannel clientChannel = mService as IClientChannel;

					if (clientChannel.State == CommunicationState.Faulted)
						clientChannel.Abort();
					else
						clientChannel.Close();

					clientChannel.Dispose();
				}
			}
		}
	}
}
