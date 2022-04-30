using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyService.PcInfo.Devices {
	public class PrinterObject {
		public string Name { get; set; }
		public string Status { get; set; }
		public bool Default { get; set; }
		public bool Network { get; set; }
		public PrinterObject(string Name, string Status, bool Default, bool Network) {
			this.Name = Name;
			this.Status = Status;
			this.Default = Default;
			this.Network = Network;
		}
	}
}
