using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EasyService {
	public class HelperMethods {
		static public string GetMacAddress() {
			string macAddresses = "";
			foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces()) {
				if (nic.OperationalStatus == OperationalStatus.Up) {
					macAddresses += nic.GetPhysicalAddress().ToString();
					break;
				}
			}
			return macAddresses;
		}
		static public string GetBeautiMacAddress() {
			string macAddresses = GetMacAddress();
			if (macAddresses.Length != 12) {
				return macAddresses = "Error";
			}
			macAddresses = macAddresses.Insert(2, ":");
			macAddresses = macAddresses.Insert(5, ":");
			macAddresses = macAddresses.Insert(8, ":");
			macAddresses = macAddresses.Insert(11, ":");
			macAddresses = macAddresses.Insert(14, ":");
			return macAddresses;
		}
		static public string GetIPv4() {
			IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (IPAddress ip in host.AddressList) {
				if (ip.AddressFamily == AddressFamily.InterNetwork) {
					return ip.ToString();
				}
			}
			return null;
		}
	}
}
