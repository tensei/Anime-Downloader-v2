using System.Drawing;
namespace AnimeDownloader {
	/// <summary>
	///     Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow {
		public MainWindow() {
			InitializeComponent();
			Instance = this;
		}
		public static MainWindow Instance;
	}
}