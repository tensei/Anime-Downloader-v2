using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using AnimeDownloader.Common;

namespace AnimeDownloader.Helpers.Torrent_Clients {
	public static class DelugeHelper {
		public static void Add(string link, string safeLocation) {
			//"add -p 'D:/Program Files (x86)/Deluge' 'D:/Development/python34/Anime checker/torrents/[HorribleSubs] Hibike! Euphonium - 13 [720p].mkv.torrent'"
			var call = $"\"add -p '{safeLocation}' '{link.Replace("https", "http")}'\"";
			call = Regex.Replace(call, @"\\+", "/");
			StartProcess(call);
		}

		private static string _fullOutput = string.Empty;
		private static void StartProcess(string call, bool output = false) {
			var sta = new Process {
				StartInfo = {
					FileName = Settings.Config.TorrentClient,
					Arguments = call
				},
				EnableRaisingEvents = true
			};
			//uncomment to see output
			if (output) {
				sta.OutputDataReceived += (sender, args) => { _fullOutput += $"{args.Data}\n"; };
				sta.StartInfo.RedirectStandardOutput = true;
				sta.StartInfo.RedirectStandardError = true;

			}
			sta.StartInfo.CreateNoWindow = true;
			sta.StartInfo.UseShellExecute = false;
			sta.Start();
			if (!output) return;
			sta.BeginErrorReadLine();
			sta.BeginOutputReadLine();
			sta.WaitForExit();
			sta.Dispose();
		}

		public static void CallInfo() {
			if(GlobalVariables.AnimeInternal.Count < 0) return;
			_fullOutput = string.Empty;
			StartProcess("info", true);
			StringParser.ParseOuput(_fullOutput);
		}
	}
}