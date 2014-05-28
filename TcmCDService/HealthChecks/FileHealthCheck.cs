#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: File HealthCheck
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
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TcmCDService.Configuration;
using TcmCDService.Logging;

namespace TcmCDService.HealthChecks
{
	/// <summary>
	/// <see cref="FileHealthCheck" /> exposes healthcheck functionality as status file on the system
	/// </summary>
	public class FileHealthCheck : BaseHealthCheck
	{
		private CancellationTokenSource mCancellationTokenSource;
		private Task mTask;
		private String mPath;
		private int mInterval;

		private void ScheduledHealthCheck(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					using (FileStream fileStream = new FileStream(mPath, FileMode.Create, FileAccess.Write, FileShare.Read))
					{
						using (StreamWriter streamWriter = new StreamWriter(fileStream))
						{
							base.ExecuteHealthChecks(streamWriter);
							streamWriter.Flush();
							fileStream.Flush(true);
						}
					}
				}
				catch (OperationCanceledException)
				{
					return;
				}
				catch (Exception ex)
				{
					Logger.Error("FileHealthCheck", ex);
				}
				
				Thread.Sleep(mInterval * 1000);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FileHealthCheck" /> class.
		/// </summary>
		/// <param name="configuration"><see cref="T:TcmCDService.Configuration.HealthCheckTypeElement" /></param>
		public FileHealthCheck(HealthCheckTypeElement configuration): base(configuration)
		{
			mPath = Settings.Get<String>("path");

			if (String.IsNullOrEmpty(mPath))
				throw new ConfigurationErrorsException("No configured \"path\" for FileHealthCheck");

			try
			{
				Directory.CreateDirectory(Path.GetDirectoryName(mPath));
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException("Cannot create \"path\" for FileHealthCheck", ex);
			}

			mInterval = Settings.Get<int>("interval");

			if (String.IsNullOrEmpty(mPath))
				throw new ConfigurationErrorsException("No configured \"interval\" for FileHealthCheck");

			mCancellationTokenSource = new CancellationTokenSource();

			mTask = new Task(() => ScheduledHealthCheck(mCancellationTokenSource.Token), TaskCreationOptions.LongRunning);
			mTask.Start();
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected override void Dispose(Boolean disposing)
		{
			if (disposing)
			{
				mCancellationTokenSource.Cancel();
				mTask.Wait(5000);
			}
			
			base.Dispose(disposing);
		}
	}
}
