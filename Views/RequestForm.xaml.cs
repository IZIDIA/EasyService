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

namespace EasyService.Views {
	public partial class RequestForm : UserControl {
		public RequestForm() {
			InitializeComponent();
		}
		private void ImagePickButton_Click(object sender, RoutedEventArgs e) {
			var dialog = new Microsoft.Win32.OpenFileDialog {
				Filter = "Изображение(*.JPG;*.JPEG;*.PNG)|*.JPG;*.JPEG;*.PNG"
			};
			var result = dialog.ShowDialog();
			if (result == true) {
				ImageName.Text = dialog.SafeFileName;
			}
		}
	}
}
