using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyService.PcInfo.Devices {
	public class GPUObject {
		public string GPUName { get; set; }
		public string GPUDriverVersion { get; set; }
		public GPUObject(string GPUName, string GPUDriverVersion) {
			this.GPUName = GPUName;
			this.GPUDriverVersion = GPUDriverVersion;
		}
	}
}
