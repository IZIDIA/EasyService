using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyService.PcInfo {
	public class InstalledPrograms {
		public List<string> InstalledProgramsList { get; set; }
		public InstalledPrograms() {
			GetInstalledPrograms();
		}
		private void GetInstalledPrograms() {
			InstalledProgramsList = new List<string>();
			using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall")) {
				foreach (string subkey_name in key.GetSubKeyNames()) {
					using (RegistryKey subkey = key.OpenSubKey(subkey_name)) {
						var value = subkey.GetValue("DisplayName");
						if (value != null) {
							InstalledProgramsList.Add(string.Format("{0}", value));
						}
					}
				}
			}
		}
	}
}
