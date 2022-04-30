using EasyService.PcInfo.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace EasyService.PcInfo {
	public class Peripherals {
		public List<PrinterObject> Printers { get; set; }
		public Peripherals() {
			GetPrinters();
		}
		public void GetPrinters() {
			Printers = new List<PrinterObject>();
			var printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");
			foreach (var printer in printerQuery.Get()) {
				Printers.Add(new PrinterObject(
					string.Format("{0}", printer.GetPropertyValue("Name")),
					string.Format("{0}", printer.GetPropertyValue("Status")),
					(bool)printer.GetPropertyValue("Default"),
					(bool)printer.GetPropertyValue("Network")
				));
			}
		}
	}
}
