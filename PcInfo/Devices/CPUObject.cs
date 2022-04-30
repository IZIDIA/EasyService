using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyService.PcInfo.Devices {
	public class CPUObject {
		public string CPUName { get; set; }
		public string CPUCores { get; set; }
		public string CPULogicalCores { get; set; }
		public CPUObject(string CPUName, string CPUCores, string CPULogicalCores) {
			this.CPUName = CPUName;
			this.CPUCores = CPUCores;
			this.CPULogicalCores = CPULogicalCores;
		}
	}
}
