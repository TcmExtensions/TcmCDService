#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: RolloverTraceListener
// ---------------------------------------------------------------------------------
//	Date Created	: February 28, 2013
//	Author			: Rob van Oostenrijk
// ---------------------------------------------------------------------------------
//
////////////////////////////////////////////////////////////////////////////////////
#endregion
using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.Configuration;
using TcmCDService.Configuration;

namespace TcmCDService.Logging
{
	/// <summary>
	/// <para><see cref="RolloverTraceListener"/> implements a <see cref="T:System.Diagnostics.TraceListener" /> which rolls over
	/// to a new file every day, creating filenames of the format: &lt;filename&gt;_20071101.log".</para>
	/// </summary>
	/// <remarks>
	/// <para>The <see cref="RolloverTraceListener" /> allows logging to files which roll over automatically on a daily basis.
	/// This allows for easier debug log file management.</para>
	/// 
	/// <para>The trace listener is configured in the web.config &lt;system.diagnostics&gt; section as follows:</para>
	/// <code>
	///   &lt;trace autoflush="true" indentsize="2"&gt;
	///     &lt;listeners&gt;
	///       &lt;remove name="Default"/&gt;
	///       &lt;add name="FileLogListener" type="TcmCDService.Logging.RolloverTraceListener, TcmCDService" initializeData="c:\data\logs\TcmCDService.log" /&gt;
	///     &lt;/listeners&gt;
	///   &lt;/trace&gt;
	/// </code>
	/// </remarks>
	public class RolloverTraceListener : System.Diagnostics.TraceListener
	{
		private const int LOG_SIZE = 5242880;

		// Fields
		private String mFileName;
		private String mCurrentFile;
		private int mPruneAge;
		internal TextWriter mWriter;
		DateTime mCurrentDate;

		// Methods
		/// <summary>
		/// Initializes a new instance of the <see cref="RolloverTraceListener"/> class.
		/// </summary>
		public RolloverTraceListener()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RolloverTraceListener"/> class.
		/// </summary>
		/// <param name="fileName">Log filename</param>
		public RolloverTraceListener(String initializeData)
		{
			String[] data = initializeData.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

			mFileName = data[0];
			mPruneAge = 30;

			if (data.Length > 1)
				int.TryParse(data[1], out mPruneAge);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RolloverTraceListener"/> class.
		/// </summary>
		/// <param name="fileName">Log filename</param>
		/// <param name="name">Log name</param>
		public RolloverTraceListener(String fileName, String name): base(name)
		{
			mFileName = fileName;
		}

		private bool IsWebGarden()
		{
			// Get the section.
			ProcessModelSection processModelSection = ConfigurationManager.GetSection("system.web/processModel") as ProcessModelSection;

			if (processModelSection != null)
				return processModelSection.WebGarden;

			return false;
		}

		/// <summary>
		/// Closes the output stream so it no longer receives tracing or debugging output.
		/// </summary>
		public override void Close()
		{
			if (mWriter != null)
				mWriter.Close();

			mWriter = null;
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="T:System.Diagnostics.TraceListener"></see> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
				Close();
		}

		/// <summary>
		/// Writes trace information, a message, and event information to the listener specific output.
		/// </summary>
		/// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.</param>
		/// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
		/// <param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType"/> values specifying the type of event that has caused the trace.</param>
		/// <param name="id">A numeric identifier for the event.</param>
		/// <param name="message">A message to write.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode"/>
		///   </PermissionSet>
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
		{
			if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, message, null, null, null))
				WriteLine(message);
		}

		/// <summary>
		/// Writes trace information, a formatted array of objects and event information to the listener specific output.
		/// </summary>
		/// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.</param>
		/// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
		/// <param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType"/> values specifying the type of event that has caused the trace.</param>
		/// <param name="id">A numeric identifier for the event.</param>
		/// <param name="format">A format string that contains zero or more format items, which correspond to objects in the <paramref name="args"/> array.</param>
		/// <param name="args">An object array containing zero or more objects to format.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode"/>
		///   </PermissionSet>
		public override void TraceEvent(TraceEventCache eventCache, String source, TraceEventType eventType, int id, String format, params Object[] args)
		{
			if (Filter == null || Filter.ShouldTrace(eventCache, source, eventType, id, format, args, null, null))
				if (args != null)
					WriteLine(String.Format(format, args));
				else
					WriteLine(format);
		}

		/// <summary>
		/// Generates the rollover log filename
		/// </summary>
		/// <param name="fileName">basename of the filename to be generated</param>
		/// <returns>Generated filename</returns>
		private String GenerateFilename(String fileName)
		{
			// Initialize the new log file for today
			mCurrentDate = Config.Instance.LocalTime ? DateTime.Now.Date : DateTime.UtcNow.Date;

			String directory = Path.GetDirectoryName(fileName);
			String filePrefix = Path.GetFileNameWithoutExtension(fileName);
			String fileExtension = Path.GetExtension(fileName);

			try
			{
				// Try and create the log directory
				Directory.CreateDirectory(directory);

				// Remove older files
				foreach (String path in Directory.GetFiles(directory, String.Format("{0}_*{1}", filePrefix, fileExtension), SearchOption.TopDirectoryOnly))
				{
					String file = Path.GetFileNameWithoutExtension(path);
					String dateStamp = file.Substring(0, filePrefix.Length + 9);

					DateTime stamp;

					if (DateTime.TryParseExact(dateStamp, String.Format("'{0}_'yyyyMMdd", filePrefix), CultureInfo.InvariantCulture, DateTimeStyles.None, out stamp))
						if ((Config.Instance.LocalTime ? DateTime.Now : DateTime.UtcNow).Subtract(stamp).Days > mPruneAge)
							File.Delete(path);
				}
			}
			catch
			{
				// Log file pruning failed, not critical
			}

			// Return a filename in the form of "c:\temp\out1_20071101.log"
			if (IsWebGarden())
				// For Web Gardens, append the process id.
				return Path.Combine(directory, String.Format("{0}_{1}_{2}{3}", filePrefix, mCurrentDate.ToString("yyyyMMdd"), Process.GetCurrentProcess().Id, fileExtension));
			else
				return Path.Combine(directory, String.Format("{0}_{1}{2}", filePrefix, mCurrentDate.ToString("yyyyMMdd"), fileExtension));
		}

		/// <summary>
		/// Gets the encoding with fallback to default encoding.
		/// </summary>
		/// <param name="encoding">Encoding to use for output</param>
		/// <returns></returns>
		private static Encoding GetEncodingWithFallback(Encoding encoding)
		{
			Encoding encoding2 = (Encoding)encoding.Clone();
			encoding2.EncoderFallback = EncoderFallback.ReplacementFallback;
			encoding2.DecoderFallback = DecoderFallback.ReplacementFallback;
			return encoding2;
		}

		/// <summary>
		/// Ensures that the output writer is properly created and initialized,
		/// also rolls over into a new file if required.
		/// </summary>
		/// <returns>Whether the output writer is created successfully</returns>
		private bool EnsureWriter()
		{
			bool flag = true;

			// If the date has rolled over to the next day, release the write so a new 
			// writer will be initialized
			if (mCurrentDate.CompareTo(Config.Instance.LocalTime ? DateTime.Now.Date : DateTime.UtcNow.Date) != 0)
				Close();

			// Close the stream if the file is no longer accessible
			try
			{
				if (!String.IsNullOrEmpty(mCurrentFile))
					File.SetAttributes(mCurrentFile, FileAttributes.Archive);
			}
			catch
			{
				Close();
			}


			// Initialize a new writer if none exists or if the current log file has been deleted
			if (mWriter == null)
			{
				flag = false;

				if (mFileName == null)
					return flag;

				Encoding encodingWithFallback = GetEncodingWithFallback(new UTF8Encoding(false));
				mCurrentFile = GenerateFilename(mFileName);

				// Try creating the file
				// 1. Create file name as given
				// 2. Create filename with a GUID appended in front
				// 3. File could not be created
				for (int i = 0; i < 2; i++)
				{
					try
					{
						FileStream fsOutput = new FileStream(mCurrentFile, FileMode.Append, FileAccess.Write, FileShare.Delete | FileShare.ReadWrite);
						mWriter = new StreamWriter(fsOutput, encodingWithFallback, 0x1000);
						flag = true;
						break;
					}
					catch (IOException)
					{
						String directoryName = Path.GetDirectoryName(mCurrentFile);
						String fileName = Path.GetFileName(mCurrentFile);

						fileName = Guid.NewGuid().ToString() + fileName;
						mCurrentFile = Path.Combine(directoryName, fileName);
					}
					catch (Exception)
					{
						// Unrecoverable exception
						break;
					}
				}

				if (!flag)
				{
					mFileName = null;
				}
			}

			return flag;
		}

		/// <summary>
		/// Flushes the output buffer to the log file
		/// </summary>
		public override void Flush()
		{
			if (EnsureWriter())
				mWriter.Flush();
		}

		/// <summary>
		/// Writes the specified message to the listener
		/// </summary>
		/// <param name="message">Message to write</param>
		public override void Write(String message)
		{
			if (EnsureWriter())
			{
				if (NeedIndent)
					WriteIndent();

				mWriter.Write(message);
			}
		}

		/// <summary>
		/// Writes the specified message to the listener, followed by a line terminator.
		/// </summary>
		/// <param name="message">Message to write</param>
		public override void WriteLine(String message)
		{
			if (EnsureWriter())
			{
				if (base.NeedIndent)
					WriteIndent();

				mWriter.WriteLine(message);
				NeedIndent = true;
			}
		}

		// Properties
		/// <summary>
		/// Gets or sets the writer.
		/// </summary>
		/// <value>Textwriter currently used, or to be used for output</value>
		public TextWriter Writer
		{
			get
			{
				EnsureWriter();
				return mWriter;
			}
			set
			{
				mWriter = value;
			}
		}
	}
}
