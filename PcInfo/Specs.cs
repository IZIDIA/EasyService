using EasyService.PcInfo.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace EasyService.PcInfo {
	public class Specs {
		public List<CPUObject> CPU { get; set; }
		public List<GPUObject> GPU { get; set; }
		public List<RAMObject> RAM { get; set; }
		public List<MotherboardObject> Motherboard { get; set; }
		public Specs() {
			GetProcessorInfo();
			GetVideoControllerInfo();
			GetPhysicalMemoryInfo();
			GetMotherboardInfo();
		}
		public void GetProcessorInfo() {
			CPU = new List<CPUObject>();
			ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
			foreach (ManagementObject queryObj in searcher.Get()) {
				CPU.Add(new CPUObject(
					string.Format("{0}", queryObj["Name"]),
					string.Format("{0}", queryObj["NumberOfCores"]),
					string.Format("{0}", queryObj["NumberOfLogicalProcessors"])
				));
			}
		}
		public void GetVideoControllerInfo() {
			GPU = new List<GPUObject>();
			ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");
			foreach (ManagementObject queryObj in searcher.Get()) {
				GPU.Add(new GPUObject(
					string.Format("{0}", queryObj["Caption"]),
					string.Format("{0}", queryObj["DriverVersion"])
				));
			}
		}
		public void GetPhysicalMemoryInfo() {
			RAM = new List<RAMObject>();
			ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PhysicalMemory");
			foreach (ManagementObject queryObj in searcher.Get()) {
				RAM.Add(new RAMObject(
					string.Format("{0}", queryObj["PartNumber"]),
					string.Format("{0}", Math.Round(Convert.ToDouble(queryObj["Capacity"]) / 1073741824, 1)),
					string.Format("{0}", queryObj["Speed"])
				));
			}
		}
		public void GetMotherboardInfo() {
			Motherboard = new List<MotherboardObject>();
			ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
			foreach (ManagementObject queryObj in searcher.Get()) {
				Motherboard.Add(new MotherboardObject(
					string.Format("{0}", queryObj["Manufacturer"]),
					string.Format("{0}", queryObj["Product"])
				));
			}
		}
	}
}
