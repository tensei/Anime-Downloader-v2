namespace AnimeDownloader.Helpers {
	public class NyaaHelper {
		public static int GetQuality(string summary) {
			if (summary.ToLower().Contains("remake")) return 4;
			if (summary.ToLower().Contains("a+ - trusted")) return 1;
			if (summary.ToLower().Contains("trusted")) return 2;
			return 3;
		}
	}
}