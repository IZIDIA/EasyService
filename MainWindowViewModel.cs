using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EasyService {
	public class MainWindowViewModel : INotifyPropertyChanged {
		public MainWindow mainWindow;
		public readonly string addres = "http://127.0.0.1";
		public readonly string cheker = "accdede43f326c52d88d62b98de5e940";
		private string welcomeText;
		public string WelcomeText {
			get { return welcomeText; }
			set {
				welcomeText = value;
				OnPropertyChanged("WelcomeText");
			}
		}
		private string welcomeIconType;
		public string WelcomeIconType {
			get { return welcomeIconType; }
			set {
				welcomeIconType = value;
				OnPropertyChanged("WelcomeIconType");
			}
		}
		private string welcomeIconColor;
		public string WelcomeIconColor {
			get { return welcomeIconColor; }
			set {
				welcomeIconColor = value;
				OnPropertyChanged("WelcomeIconColor");
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "") {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}

		protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null) {
			if (!Equals(field, newValue)) {
				field = newValue;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
				return true;
			}

			return false;
		}
	}
}
