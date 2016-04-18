using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace DeviceManager
{
	class Program
	{
		static void Main(string[] args)
		{
			List<ManagementObject> adapterList = new List<ManagementObject>();

			try
			{
				ObjectQuery oq = new ObjectQuery("select * from Win32_NetworkAdapter Where NetConnectionID IS NOT NULL");
				ManagementObjectSearcher mos = new ManagementObjectSearcher(oq);
				foreach (ManagementObject mo in mos.Get())
				{
					adapterList.Add(mo);
				}
			}
			catch (Exception)
			{
				throw;
			}

			if (adapterList == null) return;

			foreach (var mo in adapterList)
			{
				if (string.Equals(args[0], mo.Properties["Name"].Value.ToString()))
				{
					mo.InvokeMethod("Enable", null);
				}
				else
				{
					mo.InvokeMethod("Disable", null);
				}
			}
		}
	}
}
