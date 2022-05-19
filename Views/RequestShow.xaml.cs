using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

namespace EasyService.Views {
	public partial class RequestShow : UserControl {
		private readonly MainWindowViewModel viewModel;
		private RequestInfo requestInfo;
		public RequestShow(MainWindowViewModel viewModel, int id) {
			InitializeComponent();
			MainGrid.Visibility = Visibility.Collapsed;
			this.viewModel = viewModel;
			LaunchApp(id);
		}
		public async void LaunchApp(int id) {
			await RefreshRequestInfo(id);
			MainGrid.Visibility = Visibility.Visible;
			if (viewModel.mainWindow.controller.IsOpen) {
				await viewModel.mainWindow.controller.CloseAsync();
			}
		}
		private void ImageHyperlink_Click(object sender, RoutedEventArgs e) {
			if (requestInfo != null && requestInfo.Photo != null) {
				_ = Process.Start("explorer.exe", viewModel.addres + "/storage/" + requestInfo.Photo);
			}
		}

		private void DenyRequestButton_Click(object sender, RoutedEventArgs e) {

		}

		private void SendCommentButton_Click(object sender, RoutedEventArgs e) {

		}

		public async Task RefreshRequestInfo(int id) {
			try {
				var client = new HttpClient();
				client.DefaultRequestHeaders.Add("Checker", viewModel.cheker);
				var responseBody = await client.GetStringAsync($"{viewModel.addres}/api/v1/{viewModel.mainWindow.mac}/" + id);
				requestInfo = JsonConvert.DeserializeObject<RequestInfo>(responseBody);
				if (requestInfo != null) {
					IdInput.Content = "Заявка №" + id;
					StatusInput.Text = requestInfo.Status;
					NameInput.Text = requestInfo.Name;
					ExecutorInput.Text = requestInfo.Admin;
					EmailInput.Text = requestInfo.Email;
					PhoneInput.Text = requestInfo.PhoneCallNumber;
					CreatedAtInput.Text = requestInfo.CreatedAt;
					ClosedAtInput.Text = requestInfo.ClosedAt;
					LocationInput.Text = requestInfo.Location;
					IpInput.Text = requestInfo.IpAddress;
					InventoryNumberInput.Text = requestInfo.InventoryNumber;
					WorkWithInput.Text = requestInfo.SolutionWithMe;
					//Сделать время в json формате
					ProblemWithPcinput.Text = requestInfo.ProblemWithMyPc.ToString();
					PasswordInput.Text = requestInfo.UserPassword;
					TitleInput.Text = requestInfo.Topic;
					MessageInput.Text = requestInfo.Text;
					if (requestInfo.Photo != null) {
						var bitmap = new BitmapImage();
						bitmap.BeginInit();
						bitmap.UriSource = new Uri(viewModel.addres + "/storage/" + requestInfo.Photo, UriKind.Absolute);
						bitmap.EndInit();
						PhotoInput.Source = bitmap;
					}
				}
			}
			catch {
				if (viewModel.mainWindow.controller.IsOpen) {
					await viewModel.mainWindow.controller.CloseAsync();
				}
				_ = await viewModel.mainWindow.ShowMessageAsync("Ошибка", "Отсутствует соединение с сервером");
				viewModel.mainWindow.ShowNetworkProblem();
				viewModel.mainWindow.launch = false;
				viewModel.mainWindow.CloseAnyForm();
			}
		}
	}
}
