using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyService.PcInfo {
	public class InstalledPrograms {
		const string Registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
		public List<string> InstalledProgramsList { get; set; }
		public InstalledPrograms() {
			InstalledProgramsList = GetInstalledPrograms();
		}
		public static List<string> GetInstalledPrograms() {
			var result = new List<string>();
			result.AddRange(GetInstalledProgramsFromRegistry(RegistryView.Registry32));
			result.AddRange(GetInstalledProgramsFromRegistry(RegistryView.Registry64));
			result.Sort();
			return result;
		}
		private static IEnumerable<string> GetInstalledProgramsFromRegistry(RegistryView registryView) {
			var result = new List<string>();
			using (RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView).OpenSubKey(Registry_key)) {
				foreach (string subkey_name in key.GetSubKeyNames()) {
					using (RegistryKey subkey = key.OpenSubKey(subkey_name)) {
						if (IsProgramVisible(subkey)) {
							result.Add((string)subkey.GetValue("DisplayName"));
						}
					}
				}
			}
			return result;
		}
		private static bool IsProgramVisible(RegistryKey subkey) {
			var name = (string)subkey.GetValue("DisplayName");
			var releaseType = (string)subkey.GetValue("ReleaseType");
			var systemComponent = subkey.GetValue("SystemComponent");
			var parentName = (string)subkey.GetValue("ParentDisplayName");
			return
					!string.IsNullOrEmpty(name)
					&& string.IsNullOrEmpty(releaseType)
					&& string.IsNullOrEmpty(parentName)
					&& (systemComponent == null);
		}
	}
}
