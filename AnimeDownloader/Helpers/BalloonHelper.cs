using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardcodet.Wpf.TaskbarNotification;
using System.Drawing;
namespace AnimeDownloader.Helpers {
	class BalloonHelper {
		public static void Show(string message) {
			MainWindow.Instance.NotifyIcon.ShowBalloonTip("Anime Downloader", message, BalloonIcon.Info);
		}
	}
}
