using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyService.PcInfo {
	public class Performance {
		public List<KeyValuePair<string, string>> CPULoad { get; set; }
		public List<KeyValuePair<string, string>> GPULoad { get; set; }
		public List<KeyValuePair<string, string>> RAMLoad { get; set; }
		public Performance() {
			GetCpuLoad();
			GetGpuLoad();
			GetRAMLoad();
		}
		private void GetCpuLoad() {
			CPULoad = new List<KeyValuePair<string, string>>();
			Computer computer = new Computer() { CPUEnabled = true };
			computer.Open();
			foreach (IHardware hardware in computer.Hardware) {
				hardware.Update();
				foreach (ISensor sensor in hardware.Sensors) {
					if (sensor.SensorType == SensorType.Load && sensor.Name == "CPU Total") {
						CPULoad.Add(new KeyValuePair<string, string>(string.Format("{0}", sensor.Hardware.Name), string.Format("{0}", Math.Round((double)sensor.Value, 1))));
					}
				}
			}
		}
		private void GetGpuLoad() {
			GPULoad = new List<KeyValuePair<string, string>>();
			Computer computer = new Computer() { GPUEnabled = true };
			computer.Open();
			foreach (IHardware hardware in computer.Hardware) {
				hardware.Update();
				foreach (ISensor sensor in hardware.Sensors) {
					if (sensor.SensorType == SensorType.Load && sensor.Name == "GPU Core") {
						GPULoad.Add(new KeyValuePair<string, string>(string.Format("{0}", sensor.Hardware.Name), string.Format("{0}", Math.Round((double)sensor.Value, 1))));
					}
				}
			}
		}
		private void GetRAMLoad() {
			RAMLoad = new List<KeyValuePair<string, string>>();
			Computer computer = new Computer() { RAMEnabled = true };
			computer.Open();
			foreach (IHardware hardware in computer.Hardware) {
				hardware.Update();
				foreach (ISensor sensor in hardware.Sensors) {
					if (sensor.SensorType == SensorType.Load && sensor.Name == "Memory") {
						RAMLoad.Add(new KeyValuePair<string, string>(string.Format("{0}", sensor.Hardware.Name), string.Format("{0}", Math.Round((double)sensor.Value, 1))));
					}
				}
			}
		}
	}
}
