using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyService.PcInfo.Devices {
	public class MotherboardObject {
		public string MotherboardCompanyName { get; set; }
		public string MotherboardModel { get; set; }
		public MotherboardObject(string MotherboardCompanyName, string MotherboardModel) {
			this.MotherboardCompanyName = MotherboardCompanyName;
			this.MotherboardModel = MotherboardModel;
		}
	}
}
