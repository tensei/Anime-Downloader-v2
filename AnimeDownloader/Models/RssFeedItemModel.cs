using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using AnimeDownloader.Common;
using AnimeDownloader.Helpers;
using AnimeDownloader.Helpers.Torrent_Clients;
using AnimeDownloader.ViewModels;
using PropertyChanged;

namespace AnimeDownloader.Models {
	public class RssFeedItemModel : INotifyPropertyChanged {
		public RssFeedItemModel() {
			DownloadCommand = new ActionCommand(Download);

		}

		public string Name { get; set; }
		public string[] NameArray { get; set; }
		public string Description { get; set; }
		public int Quality { get; set; } = 3;
		public string DownloadLink { get; set; }
		public string Link { get; set; }
		public string SavePath { get; set; }
		public string Added => DaysSinceRelease();
		public DateTime Released { get; set; }
		public string SuggestedFolderName { get; set; }

		public ICommand DownloadCommand { get; }

		private string DaysSinceRelease() {
			var dateNow = DateTime.Now;
			var diff = dateNow - Released;
			if (diff.Days < 0)
				return "Unknown";
			return diff.Days == 0 ? "Today" : $"{diff.Days} day(s) ago";
		}

		private void Download() {
			if (!Directory.Exists(SavePath)) {
				try {
					Directory.CreateDirectory(SavePath);
				} catch {
					
					MainWindowViewModel.Instance.MessageQueue.Enqueue($"Invalid Save path!\n{SavePath}");
					return;
				}
			}
			ConvertHelper.RssModelToList(this, SavePath);
			if (!Directory.Exists(SavePath)) Directory.CreateDirectory(SavePath);
			QbitTorrentHelper.Add(DownloadLink, SavePath);
			MainWindowViewModel.Instance.MessageQueue.Enqueue($"Downloading \n{Name}");
		}

	    public event PropertyChangedEventHandler PropertyChanged;
	}
}