using System;
using System.Collections.Generic;
using System.Diagnostics;
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
	public partial class RequestShow : UserControl {
		private readonly MainWindowViewModel viewModel;
		private readonly RequestInfo requestInfo;
		public RequestShow(MainWindowViewModel viewModel) {
			InitializeComponent();
			this.viewModel = viewModel;
		}

		private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e) {
			if (!(requestInfo is null) && !(requestInfo.Photo is null)) {
				_ = Process.Start("explorer.exe", viewModel.addres + "/" + requestInfo.Photo);
			}
		}

		private void DenyRequestButton_Click(object sender, RoutedEventArgs e) {

		}

		private void SendCommentButton_Click(object sender, RoutedEventArgs e) {

		}
	}
}
