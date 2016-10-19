namespace AnimeDownloader {
	/// <summary>
	///     Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow {
		public static MainWindow Instance;

		public MainWindow() {
			InitializeComponent();
			Instance = this;
		}
	}
}