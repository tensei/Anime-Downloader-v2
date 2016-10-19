using System.Diagnostics;
using System.Linq;

namespace AnimeDownloader.Helpers {
	public class ProcessHelper {
		public static bool LookForClient() {
			var processNames = Process.GetProcesses().Any(x => x.ProcessName.Contains("deluged"));
			return processNames;
		}
	}
}