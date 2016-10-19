using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AnimeDownloader.Common;
using AnimeDownloader.Helpers;
using AnimeDownloader.Models;
using PropertyChanged;

namespace AnimeDownloader.ViewModels {
	[ImplementPropertyChanged]
	public class RssFeedViewModel {
		public static RssFeedViewModel Instance;
		private readonly Dictionary<string, string> _quality = GlobalVariables.Quality;

		public RssFeedViewModel() {
			Instance = this;
			RssFeed = new ReadOnlyObservableCollection<RssFeedItemModel>(GlobalVariables.RssFeedInternal);
			DeselectCommand = new ActionCommand(() => { SelectedAnime = null; });
			RefreshCommand = new ActionCommand(() => Refresh());
			SearchCommand = new ActionCommand(() => Refresh(true));
			ClearFilterCommand = new ActionCommand(() => {
				if (Filter == string.Empty) { return; }
				Filter = string.Empty;
				Refresh(true);
			});
			OpenSiteCommand = new ActionCommand(() => {
				Process.Start(SelectedAnime.Link);
			});
		}

		public ReadOnlyObservableCollection<RssFeedItemModel> RssFeed { get; }

		public RssFeedItemModel SelectedAnime { get; set; }

		public ICommand DeselectCommand { get; }
		public ICommand RefreshCommand { get; }

		public int SelectedQualityIndex { get; set; }
		public string SelectedQuality { get; set; }

		public string Filter { get; set; } = string.Empty;

		public Visibility ProgressBarVisibility { get; set; } = Visibility.Visible;

		public DateTime LastRefresh { get; set; }

		public ICommand ClearFilterCommand { get; }

		public ICommand SearchCommand { get; }

		public ICommand OpenSiteCommand { get; }

		public async void Refresh(bool force = false) {
			var minago = DateTime.Now - LastRefresh;
			if (minago.TotalSeconds < 10 && !force) return;
			ProgressBarVisibility = Visibility.Visible;
			GlobalVariables.RssFeedInternal.Clear();
			await Task.Run(async () => {
				var y = await RssFeedHelper.GetFeedItemsToDownload(_quality[SelectedQuality] + Filter);
				await
					Application.Current.Dispatcher.BeginInvoke(
						new Action(() => {
							y.ForEach(a => GlobalVariables.RssFeedInternal.Add(a));
							ProgressBarVisibility = Visibility.Collapsed;
							LastRefresh = DateTime.Now;
						}));
			});
		}
	}
}