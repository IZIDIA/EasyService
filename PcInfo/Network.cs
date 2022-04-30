using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Management;
using System.Net;
using System.Net.Sockets;
using NetInfo;

namespace EasyService.PcInfo {
	class Network {
		public bool PingYandex { get; set; }
		public bool PingGoogle { get; set; }
		public AdapterInfo adapterInfo { get; set; }
		public Network() {
			GetDnsAdress();
			GetPingYandex();
			GetPingGoogle();
		}
		private void GetDnsAdress() {
			adapterInfo = AdapterInfo.CreateInstance();
		}
		public void GetPingYandex() {
			try {
				var ping = new System.Net.NetworkInformation.Ping();
				var result = ping.Send("yandex.ru");
				if (result.Status == IPStatus.Success) {
					PingYandex = true;
				}
				else {
					PingYandex = false;
				}
			}
			catch {
				PingYandex = false;
			}
		}
		public void GetPingGoogle() {
			try {
				var ping = new System.Net.NetworkInformation.Ping();
				var result = ping.Send("google.com");
				if (result.Status == IPStatus.Success) {
					PingGoogle = true;
				}
				else {
					PingGoogle = false;
				}
			}
			catch {
				PingGoogle = false;
			}
		}
	}
}
