using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace ProxyManager_cmd
{
	public static class ProxyOperator
	{
		public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
		public const int INTERNET_OPTION_REFRESH = 37;

		[DllImport("wininet.dll")]
		public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufgferLength);

		static ProxyOperator() { }

		public static void SetProxy(ConfigData data)
		{
			RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);

			if (!string.IsNullOrWhiteSpace(data.Proxy))
			{
				regKey.SetValue("ProxyServer", string.Concat(data.Proxy, ":", data.Port));
				regKey.SetValue("ProxyEnable", 1);
			}
			else
			{
				regKey.SetValue("ProxyEnable", 0);
			}

			InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
			InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
		}
	}
}
