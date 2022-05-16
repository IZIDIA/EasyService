using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace EasyService.PcInfo {
	public class AutoloadPrograms {
		public List<string> AutoloadProgramsList { get; set; }
		public AutoloadPrograms() {
			GetAutoloadPrograms();
		}
		private void GetAutoloadPrograms() {
			AutoloadProgramsList = new List<string>();
			ManagementObjectSearcher searcher = new ManagementObjectSearcher();
			searcher.Query = new ObjectQuery("SELECT * From Win32_StartupCommand");
			foreach (ManagementObject mObject in searcher.Get()) {
					AutoloadProgramsList.Add(mObject["Name"].ToString());
			}
			AutoloadProgramsList.Sort();
		}
	}
}
