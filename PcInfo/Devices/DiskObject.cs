using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyService.PcInfo.Devices {
	public class DiskObject {
		public string DiskModel { get; set; }
		public string DriveName { get; set; }
		public string VolumeName { get; set; }
		public string FreeSpace { get; set; }
		public string TotalSpace { get; set; }
		public string FileSystem { get; set; }
		public string MediaStatus { get; set; }
		public DiskObject(
			string DiskModel,
			string DriveName,
			string VolumeName,
			string FreeSpace,
			string TotalSpace,
			string FileSystem,
			string MediaStatus
		) {
			this.DiskModel = DiskModel;
			this.DriveName = DriveName;
			this.VolumeName = VolumeName;
			this.FreeSpace = FreeSpace;
			this.TotalSpace = TotalSpace;
			this.FileSystem = FileSystem;
			this.MediaStatus = MediaStatus;
		}
	}
}
