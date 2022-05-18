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
		private readonly RequestInfo requestInfo;
		public RequestShow(MainWindowViewModel viewModel) {
			InitializeComponent();
			this.viewModel = viewModel;
			
		}
		public async void LaunchApp() {




			//Вытаскивать ID


			await RefreshRequestInfo(1);

			//var image = new Image();
			//var fullFilePath = @"http://www.americanlayout.com/wp/wp-content/uploads/2012/08/C-To-Go-300x300.png";

			//BitmapImage bitmap = new BitmapImage();
			//bitmap.BeginInit();
			//bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
			//bitmap.EndInit();

			//image.Source = bitmap;
			//wrapPanel1.Children.Add(image);
		}
		private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e) {
			if (requestInfo != null && requestInfo.Photo != null) {
				_ = Process.Start("explorer.exe", viewModel.addres + "/" + requestInfo.Photo);
			}
		}

		private void DenyRequestButton_Click(object sender, RoutedEventArgs e) {

		}

		private void SendCommentButton_Click(object sender, RoutedEventArgs e) {

		}

		public async Task RefreshRequestInfo(string id) {
			try {
				var client = new HttpClient();
				client.DefaultRequestHeaders.Add("Checker", viewModel.cheker);
				var responseBody = await client.GetStringAsync($"{viewModel.addres}/api/v1/{viewModel.mainWindow.mac}/" + id);
				var requestsFromJson = JsonConvert.DeserializeObject<RequestInfo>(responseBody);
				if (requestsFromJson != null) {
					StatusInput.Text = requestsFromJson.Status;
					NameInput.Text = requestsFromJson.Name;
					ExecutorInput.Text = requestsFromJson.Admin;
					EmailInput.Text = requestsFromJson.Email;
					PhoneInput.Text = requestsFromJson.PhoneCallNumber;
					CreatedAtInput.Text = requestsFromJson.CreatedAt;
					ClosedAtInput.Text = requestsFromJson.ClosedAt;
					LocationInput.Text = requestsFromJson.Location;
					IpInput.Text = requestsFromJson.IpAddress;
					InventoryNumberInput.Text = requestsFromJson.InventoryNumber;
					WorkWithInput.Text = requestsFromJson.SolutionWithMe;
					//Сделать время в json формате
					ProblemWithPcinput.Text = requestsFromJson.ProblemWithMyPc.ToString();
					PasswordInput.Text = requestsFromJson.UserPassword;
					TitleInput.Text = requestsFromJson.Topic;
					MessageInput.Text = requestsFromJson.Text;


				}
			}
			catch {
				if (viewModel.mainWindow.controller.IsOpen) {
					await viewModel.mainWindow.controller.CloseAsync();
				}
				_ = await viewModel.mainWindow.ShowMessageAsync("Ошибка", "Отсутствует соединение с сервером");
				if (viewModel.mainWindow.Requests != null) {
					viewModel.mainWindow.Requests.Clear();
				}
				viewModel.mainWindow.ShowNetworkProblem();
				viewModel.mainWindow.launch = false;
			}
		}
	}
}
