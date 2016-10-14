using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeDownloader.Helpers {
	public class ProcessHelper {
		public static bool LookForClient() {
			var processNames = Process.GetProcesses().Any(x => x.ProcessName.Contains("deluged"));
			return processNames;
		}
	}
}
