using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EasyService {
	public class MainWindowViewModel : INotifyPropertyChanged {
		private string welcomeText;
		private string welcomeIconType;
		private string welcomeIconColor;
		public string WelcomeText {
			get { return welcomeText; }
			set {
				welcomeText = value;
				OnPropertyChanged("WelcomeText");
			}
		}
		public string WelcomeIconType {
			get { return welcomeIconType; }
			set {
				welcomeIconType = value;
				OnPropertyChanged("WelcomeIconType");
			}
		}
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

		private bool enableAnonym;

		public bool EnableAnonym { get => enableAnonym; set => SetProperty(ref enableAnonym, value); }
	}
}
