using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EasyService.PcInfo {
	public class OperatingSystemInfo {
		public string NameOc { get; set; }
		public string VersionOc { get; set; }
		public string SerialNumber { get; set; }
		public string SystemName { get; set; }
		public string UserName { get; set; }
		public string Architecture { get; set; }
		public bool UEFI { get; set; }
		public OperatingSystemInfo() {
			GetOSInfo();
			try {
				UEFI = IsWindowsUEFI();
			}
			catch {
				UEFI = false;
			}
		}
		public void GetOSInfo() {
			ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
			foreach (ManagementObject queryObj in searcher.Get()) {
				NameOc = string.Format("{0}", queryObj["Caption"]);
				VersionOc = string.Format("{0}", queryObj["Version"]);
				SerialNumber = string.Format("{0}", queryObj["SerialNumber"]);
				SystemName = string.Format("{0}", queryObj["CSName"]);
				UserName = string.Format("{0}", queryObj["RegisteredUser"]);
				Architecture = string.Format("{0}", queryObj["OSArchitecture"]);
			}
		}
		[DllImport("kernel32.dll",
		EntryPoint = "GetFirmwareEnvironmentVariableW",
		SetLastError = true,
		CharSet = CharSet.Unicode,
		ExactSpelling = true,
		CallingConvention = CallingConvention.StdCall)]
		public static extern int GetFirmwareType(string lpName, string lpGUID, IntPtr pBuffer, uint size);
		public const int ERROR_INVALID_FUNCTION = 1;
		public static bool IsWindowsUEFI() {
			GetFirmwareType("", "{00000000-0000-0000-0000-000000000000}", IntPtr.Zero, 0);
			if (Marshal.GetLastWin32Error() == ERROR_INVALID_FUNCTION) {
				return false;
			}
			else {
				return true;
			}
		}
	}
}
