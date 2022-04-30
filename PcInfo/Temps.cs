using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyService.PcInfo {
	public class Temps {
		public List<KeyValuePair<string, string>> CPUTemp { get; set; }
		public List<KeyValuePair<string, string>> GPUTemp { get; set; }
		public Temps() {
			GetCpuTemps();
			GetGpuTemps();
		}
		private void GetCpuTemps() {
			CPUTemp = new List<KeyValuePair<string, string>>();
			Computer computer = new Computer() { CPUEnabled = true };
			computer.Open();
			foreach (IHardware hardware in computer.Hardware) {
				hardware.Update();
				foreach (ISensor sensor in hardware.Sensors) {
					if (sensor.SensorType == SensorType.Temperature && sensor.Name == "CPU Package") {
						CPUTemp.Add(new KeyValuePair<string, string>(string.Format("{0}", sensor.Hardware.Name), string.Format("{0}", sensor.Value)));
					}
				}
			}
		}
		private void GetGpuTemps() {
			GPUTemp = new List<KeyValuePair<string, string>>();
			Computer computer = new Computer() { GPUEnabled = true };
			computer.Open();
			foreach (IHardware hardware in computer.Hardware) {
				hardware.Update();
				foreach (ISensor sensor in hardware.Sensors) {
					if (sensor.SensorType == SensorType.Temperature) {
						GPUTemp.Add(new KeyValuePair<string, string>(string.Format("{0}", sensor.Hardware.Name), string.Format("{0}", sensor.Value)));
					}
				}
			}
		}
	}
}
