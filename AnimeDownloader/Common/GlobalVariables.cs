using System.Collections.Generic;
using System.Collections.ObjectModel;
using AnimeDownloader.Models;

namespace AnimeDownloader.Common {
	public static class GlobalVariables {
		public static ObservableCollection<AnimeInfoModel> AnimeInternal = new ObservableCollection<AnimeInfoModel>();
		public static ObservableCollection<RssFeedItemModel> RssFeedInternal = new ObservableCollection<RssFeedItemModel>();

		public static readonly Dictionary<int, string> Order = new Dictionary<int, string> {
			{0, "Added"},
			{1, "Name"},
			{2, "Watched"}
		};

		public static readonly Dictionary<string, string> Quality = new Dictionary<string, string> {
			{"Show all", "http://www.nyaa.se/?page=rss&cats=1_37&filter=0&term="},
			{"A+ only", "http://www.nyaa.se/?page=rss&cats=1_37&filter=3&term="},
			{"Trusted only", "http://www.nyaa.se/?page=rss&cats=1_37&filter=2&term="},
			{"Filter remakes", "http://www.nyaa.se/?page=rss&cats=1_37&filter=1&term="}
		};

		public static List<string> AllFiles = new List<string>();
		public static List<FolderModel> FolderPaths = new List<FolderModel>();

		public static List<string> Resolutions = new List<string> {"240", "360", "480", "560", "720", "1080", "1440"};
	}
}