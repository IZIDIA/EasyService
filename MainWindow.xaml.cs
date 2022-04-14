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
using EasyService.Models;
using System.Diagnostics;

namespace EasyService {
	public partial class MainWindow : MetroWindow {
		string addres = "http://laravelproject";
		string mac = HelperMethods.GetBeautiMacAddress();
		string cheker = "accdede43f326c52d88d62b98de5e940";
		public ObservableCollection<RequestInfo> Requests;
		public MainWindow() {
			InitializeComponent();
			//DataContext = new MainWindowViewModel();
			this.mainContentControl.Content = new Views.Welcome();
			ipv4.Text = "IP: " + HelperMethods.GetIPv4();
			hostName.Text = "Host: " + Dns.GetHostName();
			LaunchApp();
		}
		public async void LaunchApp() {
			try {
				HttpClient client = new HttpClient();
				client.DefaultRequestHeaders.Add("Checker", cheker);
				var responseBody = await client.GetStringAsync($"{addres}/api/v1/info");
				var requestsFromJson = JsonSerializer.Deserialize<Options>(responseBody);
				var viewModel = (MainWindowViewModel)DataContext;
				viewModel.WelcomeText = requestsFromJson.WelcomeTextApp;
				this.Title = requestsFromJson.CompanyName;
				RefreshRequestsList();
			}
			catch {
				await this.ShowMessageAsync("Ошибка", "Отсутствует соединение с сервером");
			}
		}
		public async void RefreshRequestsList() {
			Refresh_Button.IsEnabled = false;
			try {
				HttpClient client = new HttpClient();
				client.DefaultRequestHeaders.Add("Checker", cheker);
				var responseBody = await client.GetStringAsync($"{addres}/api/v1/{mac}/requests");
				var requestsFromJson = JsonSerializer.Deserialize<List<RequestInfo>>(responseBody);
				Requests = new ObservableCollection<RequestInfo>();
				foreach (var item in requestsFromJson) {
					switch (item.Status) {
						case "В обработке":
							Requests.Add(new RequestInfo {
								IdName = "Заявка №" + item.Id,
								CreatedAt = item.CreatedAt,
								Icon = "ClipboardTextClockOutline",
								IconColor = "#00cfcf"
							});
							break;
						case "В работе":
							Requests.Add(new RequestInfo {
								IdName = "Заявка №" + item.Id,
								CreatedAt = item.CreatedAt,
								Icon = "ProgressWrench",
								IconColor = "#e06900"
							});
							break;
						case "Завершено":
							Requests.Add(new RequestInfo {
								IdName = "Заявка №" + item.Id,
								CreatedAt = item.CreatedAt,
								Icon = "CheckCircleOutline",
								IconColor = "#85de00"
							});
							break;
						case "Отменено":
							Requests.Add(new RequestInfo {
								IdName = "Заявка №" + item.Id,
								CreatedAt = item.CreatedAt,
								Icon = "CloseCircleOutline",
								IconColor = "#b30000"
							});
							break;
						default:
							Requests.Add(new RequestInfo {
								IdName = "Заявка №" + item.Id,
								CreatedAt = item.CreatedAt,
								Icon = "HelpCircleOutline",
								IconColor = "#f5f5f5"
							});
							break;
					}
				}
				RequestsList.ItemsSource = Requests;
			}
			catch {
				await this.ShowMessageAsync("Ошибка", "Отсутствует соединение с сервером");
			}
			Refresh_Button.IsEnabled = true;
		}
		public void RequestsListSelectionChanged(object sender, SelectionChangedEventArgs e) {

		}

		private void Button_Refresh_Click(object sender, RoutedEventArgs e) {
			RefreshRequestsList();
		}

		private void Button_Phone_Click(object sender, RoutedEventArgs e) {
			Process.Start("explorer.exe", addres + "/contacts");
		}
		private void Button_Doc_Click(object sender, RoutedEventArgs e) {
			Process.Start("explorer.exe", addres + "/docs");
		}
		private void Button_Web_Click(object sender, RoutedEventArgs e) {
			Process.Start("explorer.exe", addres);
		}

		private void Button_Create_Click(object sender, RoutedEventArgs e) {
			this.mainContentControl.Content = new Views.RequestForm();
			this.Button_Create.Visibility = Visibility.Hidden;
			this.Button_Close.Visibility = Visibility.Visible;
		}

		private void Button_Close_Click(object sender, RoutedEventArgs e) {
			this.mainContentControl.Content = new Views.Welcome();
			this.Button_Create.Visibility = Visibility.Visible;
			this.Button_Close.Visibility = Visibility.Hidden;
		}
	}
}
