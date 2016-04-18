using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace ProxyManager_cmd
{
	class DeviceOperator
	{
		private List<ManagementObject> _adapterList = new List<ManagementObject>();

		public List<ManagementObject> AdapterList { get { return _adapterList; } }

		public DeviceOperator()
		{
			try
			{
				ObjectQuery oq = new ObjectQuery("select * from Win32_NetworkAdapter Where NetConnectionID IS NOT NULL");
				ManagementObjectSearcher mos = new ManagementObjectSearcher(oq);
				foreach (ManagementObject mo in mos.Get())
				{
					_adapterList.Add(mo);
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		public void AdapterSet(ConfigData data)
		{
			if (AdapterList == null) return;

			foreach (var mo in AdapterList)
			{
				if (data.Device == mo.Properties["Name"].Value.ToString())
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
