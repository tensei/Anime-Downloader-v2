using Hardcodet.Wpf.TaskbarNotification;

namespace AnimeDownloader.Helpers {
	internal class BalloonHelper {
		public static void Show(string message) {
			MainWindow.Instance.NotifyIcon.ShowBalloonTip("Anime Downloader", message, BalloonIcon.Info);
		}
	}
}