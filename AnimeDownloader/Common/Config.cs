using System.Collections.Generic;
using System.ComponentModel;

namespace AnimeDownloader.Common {
	public class Config : INotifyPropertyChanged{
		//public List<string> Groups = new List<string>();

		public string OngoingFolder { get; set; } = "";

		public int RefreshTime { get; set; } = 150;

		//public string Resolution = "720p";
		public string Rss { get; set; } = "";

		public string TorrentClient { get; set; } = "";

		//public string TorrentFiles = "";
	    public event PropertyChangedEventHandler PropertyChanged;
	}
}