using System.Windows.Controls;
using AnimeDownloader.ViewModels;

namespace AnimeDownloader.Views {
	/// <summary>
	///     Interaktionslogik für SettingsView.xaml
	/// </summary>
	public partial class SettingsView : UserControl {
		public SettingsView() {
			InitializeComponent();
			DataContext = new SettingsViewModel();
		}
	}
}