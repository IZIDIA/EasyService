using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyService.PcInfo.Devices {
	public class RAMObject {
		public string MemoryName { get; set; }
		public string MemorySize { get; set; }
		public string MemorySpeed { get; set; }
		public RAMObject(string MemoryName, string MemorySize, string MemorySpeed) {
			this.MemoryName = MemoryName;
			this.MemorySize = MemorySize;
			this.MemorySpeed = MemorySpeed;
		}
	}
}
