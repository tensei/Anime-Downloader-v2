using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AnimeDownloader.Models;

namespace AnimeDownloader.Common {
	public static class FolderBuilder {
		public static async Task BuildTask() {
			//GlobalVariables.AllFiles.Clear();
			GlobalVariables.FolderPaths.Clear();
			await Task.Run(() => {
				var subfolders = Directory.GetDirectories(Settings.Config.OngoingFolder);
				foreach (var folder in subfolders) {
					var filenames = Directory.GetFiles(folder).Select(Path.GetFileName).ToArray();
					if (filenames.Length <= 0) continue;
					foreach (var filename in filenames) {
						if (!GlobalVariables.AllFiles.Contains(filename)) {
							GlobalVariables.AllFiles.Add(filename);
						}
					}
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
		    var splitter = Regex.Replace(name, "[^\\[\\]0-9a-zA-Z]+", " ").Trim();

            var splitname = splitter.Split(' ').ToList();
            
            var toremove =
				(from s in splitname let regex = Regex.Match(s, "^[0-9]+v?[0-9]?$") where regex.Success select s).ToList
					();
			toremove.ForEach(x => splitname.Remove(x));
			splitname = splitname.Select(x => x.Replace(".mkv", string.Empty)).ToList();
			splitname = splitname.Select(x => x.Replace(".mp4", string.Empty)).ToList();
			//splitname.ForEach(x => Regex.Replace(x, @"[\.mkv|\.mp4]", string.Empty));
		    splitname = splitname.Select(x =>
		    {
		        if (x.Contains("]["))
		        {
		            x = x.Split(']').Last() +"]";
		        }
                return x;
		    }).ToList();
			return splitname.ToArray();
		}
	}
}