using EasyService.PcInfo.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace EasyService.PcInfo {
	public class Disks {
		public List<DiskObject> Disk { get; set; }
		public Disks() {
			GetDisksInfo();
		}
		public void GetDisksInfo() {
			Disk = new List<DiskObject>();
			var driveQuery = new ManagementObjectSearcher("select * from Win32_DiskDrive");
			foreach (ManagementObject d in driveQuery.Get()) {
				var deviceId = d.Properties["DeviceId"].Value;
				var partitionQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_DiskDriveToDiskPartition", d.Path.RelativePath);
				var partitionQuery = new ManagementObjectSearcher(partitionQueryText);
				foreach (ManagementObject p in partitionQuery.Get()) {
					var logicalDriveQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_LogicalDiskToPartition", p.Path.RelativePath);
					var logicalDriveQuery = new ManagementObjectSearcher(logicalDriveQueryText);
					foreach (ManagementObject ld in logicalDriveQuery.Get()) {
						Disk.Add(new DiskObject(
							string.Format("{0}", d.Properties["Model"].Value),
							string.Format("{0}", ld.Properties["Name"].Value),
							string.Format("{0}", ld.Properties["VolumeName"].Value),
							string.Format("{0}", Math.Round(Convert.ToDouble(ld.Properties["FreeSpace"].Value) / 1073741824, 1)),
							string.Format("{0}", Math.Round(Convert.ToDouble(ld.Properties["Size"].Value) / 1073741824, 1)),
							string.Format("{0}", ld.Properties["FileSystem"].Value),
							string.Format("{0}", d.Properties["Status"].Value)
						));
					}
				}
			}
		}
	}
}
