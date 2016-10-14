using System.Windows.Controls;
using AnimeDownloader.ViewModels;

namespace AnimeDownloader.Views {
	/// <summary>
	///     Interaktionslogik für FolderView.xaml
	/// </summary>
	public partial class FolderView : UserControl {
		public FolderView() {
			InitializeComponent();
			DataContext = new FolderViewModel();
		}
	}
}