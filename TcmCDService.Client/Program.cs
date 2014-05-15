#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Program
// ---------------------------------------------------------------------------------
//	Date Created	: March 30, 2014
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
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TcmCDService.Contracts;
using TcmCDService.Remoting.Web.Linking;
using System.Runtime.Serialization;
using System.Xml;

namespace TcmCDService.Client
{
	class Program
	{
		private static String ToJSON(Object o)
		{
			if (o != null)
			{
				DataContractJsonSerializer serializer = new DataContractJsonSerializer(o.GetType());

				using (MemoryStream ms = new MemoryStream())
				{
					serializer.WriteObject(ms, o);
					return Encoding.UTF8.GetString(ms.ToArray());
				}
			}
			else
				return "(null)";
		}

		private static String ToXml(Object o)
		{
			if (o != null)
			{
				DataContractSerializer serializer = new DataContractSerializer(o.GetType());

				using (StringWriter stringWriter = new StringWriter())
				{
					using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings()
					{
						Indent = true
					}))
					{
						serializer.WriteObject(xmlWriter, o);
					}

					return stringWriter.ToString();
				}
			}
			else
				return "(null)";
		}

		private static void TestTask(bool output, bool loop)
		{
			int start = Environment.TickCount;
			int cycle = start;

			for (int i = 0; i < 10000; i++)
			{
				if (i % 100 == 0)
				{
					if (i > 0 && output)
						Console.WriteLine("Execution cycle #{0}\tCycle Time: {1} secs.\tTotal Time: {2} secs.", i, (Environment.TickCount - cycle) / 1000, (Environment.TickCount - start) / 1000);

					cycle = Environment.TickCount;
				}

				String componentlink = ComponentLink.Get("tcm:233-193779");

				if (output)
					Console.WriteLine("ComponentLink #1: {0}", componentlink);

				componentlink = ComponentLink.Get("tcm:233-11111-64", "tcm:233-193779", "tcm:233-1222111-32", true);

				if (output)
					Console.WriteLine("ComponentLink #2: {0}", componentlink);

				String binaryLink = BinaryLink.Get("tcm:233-703376", null, "anchor");

				if (output)
					Console.WriteLine("BinaryLink: {0}", binaryLink);

				String pageLink = PageLink.Get("tcm:233-192225-64");

				if (output)
					Console.WriteLine("PageLink #1: {0}", pageLink);

				pageLink = PageLink.Get("tcm:233-192225-64", "anchor", "parameters");

				if (output)
					Console.WriteLine("PageLink #2: {0}", pageLink);

				ComponentPresentation componentPresentation = TcmCDService.Remoting.DynamicContent.ComponentPresentation.GetHighestPriority("tcm:233-685281");

				if (output)
				{
					Console.WriteLine("Presentation #1: {0}", componentPresentation != null);
					Console.WriteLine("JSON:\n{0}", ToJSON(componentPresentation));
				}

				componentPresentation = TcmCDService.Remoting.DynamicContent.ComponentPresentation.GetHighestPriority(233, 685281);

				if (output)
					Console.WriteLine("Presentation #2: {0}", componentPresentation != null);

				componentPresentation = TcmCDService.Remoting.DynamicContent.ComponentPresentation.Get("tcm:233-685281", "tcm:233-355892-32");

				if (output)
					Console.WriteLine("Presentation #3: {0}", componentPresentation != null);

				componentPresentation = TcmCDService.Remoting.DynamicContent.ComponentPresentation.Get("tcm:233-685281", 355892);

				if (output)
					Console.WriteLine("Presentation #4: {0}", componentPresentation != null);

				componentPresentation = TcmCDService.Remoting.DynamicContent.ComponentPresentation.Get(233, 685281, 355892);

				if (output)
					Console.WriteLine("Presentation #5: {0}", componentPresentation != null);

				ComponentMeta componentMeta = TcmCDService.Remoting.Meta.ComponentMeta.Get("tcm:233-685281");

				if (output)
				{
					Console.WriteLine("ComponentMeta #1: {0}", componentMeta != null);
					Console.WriteLine("JSON:\n{0}", ToJSON(componentMeta));
				}

				componentMeta = TcmCDService.Remoting.Meta.ComponentMeta.Get(233, 685281);

				if (output)
					Console.WriteLine("ComponentMeta #2: {0}", componentMeta != null);

				Byte[] binary = TcmCDService.Remoting.DynamicContent.BinaryData.Get("tcm:233-684746");

				if (output)
					Console.WriteLine("Binary #1: {0}", binary.Length);

				String pageContent = TcmCDService.Remoting.DynamicContent.PageContent.Get("tcm:233-192225-64");

				if (output)
					Console.WriteLine("PageContent #1: {0}", pageContent.Length);

				IEnumerable<String> taxonomies = TcmCDService.Remoting.Taxonomies.Keyword.GetTaxonomies("tcm:0-233-1");

				if (output)
					Console.WriteLine("Taxonomies: {0}", String.Join(", ", taxonomies));

				Keyword keyword = TcmCDService.Remoting.Taxonomies.Keyword.GetKeywords("tcm:233-42180-512");

				if (output)
					Console.WriteLine("Keyword: {0}", ToXml(keyword));
				/*
				keyword = TcmCDService.Remoting.Taxonomies.Keyword.GetKeywords("tcm:233-42180-512", new TaxonomyFilter()
				{
					DepthFilteringLevel = 10,
					DepthFilteringDirection = TaxonomyFilterDirecton.Down					 
				});
				*/

				if (!loop)
					break;
			}
		}

		public static IEnumerable<Task> StartTasks(int numTasks)
		{
			for (int i = 0; i < numTasks; i++)
			{
				yield return Task.Factory.StartNew(() => TestTask(false, true), TaskCreationOptions.LongRunning);
			}
		}

		static void Main(string[] args)	
		{
			Console.WriteLine("[i] TcmCDService.Client");

			int start = Environment.TickCount;

			try
			{
				// Normal output task
				TestTask(true, false);

				// Execute multi-threaded load tasks
				//Task.WaitAll(StartTasks(8).ToArray());
			}
			catch (Exception ex)
			{
				Console.WriteLine("{0}\n{1}", ex.Message, ex.StackTrace);
			}
			
			Console.WriteLine("Total Execution time {0} secs.", (Environment.TickCount - start) / 1000);

			Console.ReadKey();
		}
	}
}
