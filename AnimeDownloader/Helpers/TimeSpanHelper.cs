using System;

namespace AnimeDownloader.Helpers {
	public static class TimeSpanHelper {
		public static string Get(DateTime created) {
			var dateNow = DateTime.Now;
			var diff = dateNow - created;
			return diff.Days < 1 ? "Today" : $"{diff.Days} Day(s) ago.";
		}
	}
}