using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using AnimeDownloader.Common;
using AnimeDownloader.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AnimeDownloader.Helpers {
	public static class SaveAnimeListHelper {
		private static readonly string SavedList = Path.Combine(Environment.GetFolderPath(
			Environment.SpecialFolder.ApplicationData), "AnimeDownloader.json");

		public static void Save() {
			if (GlobalVariables.AnimeInternal.Count < 0) return;
			var output = JsonConvert.SerializeObject(GlobalVariables.AnimeInternal, Formatting.Indented,
				new StringEnumConverter {CamelCaseText = true});

			try {
				File.WriteAllText(SavedList, output);
			} catch (Exception) {
				MessageBox.Show("error saving list");
			}
		}

		public static void Load() {
			if (!File.Exists(SavedList)) return;
			var input = File.ReadAllText(SavedList);

			var jsonSettings = new JsonSerializerSettings();
			jsonSettings.Converters.Add(new StringEnumConverter {CamelCaseText = true});
			jsonSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
			jsonSettings.DefaultValueHandling = DefaultValueHandling.Populate;
			var list = JsonConvert.DeserializeObject<ObservableCollection<AnimeInfoModel>>(input, jsonSettings);
			foreach (var animeInfoModel in list) GlobalVariables.AnimeInternal.Add(animeInfoModel);
			foreach (var animeInfoModel in list) {
				if (!GlobalVariables.AllFiles.Contains(animeInfoModel.Name)) {
					GlobalVariables.AllFiles.Add(animeInfoModel.Name);
				}
			}
		}
	}
}