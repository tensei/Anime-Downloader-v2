﻿using System;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AnimeDownloader.Common {
	public static class Settings {
		public static Config Config { get; set; }
		private static readonly string ConfigFile = Path.Combine(Directory.GetCurrentDirectory(), "AnimeDownloader.json");

		public static void Load() {
			if (File.Exists(ConfigFile)) {
				var input = File.ReadAllText(ConfigFile);

				var jsonSettings = new JsonSerializerSettings();
				jsonSettings.Converters.Add(new StringEnumConverter {CamelCaseText = true});
				jsonSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
				jsonSettings.DefaultValueHandling = DefaultValueHandling.Populate;
				Config = JsonConvert.DeserializeObject<Config>(input, jsonSettings);
			} else {
				Config = new Config();
			}
            Config.PropertyChanged += ConfigOnPropertyChanged;
			Save();
		}

	    private static void ConfigOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
	    {
            Save();
	    }

	    private static void Save() {
			var output = JsonConvert.SerializeObject(Config, Formatting.Indented,
				new StringEnumConverter {CamelCaseText = true});

			var folder = Path.GetDirectoryName(ConfigFile);
			if ((folder != null) && !Directory.Exists(folder)) Directory.CreateDirectory(folder);
			try {
				File.WriteAllText(ConfigFile, output);
			} catch (Exception) {
				//ignore
			}
		}
	}
}