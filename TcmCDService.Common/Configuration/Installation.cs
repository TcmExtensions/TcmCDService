using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Text;

namespace TcmCDService.Configuration
{
	/// <summary>
	/// <see cref="Installer" /> provides functionality to install assemblies such as Windows Services
	/// </summary>
	public static class Installer
	{
		/// <summary>
		/// Installs the assembly specified by <see cref="P:assemblyPath" />
		/// </summary>
		/// <param name="assemblyPath">Full path to the assembly to install</param>
		public static void Install(String assemblyPath)
		{
			try
			{
				Console.WriteLine("Installing: {0}", assemblyPath);

				AssemblyInstaller installer = new AssemblyInstaller(assemblyPath, new String[] { })
				{
					UseNewContext = true
				};

				installer.Install(null);
				installer.Commit(null);
			}
			catch (FileNotFoundException exception)
			{
				Console.WriteLine("[!] Installer state file not found, installing service without storing the state" + exception.Message);
			}
			catch (Exception exception2)
			{
				Console.WriteLine("[!] Unable to install service: " + exception2.Message);
			}
		}

		/// <summary>
		/// Uninstalls the assembly specified by <see cref="P:assemblyPath" />
		/// </summary>
		/// <param name="assemblyPath">Full path to the assembly to uninstall</param>
		public static void Uninstall(string assemblyPath)
		{
			try
			{
				AssemblyInstaller installer = new AssemblyInstaller(assemblyPath, new String[] { })
				{
					UseNewContext = true
				};

				installer.Uninstall(null);
				installer.Commit(null);
			}
			catch (FileNotFoundException exception)
			{
				Console.WriteLine("[!] Installer state file not found, uninstalling service without storing the state" + exception.Message);
			}
			catch (Exception exception2)
			{
				Console.WriteLine("[!] Unable to uninstall service: " + exception2.Message);
			}
		}
	}
}
