using System.IO;
using Microsoft.Win32;

namespace AnimeDownloader.Helpers {
	public static class FindDefaultTorrentClientHelper {
		public static string Find() {
			var executable = Registry.ClassesRoot.OpenSubKey(".torrent");
			if (executable == null) return "";
			var path = Registry.ClassesRoot.OpenSubKey(executable.GetValue("") + @"\shell\open\command");
			if (path == null) return "";
			var p = path.GetValue("").ToString().Replace("\"", string.Empty).Replace(" %1", string.Empty);
			return !File.Exists(p) ? string.Empty : p;
		}
	}
}