using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;

namespace AnimeDownloader.Common {
	public class StringParser {
		public static string SuggestedFolderName(string title) {
			var suggested = Regex.Match(title, @".+\] ?([a0-z9\W']+) ?\[?- ?[vol.]?\d{0,3}?", RegexOptions.IgnoreCase);
			if (suggested.Success) return suggested.Groups[1].Value.Trim();

			suggested = Regex.Match(title, @".+\] ([a0-z9\W']+) \d{0,3}$", RegexOptions.IgnoreCase);
			if (suggested.Success) return suggested.Groups[1].Value.Trim();

			suggested = Regex.Match(title, @"^([a0-z9\W']+) \[|\(", RegexOptions.IgnoreCase);
			if (suggested.Success) return suggested.Groups[1].Value.Trim();

			suggested = Regex.Match(title, @"^\[.+\] (.+)-.+", RegexOptions.IgnoreCase);
			if (suggested.Success) return suggested.Groups[1].Value.Trim();

			return "ReplaceMe";
		}

		public static async Task ParseOuput(string output) {
			await Task.Run(() => {
				var matches = Regex.Matches(output,
					@"Name: (?<Name>.+)\nID: (?<ID>.+)\n?(State: )?(?<State>.+)?\n?.+?\n(Seeds: )?(?<Seeds>.+)?\n?Size: (?<SizeCurrent>\d{0,9}.?\d+?)  ?([a-z]+)? ?\/ ?(?<SizeMax>\d+.?\d+?) ?([a-z]+)? ?Ratio: (?<Ratio>-?\d+\.?\d{0,9})",
					RegexOptions.IgnoreCase);
				foreach (Match match in matches)
					foreach (var animeInfoModel in GlobalVariables.AnimeInternal) {
						if (match.Groups["Name"].Value != animeInfoModel.Name || animeInfoModel.Downloaded == PackIconKind.Check) continue;
						//MessageBox.Show(match.Groups["Name"].Value);
						//if (match.Groups["SizeCurrent"].Value != match.Groups["SizeMax"].Value) continue;
						animeInfoModel.DelugeID = match.Groups["ID"].Value;
						var state = match.Groups["State"].Value;
						if (state.Contains("Seeding")) {
							animeInfoModel.Downloaded = PackIconKind.Upload;
							animeInfoModel.ProgressVisibility = Visibility.Collapsed;
						}
						if (state.Contains("Pause")) animeInfoModel.Downloaded = PackIconKind.Check;
						animeInfoModel.ProgressVisibility = match.Groups["SizeCurrent"].Value == match.Groups["SizeMax"].Value
							? Visibility.Collapsed
							: Visibility.Visible;

						animeInfoModel.CurrentProgress = float.Parse(match.Groups["SizeCurrent"].Value, CultureInfo.InvariantCulture);
						animeInfoModel.Maximum = float.Parse(match.Groups["SizeMax"].Value, CultureInfo.InvariantCulture);
					}
			});
			/*
			Name: The.Jungle.Book.2016.1080p.BluRay.DTS.x264-HDMaNiAcS
			ID: 6f8d46eb9f7ed727b61f2d5154fb5afeb4583c66
			State: Seeding Up Speed: 0.0 KiB/s
			Seeds: 0 (546) Peers: 1 (1) Availability: 0.00
			Size: 12.7 GiB/12.7 GiB Ratio: 0.007
			*/
		}
	}
}