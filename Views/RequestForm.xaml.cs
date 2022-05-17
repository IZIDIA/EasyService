using System;
using System.Collections.Generic;
using System.Linq;
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
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using EasyService.PcInfo;
using EasyService.HelperClasses;

namespace EasyService.Views {
	public partial class RequestForm : UserControl {
		private readonly MainWindowViewModel viewModel;
		private string image;
		private PersonalData personalData;
		public RequestForm(MainWindowViewModel viewModel) {
			InitializeComponent();
			this.viewModel = viewModel;
			if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "PersonalData.json")) {
				try {
					personalData = JsonConvert.DeserializeObject<PersonalData>(
						File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "PersonalData.json")
					);
					FirstNameInput.Text = personalData.FirstName;
					LastNameInput.Text = personalData.LastName;
					EmailInput.Text = personalData.Email;
					LocationInput.Text = personalData.Location;
					PhoneInput.Text = personalData.Phone;
					SavePersonDataInput.IsOn = true;
				}
				catch {
					if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "PersonalData.json")) {
						File.Delete(AppDomain.CurrentDomain.BaseDirectory + "PersonalData.json");
					}
				}
			}
		}
		private async void ImagePickButton_Click(object sender, RoutedEventArgs e) {
			var dialog = new Microsoft.Win32.OpenFileDialog {
				Filter = "Изображение(*.JPG;*.JPEG;*.PNG)|*.JPG;*.JPEG;*.PNG"
			};
			var result = dialog.ShowDialog();
			if (result == true) {
				var size = new FileInfo(dialog.FileName).Length;
				if (size > 10485760) {
					_ = await viewModel.mainWindow.ShowMessageAsync("Предупреждение", "Размер файла должен быть менее 10 МБ");
				}
				else {
					image = dialog.FileName;
					ImageName.Text = dialog.SafeFileName;
				}
			}
		}

		private void AnonimInput_Toggled(object sender, RoutedEventArgs e) {
			var toggleSwitch = sender as ToggleSwitch;
			if (toggleSwitch != null) {
				if (toggleSwitch.IsOn) {
					NamePanel.Visibility = Visibility.Collapsed;
					EmailPanel.Visibility = Visibility.Collapsed;
					PhonePanel.Visibility = Visibility.Collapsed;
					WorkTimePanel.Visibility = Visibility.Collapsed;
					WorkTimeSeparator.Visibility = Visibility.Collapsed;
					SavePersonalDataPanel.Visibility = Visibility.Collapsed;
				}
				else {
					NamePanel.Visibility = Visibility.Visible;
					EmailPanel.Visibility = Visibility.Visible;
					PhonePanel.Visibility = Visibility.Visible;
					WorkTimePanel.Visibility = Visibility.Visible;
					WorkTimeSeparator.Visibility = Visibility.Visible;
					SavePersonalDataPanel.Visibility = Visibility.Visible;
				}
			}
		}

		private void RadioInput_Click(object sender, RoutedEventArgs e) {
			WorkTimeInputPanel.Visibility = (bool)RadioInputOne.IsChecked ? Visibility.Collapsed : Visibility.Visible;
		}

		private void PasswordCheck_Click(object sender, RoutedEventArgs e) {
			var PasswordCheck = sender as CheckBox;
			if ((bool)PasswordCheck.IsChecked) {
				PasswordPanel.Visibility = Visibility.Visible;
			}
			else {
				PasswordInput.Text = string.Empty;
				PasswordPanel.Visibility = Visibility.Collapsed;
			}
		}

		private async void SendRequestButton_Click(object sender, RoutedEventArgs e) {
			if (CheckFormInputs()) {
				if (!AnonimInput.IsOn && !IsValidEmailAddress(EmailInput.Text)) {
					_ = await viewModel.mainWindow.ShowMessageAsync("Предупреждение", "Заполните правильно поле Email");
					return;
				}
				var solution_with_me = "1";
				if ((bool)RadioInputTwo.IsChecked) {
					solution_with_me = "2";
				}
				else if ((bool)RadioInputThree.IsChecked) {
					solution_with_me = "3";
				}
				if (SavePersonDataInput.IsOn) {
					var personalData = new PersonalData {
						FirstName = FirstNameInput.Text,
						LastName = LastNameInput.Text,
						Email = EmailInput.Text,
						Location = LocationInput.Text,
						Phone = PhoneInput.Text
					};
					File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "PersonalData.json", JsonConvert.SerializeObject(personalData));
				}
				viewModel.mainWindow.controller = await viewModel.mainWindow.ShowProgressAsync("Отправка заявки", "Пожалуйста подождите...");
				viewModel.mainWindow.controller.SetIndeterminate();
				PostRequestInfo(solution_with_me);
			}
			else {
				_ = await viewModel.mainWindow.ShowMessageAsync("Предупреждение", "Заполните все обязательные поля");
			}
		}

		private async void PostRequestInfo(string solution_with_me) {
			try {
				using (var multipartFormContent = new MultipartFormDataContent())
				using (var client = new HttpClient()) {
					client.BaseAddress = new Uri(viewModel.addres);
					client.DefaultRequestHeaders.Add("Checker", viewModel.cheker);
					multipartFormContent.Add(new StringContent(AnonimInput.IsOn ? "1" : "0"), "anonym");
					multipartFormContent.Add(new StringContent(HelperMethods.GetBeautiMacAddress()), "mac");
					multipartFormContent.Add(new StringContent(HelperMethods.GetIPv4()), "ip_address");
					multipartFormContent.Add(new StringContent(solution_with_me), "solution_with_me");
					multipartFormContent.Add(new StringContent(FirstNameInput.Text), "first_name");
					multipartFormContent.Add(new StringContent(LastNameInput.Text), "last_name");
					multipartFormContent.Add(new StringContent(EmailInput.Text), "email");
					multipartFormContent.Add(new StringContent(LocationInput.Text), "location");
					multipartFormContent.Add(new StringContent(PhoneInput.Text), "phone_call_number");
					multipartFormContent.Add(new StringContent(InventoryNumberInput.Text), "inventory_number");
					multipartFormContent.Add(new StringContent((bool)PasswordCheck.IsChecked ? "on" : "off"), "problem_with_my_pc");
					multipartFormContent.Add(new StringContent(PasswordInput.Text), "user_password");
					multipartFormContent.Add(new StringContent(TitleInput.Text), "topic");
					multipartFormContent.Add(new StringContent(MessageInput.Text), "text");
					multipartFormContent.Add(new StringContent((bool)MondayCheck.IsChecked ? "on" : "off"), "monday");
					multipartFormContent.Add(new StringContent(MondayFrom.SelectedDateTime?.ToString("HH:mm")), "from_monday");
					multipartFormContent.Add(new StringContent(MondayTo.SelectedDateTime?.ToString("HH:mm")), "to_monday");
					multipartFormContent.Add(new StringContent((bool)TuesdayCheck.IsChecked ? "on" : "off"), "tuesday");
					multipartFormContent.Add(new StringContent(TuesdayFrom.SelectedDateTime?.ToString("HH:mm")), "from_tuesday");
					multipartFormContent.Add(new StringContent(TuesdayTo.SelectedDateTime?.ToString("HH:mm")), "to_tuesday");
					multipartFormContent.Add(new StringContent((bool)WednesdayCheck.IsChecked ? "on" : "off"), "wednesday");
					multipartFormContent.Add(new StringContent(WednesdayFrom.SelectedDateTime?.ToString("HH:mm")), "from_wednesday");
					multipartFormContent.Add(new StringContent(WednesdayTo.SelectedDateTime?.ToString("HH:mm")), "to_wednesday");
					multipartFormContent.Add(new StringContent((bool)ThursdayCheck.IsChecked ? "on" : "off"), "thursday");
					multipartFormContent.Add(new StringContent(ThursdayFrom.SelectedDateTime?.ToString("HH:mm")), "from_thursday");
					multipartFormContent.Add(new StringContent(ThursdayTo.SelectedDateTime?.ToString("HH:mm")), "to_thursday");
					multipartFormContent.Add(new StringContent((bool)FridayCheck.IsChecked ? "on" : "off"), "friday");
					multipartFormContent.Add(new StringContent(FridayFrom.SelectedDateTime?.ToString("HH:mm")), "from_friday");
					multipartFormContent.Add(new StringContent(FridayTo.SelectedDateTime?.ToString("HH:mm")), "to_friday");
					multipartFormContent.Add(new StringContent((bool)SaturdayCheck.IsChecked ? "on" : "off"), "saturday");
					multipartFormContent.Add(new StringContent(SaturdayFrom.SelectedDateTime?.ToString("HH:mm")), "from_saturday");
					multipartFormContent.Add(new StringContent(SaturdayTo.SelectedDateTime?.ToString("HH:mm")), "to_saturday");
					multipartFormContent.Add(new StringContent((bool)SundayCheck.IsChecked ? "on" : "off"), "sunday");
					multipartFormContent.Add(new StringContent(SundayFrom.SelectedDateTime?.ToString("HH:mm")), "from_sunday");
					multipartFormContent.Add(new StringContent(SundayTo.SelectedDateTime?.ToString("HH:mm")), "to_sunday");
					if (File.Exists(image)) {
						var fileStreamContent = new StreamContent(File.OpenRead(image));
						var fileContent = new ByteArrayContent(await fileStreamContent.ReadAsByteArrayAsync());
						fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
						multipartFormContent.Add(fileContent, "photo", System.IO.Path.GetFileName(image));
					}
					if ((bool)PasswordCheck.IsChecked) {
						await Task.Run(() => {
							multipartFormContent.Add(new StringContent(JsonConvert.SerializeObject(new OperatingSystemInfo())), "operating_system");
							multipartFormContent.Add(new StringContent(JsonConvert.SerializeObject(new Specs())), "specs");
							multipartFormContent.Add(new StringContent(JsonConvert.SerializeObject(new Temps())), "temps");
							multipartFormContent.Add(new StringContent(JsonConvert.SerializeObject(new ActiveProcesses())), "active_processes");
							multipartFormContent.Add(new StringContent(JsonConvert.SerializeObject(new Network())), "network");
							multipartFormContent.Add(new StringContent(JsonConvert.SerializeObject(new Peripherals())), "devices");
							multipartFormContent.Add(new StringContent(JsonConvert.SerializeObject(new Disks())), "disks");
							multipartFormContent.Add(new StringContent(JsonConvert.SerializeObject(new InstalledPrograms())), "installed_programs");
							multipartFormContent.Add(new StringContent(JsonConvert.SerializeObject(new AutoloadPrograms())), "autoload_programs");
							multipartFormContent.Add(new StringContent(JsonConvert.SerializeObject(new Performance())), "performance");
						});
					}
					var result = await client.PostAsync("/api/v1/requests", multipartFormContent);
					var resultContent = await result.Content.ReadAsStringAsync();
					if (resultContent.Contains("Successfully")) {
						if (viewModel.mainWindow.controller.IsOpen) {
							await viewModel.mainWindow.controller.CloseAsync();
						}
						_ = await viewModel.mainWindow.ShowMessageAsync("Отлично", "Заявка успешно создана");
						viewModel.mainWindow.CloseAnyForm();
						_ = viewModel.mainWindow.RefreshWelcomePageAndRequestsList();
					}
					else {
						if (viewModel.mainWindow.controller.IsOpen) {
							await viewModel.mainWindow.controller.CloseAsync();
						}
						_ = await viewModel.mainWindow.ShowMessageAsync("Ошибка", "Не получилось создать заявку");
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
		private bool CheckFormInputs() {
			if (AnonimInput.IsOn) {
				return LocationInput.Text.Length != 0
				&& TitleInput.Text.Length != 0
				&& MessageInput.Text.Length != 0;
			}
			return FirstNameInput.Text.Length != 0
				&& LastNameInput.Text.Length != 0
				&& EmailInput.Text.Length != 0
				&& LocationInput.Text.Length != 0
				&& PhoneInput.Text.Length != 0
				&& TitleInput.Text.Length != 0
				&& MessageInput.Text.Length != 0;
		}

		private void SavePersonDataInput_Toggled(object sender, RoutedEventArgs e) {
			if (!SavePersonDataInput.IsOn) {
				if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "PersonalData.json")) {
					File.Delete(AppDomain.CurrentDomain.BaseDirectory + "PersonalData.json");
				}
			}
		}
		private void PhoneInput_PreviewTextInput(object sender, TextCompositionEventArgs e) {
			var regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}
		private bool IsValidEmailAddress(string s) {
			var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
			return regex.IsMatch(s);
		}
	}
}
