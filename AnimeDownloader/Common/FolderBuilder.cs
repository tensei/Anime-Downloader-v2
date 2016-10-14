using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AnimeDownloader.Models;

namespace AnimeDownloader.Common {
	public static class FolderBuilder {
		public static async Task BuildTask() {
			GlobalVariables.AllFiles.Clear();
			GlobalVariables.FolderPaths.Clear();
			await Task.Run(() => {
				var subfolders = Directory.GetDirectories(Settings.Config.OngoingFolder);
				foreach (var folder in subfolders) {
					var filenames = Directory.GetFiles(folder).Select(Path.GetFileName).ToArray();
					if (filenames.Length <= 0) continue;
					GlobalVariables.AllFiles.AddRange(filenames);
					string[] lastfile = {"lul"};
					foreach (var file in filenames) {
						var splitfile = SplitName(file);
						if (splitfile.SequenceEqual(lastfile)) continue;
						GlobalVariables.FolderPaths.Add(new FolderModel {
							EpisodeArray = splitfile,
							Episode = file,
							FolderPath = folder
						});
						lastfile = splitfile;
					}
				}
			});
		}

		public static string[] SplitName(string name) {
			var splitter = name.Contains(" ") ? ' ' : '_';
			var splitname = name.Split(splitter).ToList();
			var toremove = (from s in splitname let regex = Regex.Match(s, "^[0-9]+v?[0-9]?$") where regex.Success select s).ToList();
			toremove.ForEach(x => splitname.Remove(x));
			splitname = splitname.Select(x => x.Replace(".mkv", string.Empty)).ToList();
			splitname = splitname.Select(x => x.Replace(".mp4", string.Empty)).ToList();
			 //splitname.ForEach(x => Regex.Replace(x, @"[\.mkv|\.mp4]", string.Empty));
			return splitname.ToArray();
		}
	}
}