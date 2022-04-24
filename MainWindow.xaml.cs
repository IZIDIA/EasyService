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
		readonly string mac = HelperMethods.GetBeautiMacAddress();
		public ObservableCollection<RequestInfo> Requests;
		private ProgressDialogController controller;
		private readonly MainWindowViewModel viewModel;
		bool launch = false;

		public MainWindow() {
			InitializeComponent();
			viewModel = (MainWindowViewModel)DataContext;
			LaunchApp();
		}
		public async void LaunchApp() {
			controller = await this.ShowProgressAsync("Загрузка приложения", "Пожалуйста подождите...");
			ipv4.Text = "IP: " + HelperMethods.GetIPv4();
			hostName.Text = "Host: " + Dns.GetHostName();
			await RefreshWelcomePageAndRequestsList();
			mainContentControl.Content = new Views.Welcome();
		}
		public async Task RefreshWelcomePageAndRequestsList() {
			Refresh_Button.IsEnabled = false;
			if (!launch) {
				GetWelcomeText();
			}
			try {
				var client = new HttpClient();
				client.DefaultRequestHeaders.Add("Checker", viewModel.cheker);
				var responseBody = await client.GetStringAsync($"{viewModel.addres}/api/v1/{mac}/requests");
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
				launch = true;
				if (controller.IsOpen) {
					await controller.CloseAsync();
				}
			}
			catch {
				if (controller.IsOpen) {
					await controller.CloseAsync();
				}
				_ = await this.ShowMessageAsync("Ошибка", "Отсутствует соединение с сервером");
				if (Requests != null) {
					Requests.Clear();
				}
				ShowNetworkProblem();
				launch = false;
			}
			Refresh_Button.IsEnabled = true;
		}
		public async void GetWelcomeText() {
			try {
				var client = new HttpClient();
				client.DefaultRequestHeaders.Add("Checker", viewModel.cheker);
				var responseBody = await client.GetStringAsync($"{viewModel.addres}/api/v1/info");
				var requestsFromJson = JsonSerializer.Deserialize<Options>(responseBody);
				viewModel.WelcomeText = requestsFromJson.WelcomeTextApp;
				viewModel.WelcomeIconType = "CommentAlertOutline";
				viewModel.WelcomeIconColor = "#ffffff";
				Title = requestsFromJson.CompanyName;
			}
			catch { }
		}
		public void ShowNetworkProblem() {
			viewModel.WelcomeText = "Отсутствует соединение с сервером";
			viewModel.WelcomeIconType = "AlertOutline";
			viewModel.WelcomeIconColor = "#e8b600";
		}
		public void RequestsListSelectionChanged(object sender, SelectionChangedEventArgs e) {

		}

		private void Button_Refresh_Click(object sender, RoutedEventArgs e) {
			_ = RefreshWelcomePageAndRequestsList();
		}

		private void Button_Phone_Click(object sender, RoutedEventArgs e) {
			_ = Process.Start("explorer.exe", viewModel.addres + "/contacts");
		}
		private void Button_Doc_Click(object sender, RoutedEventArgs e) {
			_ = Process.Start("explorer.exe", viewModel.addres + "/docs");
		}
		private void Button_Web_Click(object sender, RoutedEventArgs e) {
			_ = Process.Start("explorer.exe", viewModel.addres);
		}

		private void Button_Create_Click(object sender, RoutedEventArgs e) {
			ScrollViewerForUserControl.ScrollToTop();
			mainContentControl.Content = new Views.RequestForm();
			Button_Create.Visibility = Visibility.Hidden;
			Button_Close.Visibility = Visibility.Visible;
		}

		private void Button_Close_Click(object sender, RoutedEventArgs e) {
			mainContentControl.Content = new Views.Welcome();
			Button_Create.Visibility = Visibility.Visible;
			Button_Close.Visibility = Visibility.Hidden;
		}
	}
}
