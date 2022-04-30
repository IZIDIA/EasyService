using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace NetInfo {
	public class AdapterInfo {
		private static AdapterInfo adapterInfo;
		private static object objLock = new object();
		private NetworkInterface[] adapters;
		public List<AdapterObject> listAdapter;
		private AdapterInfo() {
			listAdapter = new List<AdapterObject>();
			RefreshInfos();
		}
		public static AdapterInfo CreateInstance() {
			if (adapterInfo == null)
				lock (objLock) {
					if (adapterInfo == null)
						adapterInfo = new AdapterInfo();
				}
			return adapterInfo;
		}
		public void RefreshAdapterSpeed(ref AdapterObject adapterObject) {
			adapters = NetworkInterface.GetAllNetworkInterfaces();
			var adapterName = adapterObject.Name;
			var adapter = adapters.Where(x => x.Name == adapterName).SingleOrDefault();
			if (adapter != null) {
				adapterObject.Speed = adapter.Speed;
			}
		}
		public void RefreshInfos() {
			listAdapter.Clear();
			adapters = NetworkInterface.GetAllNetworkInterfaces();
			foreach (NetworkInterface adapter in adapters) {
				var ipProp = adapter.GetIPProperties();
				foreach (UnicastIPAddressInformation ip in ipProp.UnicastAddresses) {
					if ((adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet) &&
							ip.Address.AddressFamily == AddressFamily.InterNetwork) //
					{
						IPInterfaceProperties adapterProp = adapter.GetIPProperties();
						IPv4InterfaceProperties adapterPropV4 = adapterProp.GetIPv4Properties();
						GatewayIPAddressInformationCollection gate = adapter.GetIPProperties().GatewayAddresses;
						List<string> gatesList = new List<string>();
						List<string> dhcpList = new List<string>();
						List<string> dnsList = new List<string>();
						foreach (var item in gate) {
							gatesList.Add(item.Address.ToString());
						}
						foreach (var item in adapterProp.DhcpServerAddresses) {
							dhcpList.Add(item.ToString());
						}
						foreach (var item in adapterProp.DnsAddresses) {
							dnsList.Add(item.ToString());
						}
						listAdapter.Add(new AdapterObject {
							Name = adapter.Name,
							Description = adapter.Description,
							NetworkInterfaceType = adapter.NetworkInterfaceType.ToString(),
							PhysicalAddress = adapter.GetPhysicalAddress().ToString(),
							//	IsReceiveOnly = adapter.IsReceiveOnly,
							//SupportMulticast = adapter.SupportsMulticast,
							//	IsOperationalStatusUp = adapter.OperationalStatus == OperationalStatus.Up,
							Speed = adapter.Speed,
							IpAddress = ip.Address.ToString(),
							SubnetMask = ip.IPv4Mask.ToString(),
							Gateway = gatesList,
							DnsSuffix = adapterProp.DnsSuffix,
							IsDnsEnabled = adapterProp.IsDnsEnabled,
							IsDynamicDnsEnabled = adapterProp.IsDynamicDnsEnabled,
							//Index = adapterPropV4.Index,
							//Mtu = adapterPropV4.Mtu,
							//IsAutomaticPrivateAddressingActive = adapterPropV4.IsAutomaticPrivateAddressingActive,
							//IsAutomaticPrivateAddressingEnabled = adapterPropV4.IsAutomaticPrivateAddressingEnabled,
							//IsForwardingEnabled = adapterPropV4.IsForwardingEnabled,
							//UsesWins = adapterPropV4.UsesWins,
							IsDHCPEnabled = adapterPropV4.IsDhcpEnabled,
							DHCPServer = dhcpList,
							DNSServer = dnsList,
							//Internet = adapter.GetIPv4Statistics().BytesReceived > 0 && adapter.GetIPv4Statistics().BytesSent > 0
						});
					}
				}
			}
		}
	}
}
