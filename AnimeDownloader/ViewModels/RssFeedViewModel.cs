using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AnimeDownloader.Common;
using AnimeDownloader.Helpers;
using AnimeDownloader.Models;
using PropertyChanged;
using System.Windows.Forms;
using System.IO;
using Application = System.Windows.Application;

namespace AnimeDownloader.ViewModels {
	public class RssFeedViewModel : INotifyPropertyChanged{
		public static RssFeedViewModel Instance;

		public RssFeedViewModel() {
			Instance = this;
			RssFeed = new ReadOnlyObservableCollection<RssFeedItemModel>(GlobalVariables.RssFeedInternal);
			DeselectCommand = new ActionCommand(() => { SelectedAnime = null; });
			RefreshCommand = new ActionCommand(() => Refresh());
			SearchCommand = new ActionCommand(() => Refresh());
			SavePathCommand = new ActionCommand(SelectSavePath);
			ClearFilterCommand = new ActionCommand(() => {
				if (Filter == string.Empty) { return; }
				Filter = string.Empty;
				Refresh();
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
		public ICommand SavePathCommand { get; }

		public async void Refresh() {
			ProgressBarVisibility = Visibility.Visible;
			GlobalVariables.RssFeedInternal.Clear();
			await Task.Run(async () =>
            {
                var y = await RssFeedHelper.GetFeedItemsToDownload($"https://nyaa.si/?page=rss&c=1_2&f=0&q={Filter}");
                if (Application.Current.Dispatcher != null)
                {
                    await
                        Application.Current.Dispatcher.BeginInvoke(
                            new Action(() =>
                            {
                                y?.ForEach(a => GlobalVariables.RssFeedInternal.Add(a));
                                ProgressBarVisibility = Visibility.Collapsed;
                                LastRefresh = DateTime.Now;
                            }));
                }
            });
		}
		private void SelectSavePath() {
            using (var dialog = new FolderBrowserDialog())
            {
                var res = dialog.ShowDialog();
			    if (res != DialogResult.OK) return;
			    if (Directory.Exists(dialog.SelectedPath)) {
				    SelectedAnime.SavePath = dialog.SelectedPath;
			    }
            }
		}

	    public event PropertyChangedEventHandler PropertyChanged;
	}
}