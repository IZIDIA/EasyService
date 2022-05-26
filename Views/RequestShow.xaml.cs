using EasyService.HelperClasses;
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
		private readonly int id;
		public RequestShow(MainWindowViewModel viewModel, int _id) {
			InitializeComponent();
			MainGrid.Visibility = Visibility.Collapsed;
			this.viewModel = viewModel;
			id = _id;
			LaunchApp(id);
		}
		public async void LaunchApp(int id) {
			await RefreshRequestInfo(id);
			MainGrid.Visibility = Visibility.Visible;
			if (requestInfo.Status != "Отменено") {
				CancelPanel.Visibility = Visibility.Visible;
			}
			if (viewModel.mainWindow.controller.IsOpen) {
				await viewModel.mainWindow.controller.CloseAsync();
			}
			CommentsInput.ScrollToEnd();
		}
		private void ImageHyperlink_Click(object sender, RoutedEventArgs e) {
			if (requestInfo != null && requestInfo.Photo != null) {
				_ = Process.Start("explorer.exe", viewModel.addres + "/storage/" + requestInfo.Photo);
			}
		}
		private async void DenyRequestButton_Click(object sender, RoutedEventArgs e) {
			if (requestInfo.Status != "Отменено") {
				var mySettings = new MetroDialogSettings() {
					AffirmativeButtonText = "ОК",
					NegativeButtonText = "Отмена"
				};
				var result = await viewModel.mainWindow.ShowMessageAsync("Подтверждение", "Вы действительно хотите отменить заявку №" + id + "?",
					MessageDialogStyle.AffirmativeAndNegative, mySettings);
				if (result == MessageDialogResult.Affirmative) {
					DenyRequest();
				}
			}
		}
		private async void DenyRequest() {
			try {
				using (var client = new HttpClient()) {
					client.DefaultRequestHeaders.Add("Checker", viewModel.cheker);
					var resultContent = await client.GetStringAsync($"{viewModel.addres}/api/v1/{viewModel.mainWindow.mac}/" + id + "/cancel");
					if (resultContent.Contains("Successfully")) {
						if (viewModel.mainWindow.controller.IsOpen) {
							await viewModel.mainWindow.controller.CloseAsync();
						}
						viewModel.mainWindow.Refresh();
					}
					else {
						if (viewModel.mainWindow.controller.IsOpen) {
							await viewModel.mainWindow.controller.CloseAsync();
						}
						_ = await viewModel.mainWindow.ShowMessageAsync("Ошибка", "Не получилось отменить заявку");
					}
				}
			}
			catch {
				if (viewModel.mainWindow.controller.IsOpen) {
					await viewModel.mainWindow.controller.CloseAsync();
				}
				_ = await viewModel.mainWindow.ShowMessageAsync("Ошибка", "Отсутствует соединение с сервером");
			}
		}
		private void SendCommentButton_Click(object sender, RoutedEventArgs e) {
			PostComment();
		}
		private async void PostComment() {
			try {
				using (var multipartFormContent = new MultipartFormDataContent())
				using (var client = new HttpClient()) {
					client.BaseAddress = new Uri(viewModel.addres);
					client.DefaultRequestHeaders.Add("Checker", viewModel.cheker);
					multipartFormContent.Add(new StringContent(NewCommentInput.Text), "comment_text");
					var result = await client.PostAsync($"{viewModel.addres}/api/v1/{viewModel.mainWindow.mac}/" + id + "/comment", multipartFormContent);
					var resultContent = await result.Content.ReadAsStringAsync();
					if (resultContent.Contains("Successfully")) {
						if (viewModel.mainWindow.controller.IsOpen) {
							await viewModel.mainWindow.controller.CloseAsync();
						}
						viewModel.mainWindow.Refresh();
					}
					else {
						if (viewModel.mainWindow.controller.IsOpen) {
							await viewModel.mainWindow.controller.CloseAsync();
						}
						_ = await viewModel.mainWindow.ShowMessageAsync("Ошибка", "Не получилось создать комментарий");
					}
				}
			}
			catch {
				if (viewModel.mainWindow.controller.IsOpen) {
					await viewModel.mainWindow.controller.CloseAsync();
				}
				_ = await viewModel.mainWindow.ShowMessageAsync("Ошибка", "Отсутствует соединение с сервером");
			}
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
					if (requestInfo.Coments != null) {
						var comments = JsonConvert.DeserializeObject<List<Comments>>(requestInfo.Coments);
						var comments_string = string.Empty;
						foreach (var comment in comments) {
							comments_string += ">[" + comment.Time + "] " + comment.Name + ": " + comment.Message + Environment.NewLine;
						}
						CommentsInput.Text = comments_string;
					}
					var converter = new BrushConverter();
					switch (requestInfo.Status) {
						case "В обработке":
							StatusInput.Foreground = (Brush)converter.ConvertFromString("#00ffff");
							break;
						case "В работе":
							StatusInput.Foreground = (Brush)converter.ConvertFromString("#ff9d00");
							break;
						case "Завершено":
							StatusInput.Foreground = (Brush)converter.ConvertFromString("#00ff00");
							break;
						case "Отменено":
							StatusInput.Foreground = (Brush)converter.ConvertFromString("#ad0000");
							break;
						default:
							StatusInput.Foreground = (Brush)converter.ConvertFromString("#ffffff");
							break;
					}
					NameInput.Text = requestInfo.Name;
					ExecutorInput.Text = requestInfo.Admin != null ? requestInfo.Admin : "Не назначен";
					if (requestInfo.Email != null) {
						EmailPanel.Visibility = Visibility.Visible;
						EmailInput.Text = requestInfo.Email;
					}
					if (requestInfo.PhoneCallNumber != null) {
						PhonePanel.Visibility = Visibility.Visible;
						PhoneInput.Text = requestInfo.PhoneCallNumber;
					}
					CreatedAtInput.Text = requestInfo.CreatedAt;
					if (requestInfo.ClosedAt != null) {
						ClosedAtPanel.Visibility = Visibility.Visible;
						ClosedAtInput.Text = requestInfo.ClosedAt;
					}
					LocationInput.Text = requestInfo.Location;
					IpInput.Text = requestInfo.IpAddress;
					InventoryNumberInput.Text = requestInfo.InventoryNumber;
					if (requestInfo.SolutionWithMe != null) {
						WorkTimePanel.Visibility = Visibility.Visible;
						switch (requestInfo.SolutionWithMe) {
							case "1":
								WorkWithInput.Text = "Да";
								break;
							case "0":
								WorkWithInput.Text = "Нет";
								break;
							default:
								WorkWithInput.Text = "Неизвестно";
								break;
						}
						if (requestInfo.WorkTime != null) {
							var workTime = JsonConvert.DeserializeObject<WorkTime>(requestInfo.WorkTime);
							if (workTime.Monday != null) {
								MondayPanel.Visibility = Visibility.Visible;
								MondayFrom.Text = workTime.Monday.From;
								MondayTo.Text = workTime.Monday.To;
							}
							if (workTime.Tuesday != null) {
								TuesdayPanel.Visibility = Visibility.Visible;
								TuesdayFrom.Text = workTime.Tuesday.From;
								TuesdayTo.Text = workTime.Tuesday.To;
							}
							if (workTime.Wednesday != null) {
								WednesdayPanel.Visibility = Visibility.Visible;
								WednesdayFrom.Text = workTime.Wednesday.From;
								WednesdayTo.Text = workTime.Wednesday.To;
							}
							if (workTime.Thursday != null) {
								ThursdayPanel.Visibility = Visibility.Visible;
								ThursdayFrom.Text = workTime.Thursday.From;
								ThursdayTo.Text = workTime.Thursday.To;
							}
							if (workTime.Friday != null) {
								FridayPanel.Visibility = Visibility.Visible;
								FridayFrom.Text = workTime.Friday.From;
								FridayTo.Text = workTime.Friday.To;
							}
							if (workTime.Saturday != null) {
								SaturdayPanel.Visibility = Visibility.Visible;
								SaturdayFrom.Text = workTime.Saturday.From;
								SaturdayTo.Text = workTime.Saturday.To;
							}
							if (workTime.Sunday != null) {
								SundayPanel.Visibility = Visibility.Visible;
								SundayFrom.Text = workTime.Sunday.From;
								SundayTo.Text = workTime.Sunday.To;
							}
						}
					}
					else {
						WorkWithInput.Text = "Неважно";
					}
					switch (requestInfo.ProblemWithMyPc) {
						case 1:
							ProblemWithPcInput.Text = "Да";
							break;
						case 0:
							ProblemWithPcInput.Text = "Нет";
							break;
						default:
							ProblemWithPcInput.Text = "Неизвестно";
							break;
					}
					if (requestInfo.UserPassword != null) {
						PasswordPanel.Visibility = Visibility.Visible;
						PasswordInput.Text = requestInfo.UserPassword;
					}
					TitleInput.Text = requestInfo.Topic;
					MessageInput.Text = requestInfo.Text;
					if (requestInfo.Photo != null) {
						ImagePanel.Visibility = Visibility.Visible;
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
