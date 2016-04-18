using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace ProxyManager_cmd
{
	class ConfigOperator
	{
		private static string _savePath = Environment.CurrentDirectory;
		private static string _configFile;

		static ConfigOperator()
		{
			_configFile = Path.Combine(_savePath, "config.json");

			if (!File.Exists(_configFile))
			{
				using (var f = File.Create(_configFile))
				{
					f.Close();
				}
			}
		}

		public static void SaveConfig(List<ConfigData> configDataList)
		{
			using (var sw = new StreamWriter(_configFile, false))
			{
				sw.Write(JsonConvert.SerializeObject(configDataList, Formatting.Indented));
			}
		}

		public static List<ConfigData> LoadConfig()
		{
			using (var sr = new StreamReader(_configFile))
			{
				return JsonConvert.DeserializeObject<List<ConfigData>>(sr.ReadToEnd());
			}
		}
	}
}
