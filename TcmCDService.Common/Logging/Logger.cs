#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Logger
// ---------------------------------------------------------------------------------
//	Date Created	: February 28, 2013
//	Author			: Rob van Oostenrijk
//
////////////////////////////////////////////////////////////////////////////////////
#endregion
using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace TcmCDService.Logging
{
	/// <summary>
	/// <para>Logger class with static methods to log information, warning and error messages</para>
	/// </summary>
	/// <remarks>
	/// <para>The logger uses the framework Trace object to write messages.</para>
	/// <para>Trace Listeners and Switches can be configured in the application
	/// .config file to channel the messages to the required destination (file, event log, email etc.).</para>
	/// Trace levels:
	/// <list type="bullet">
	///     <listheader>Trace Switch levels:</listheader>
	///     <item>
	///         <term>Off</term>
	///         <description> - no logging</description>
	///     </item>
	///     <item>
	///         <term>Error</term>
	///         <description> - errors only</description>
	///     </item>
	///     <item>
	///         <term>Warning</term>
	///         <description> - and warnings</description>
	///     </item>
	///     <item>
	///         <term>Information</term>
	///         <description> - and information</description>
	///     </item>
	///     <item>
	///         <term>Verbose</term>
	///         <description> - and debugging information</description>
	///     </item>
	/// </list>
	/// <para>Configuration is as follows:</para>
	/// <code>
	/// &lt;system.diagnostics&gt;
	///     &lt;sources&gt;
	///     &lt;!-- 
	///     Control log level by setting the switchValue to:
	///      - Verbose
	///      - Information
	///      - Warning
	///      - Error
	///     --&gt;
	///         &lt;source name="TcmCDService" switchValue="Verbose"&gt;
	///             &lt;listeners&gt;
	///              &lt;remove name="Default" /&gt;
	///                 &lt;add name="FileLogListener" type="TcmCDService.Logging.RolloverTraceListener, TcmCDService" initializeData="c:\data\logs\TcmCDService.log" /&gt;
	///             &lt;/listeners&gt;
	///         &lt;/source&gt;
	///     &lt;/sources&gt;
	///     &lt;trace autoflush="true" indentsize="2" /&gt;
	/// &lt;/system.diagnostics&gt;
	/// </code>
	/// </remarks>
	public sealed class Logger
	{
		/// <summary>
		/// Private constructor to prevent compilers generating default constructor
		/// </summary>
		private Logger()
		{
		}

		private static TraceSource mSource;
		private static Regex mRegEx;

		private static TraceSource Source
		{
			get
			{
				if (mSource == null)
					mSource = new TraceSource("TcmCDService");

				return mSource;
			}
		}

		private static Regex Formatter
		{
			get
			{
				if (mRegEx == null)
					mRegEx = new Regex("\r\n|\r|\n", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

				return mRegEx;
			}
		}

		private static void TraceWrite(String message, TraceEventType level)
		{
			Regex test = new Regex("", RegexOptions.None);

			// If we are handling a multi-line message, indent and format it correctly
			message = Formatter.Replace(message, "\r\n             ...      ");

			String sLevel = "Unknown";

			switch (level)
			{
				case TraceEventType.Error:
					sLevel = "Error";
					break;
				case TraceEventType.Information:
					sLevel = "Info";
					break;
				case TraceEventType.Verbose:
					sLevel = "Verbose";
					break;
				case TraceEventType.Warning:
					sLevel = "Warning";
					break;
				case TraceEventType.Critical:
					sLevel = "Critical";
					break;
			}

			Source.TraceEvent(level, 0, "{0,-13:HH:mm:ss.ff}{1,-9}{2}", DateTime.UtcNow, sLevel, message);
		}

		/// <summary>
		/// Output a log trace for the given <see cref="T:System.Exception" />
		/// </summary>
		/// <param name="ex"><see cref="T:System.Exception" /></param>
		/// <returns><see cref="T:System.Exception" /> logtrace</returns>
		public static String TraceException(Exception ex)
		{
			StringBuilder sbMessage = new StringBuilder();
			int depth = 1;

			if (ex != null)
			{
				if (!String.IsNullOrEmpty(ex.Source))
					sbMessage.AppendFormat("{0} ({1})\n", ex.GetType().FullName, ex.Source);

				if (!String.IsNullOrEmpty(ex.Message))
					sbMessage.AppendLine(ex.Message);

				if (!String.IsNullOrEmpty(ex.StackTrace))
					sbMessage.AppendLine(ex.StackTrace);

				while (ex.InnerException != null)
				{
					String indent = new String('\t', depth);

					ex = ex.InnerException;

					if (!String.IsNullOrEmpty(ex.Source))
					{
						sbMessage.Append(indent);
						sbMessage.AppendFormat("{0} ({1})\n", ex.GetType().FullName, ex.Source);
					}

					if (!String.IsNullOrEmpty(ex.Message))
					{
						sbMessage.Append(indent);
						sbMessage.AppendLine(ex.Message);
					}

					if (!String.IsNullOrEmpty(ex.StackTrace))
					{
						sbMessage.Append(indent);
						sbMessage.AppendLine(Formatter.Replace(ex.StackTrace, "\r" + indent));
					}

					depth++;
				}
			}

			return sbMessage.ToString();
		}

		/// <summary>
		/// Log an error message
		/// </summary>
		/// <param name="message">The message to log</param>
		public static void Error(String message)
		{
			TraceWrite(message, TraceEventType.Error);
		}

		/// <summary>
		/// Log an error message
		/// </summary>
		/// <param name="message">The message to log</param>
		/// <param name="ex">Associated exception to output</param>
		public static void Error(String message, Exception ex)
		{
			Error(String.Format("{0}\n{1}", message, TraceException(ex)));
		}

		/// <summary>
		/// Log an error message
		/// </summary>
		/// <param name="format">Message format string</param>
		/// <param name="args">Format string parameters</param>
		public static void Error(String format, params Object[] args)
		{
			Error(String.Format(format, args));
		}


		/// <summary>
		/// Log an error message
		/// </summary>
		/// <param name="format">Message format string</param>
		/// <param name="ex">Associated exception to output</param>
		/// <param name="args">Format string parameters</param>
		public static void Error(String format, Exception ex, params Object[] args)
		{
			Error(String.Format("{0}\n{1}", String.Format(format, args), TraceException(ex)));
		}

		/// <summary>
		/// Log a warning message
		/// </summary>
		/// <param name="message">The message to log</param>
		public static void Warning(String message)
		{
			TraceWrite(message, TraceEventType.Warning);
		}

		/// <summary>
		/// Log a warning message
		/// </summary>
		/// <param name="message">The message to log</param>
		/// <param name="ex">Associated exception to output</param>
		public static void Warning(String message, Exception ex)
		{
			Warning(String.Format("{0}\n{1}", message, TraceException(ex)));
		}

		/// <summary>
		/// Log a warning message
		/// </summary>
		/// <param name="format">Message / Message format string</param>
		/// <param name="args">Format string parameters</param>
		public static void Warning(String format, params Object[] args)
		{
			Warning(String.Format(format, args));
		}

		/// <summary>
		/// Log an warning message
		/// </summary>
		/// <param name="format">Message format string</param>
		/// <param name="ex">Associated exception to output</param>
		/// <param name="args">Format string parameters</param>
		public static void Warning(String format, Exception ex, params Object[] args)
		{
			Warning(String.Format("{0}\n{1}", String.Format(format, args), TraceException(ex)));
		}

		/// <summary>
		/// Log an information message
		/// </summary>
		/// <param name="message">The message to log</param>
		public static void Info(String message)
		{
			TraceWrite(message, TraceEventType.Information);
		}

		/// <summary>
		/// Log an information message
		/// </summary>
		/// <param name="message">The message to log</param>
		/// <param name="ex">Associated exception to output</param>
		public static void Info(String message, Exception ex)
		{
			Info(String.Format("{0}\n{1}", message, TraceException(ex)));
		}

		/// <summary>
		/// Log an information message
		/// </summary>
		/// <param name="format">Message / Message format string</param>
		/// <param name="args">Format string parameters</param>
		public static void Info(String format, params Object[] args)
		{
			Info(String.Format(format, args));
		}

		/// <summary>
		/// Log an information message
		/// </summary>
		/// <param name="format">Message format string</param>
		/// <param name="ex">Associated exception to output</param>
		/// <param name="args">Format string parameters</param>
		public static void Info(String format, Exception ex, params Object[] args)
		{
			Info(String.Format("{0}\n{1}", String.Format(format, args), TraceException(ex)));
		}

		/// <summary>
		/// Log a debug message
		/// </summary>
		/// <param name="message">The message to log</param>
		public static void Debug(String message)
		{
			TraceWrite(message, TraceEventType.Verbose);
		}

		/// <summary>
		/// Log a debug message
		/// </summary>
		/// <param name="message">The message to log</param>
		/// <param name="ex">Associated exception to output</param>
		public static void Debug(String message, Exception ex)
		{
			Debug(String.Format("{0}\n{1}", message, TraceException(ex)));
		}

		/// <summary>
		/// Log a debug message
		/// </summary>
		/// <param name="format">Message format string</param>
		/// <param name="args">Format string parameters</param>
		public static void Debug(String format, params Object[] args)
		{
			Debug(String.Format(format, args));
		}

		/// <summary>
		/// Log a debug message
		/// </summary>
		/// <param name="format">Message format string</param>
		/// <param name="ex">Associated exception to output</param>
		/// <param name="args">Format string parameters</param>
		public static void Debug(String format, Exception ex, params Object[] args)
		{
			Debug(String.Format("{0}\n{1}", String.Format(format, args), TraceException(ex)));
		}
	}
}
