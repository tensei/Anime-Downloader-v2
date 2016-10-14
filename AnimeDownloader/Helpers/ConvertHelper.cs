using System;
using System.IO;
using System.Windows;
using AnimeDownloader.Common;
using AnimeDownloader.Models;

namespace AnimeDownloader.Helpers {
	public static class ConvertHelper {
		public static void RssModelToList(RssFeedItemModel model, string folder) {
			var animemodel = new AnimeInfoModel {
				Name = model.Name,
				Created = model.Released,
				FileLocation = Path.Combine(folder, model.Name),
				FolderLocation = folder
			};
			Application.Current.Dispatcher.BeginInvoke(new Action(() => GlobalVariables.AnimeInternal.Add(animemodel)));
		}
	}
}