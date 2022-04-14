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
		public string WelcomeText {
			get { return welcomeText; }
			set {
				welcomeText = value;
				OnPropertyChanged("WelcomeText");
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "") {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
	}
}
