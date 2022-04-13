using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Collections.ObjectModel;

namespace EasyService {
	public partial class MainWindow : MetroWindow {
		static string addres = "http://laravelproject";
		static string mac = HelperMethods.GetBeautiMacAddress();
		static string cheker = "accdede43f326c52d88d62b98de5e940";
		public ObservableCollection<RequestInfo> Requests;
		public MainWindow() {
			InitializeComponent();
			ipv4.Text = "IP: " + HelperMethods.GetIPv4();
			hostName.Text = "Host: " + Dns.GetHostName();
			RefreshRequestsList();
			/*Requests = new ObservableCollection<RequestInfo>
			 {
						new RequestInfo {Id=1, CreatedAt="Apple" },
						new RequestInfo {Id=2, CreatedAt="Microsoft" },
				};*/
			
		}
		public async void RefreshRequestsList() {
			try {
				HttpClient client = new HttpClient();
				client.DefaultRequestHeaders.Add("Checker", cheker);
				var responseBody = await client.GetStringAsync($"{addres}/api/v1/{mac}/requests");
				var requestsFromJson = JsonSerializer.Deserialize<List<RequestInfo>>(responseBody);
				Requests = new ObservableCollection<RequestInfo>();
				foreach (var item in requestsFromJson) {
					Requests.Add(new RequestInfo { Id = item.Id, CreatedAt = item.CreatedAt });
				}
				RequestsList.ItemsSource = Requests;
				//await this.ShowMessageAsync("Good", restoredPerson[0].Name);
			}
			catch {
				await this.ShowMessageAsync("Ошибка", "Отсутствует соединение с сервером");
			}
		}
		public void RequestsListSelectionChanged(object sender, SelectionChangedEventArgs e) {

		}
	}
}
