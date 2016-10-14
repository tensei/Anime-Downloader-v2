using System.Collections.Generic;

namespace AnimeDownloader.Common {
	public class Config {
		public List<string> Groups = new List<string>();

		public string OngoingFolder = "";

		public int RefreshTime = 150;

		public string Resolution = "720p";
		public string Rss = "";

		public string TorrentClient = "";

		public string TorrentFiles = "";
	}
}