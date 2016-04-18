using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO;
using System.Diagnostics;

namespace ProxyManager_cmd
{
	class Program
	{
		static void Excute(string fileName, string args)
		{
			if (!File.Exists(fileName))
			{
				throw new FileNotFoundException();
			}

			var psi = new ProcessStartInfo();

			psi.FileName = fileName;
			psi.Arguments = args;

			try
			{
				var p = Process.Start(psi);
				p.WaitForExit();
			}
			catch
			{
				throw;
			}
		}

		static void Main(string[] args)
		{
			var configList = new List<ConfigData>();
			var deviceList = new List<ManagementObject>();

			var dOperator = new DeviceOperator();

			configList = ConfigOperator.LoadConfig();

			if (configList == null)
			{
				configList = new List<ConfigData>();
			}

			deviceList = dOperator.AdapterList;

			Console.Write("Save of Load?(s/l)=> ");
			var s = Console.ReadLine().ToUpper();
			Console.WriteLine();

			switch (s)
			{
				case "L":
					for (int i = 0; i < configList.Count; i++)
					{
						var item = configList[i];
						Console.WriteLine(i + ":" + item.Name);
						Console.WriteLine("    Device:       " + item.Device);
						Console.WriteLine("    Proxy:        " + item.Proxy);
						Console.WriteLine("    Port:         " + item.Port);
						Console.WriteLine("    AuthName:     " + item.AuthName);
						Console.WriteLine("    AuthPassword: " + item.AuthPassword);
						Console.WriteLine();
					}
					Console.WriteLine();
					for (int i = 0; i < deviceList.Count; i++)
					{
						if ((bool)deviceList[i].Properties["NetEnabled"].Value)
						{
							var item = configList.Where(x => x.Device == deviceList[i].Properties["Name"].Value.ToString()).First();
							Console.WriteLine("Active adapter: " + item.Name);
							Console.WriteLine();
						}
					}
					Console.Write("Select Asset =>");
					int index;
					s = Console.ReadLine();
					if (!int.TryParse(s, out index))
					{
						Console.WriteLine("Error Quit EXE");
						return;
					}

					if (index < configList.Count)
					{
						var selected = configList[index];
						ProxyOperator.SetProxy(selected);
						Excute(Path.Combine(Environment.CurrentDirectory, "DeviceManager.exe"), string.Concat("\"", selected.Device, "\""));
					}
					else
					{
						Console.WriteLine("Error Quit EXE");
						return;
					}

					break;
				case "S":
					var data = new ConfigData();
					Console.Write("Name: ");
					data.Name = Console.ReadLine();
					Console.WriteLine("Select Adapter: ");
					for (int i = 0; i < deviceList.Count; i++)
					{
						var item = deviceList[i];
						Console.WriteLine(i + ":" + item.Properties["NetConnectionID"].Value.ToString());
					}
					Console.Write("=> ");
					s = Console.ReadLine();
					if (!int.TryParse(s, out index))
					{
						Console.WriteLine("Error Quit EXE");
						return;
					}

					if (index < deviceList.Count)
					{
						data.Device = deviceList[index].Properties["Name"].Value.ToString();
					}
					else
					{
						Console.WriteLine("Error Quit EXE");
						return;
					}

					Console.Write("Proxy: ");
					data.Proxy = Console.ReadLine();
					Console.Write("Port: ");
					data.Port = Console.ReadLine();
					Console.Write("AuthName: ");
					data.AuthName = Console.ReadLine();
					Console.Write("AuthPassword: ");
					data.AuthPassword = Console.ReadLine();

					Console.Write("Save?(y/n)=> ");
					s = Console.ReadLine().ToUpper();

					switch (s)
					{
						case "Y":
							configList.Add(data);
							ConfigOperator.SaveConfig(configList);
							break;
						case "N":
							Console.WriteLine("Quit EXE");
							return;
						default:
							Console.WriteLine("Error Quit EXE");
							return;
					}

					break;
				default:
					Console.WriteLine("Error Quit EXE");
					return;
			}
		}
	}
}
