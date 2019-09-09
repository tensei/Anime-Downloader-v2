using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AnimeDownloader.Common;
using AnimeDownloader.Helpers;
using AnimeDownloader.Helpers.Torrent_Clients;
using AnimeDownloader.Models;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace AnimeDownloader.ViewModels {
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		public static MainWindowViewModel Instance;
		public static int OrderId;

		public MainWindowViewModel() {
			Instance = this;
		    MessageQueue = new SnackbarMessageQueue();
			Anime = new ReadOnlyObservableCollection<AnimeInfoModel>(GlobalVariables.AnimeInternal);
			SettingsCommand = new ActionCommand(ShowSettings);
			NyaaCommand = new ActionCommand(ShowNyaa);
			FolderCommand = new ActionCommand(ShowFolder);
			OrderCommand = new ActionCommand(ReOrder);
			CloseCommand = new ActionCommand(() => { Application.Current.Shutdown(); });
			StartStopCommand = new ActionCommand(PausPlay);
			RefreshCommand = new ActionCommand(() => SleeperZ = 3);
			//debug test
			//var x = RssFeedHelper.GetFeedItems("http://www.nyaa.se/?page=rss&cats=1_37");
			//x.Result.ForEach(a => GlobalVariables.AnimeInternal.Add(a));
			SaveAnimeListHelper.Load();
			//Task.Run(async () => {
			//	var y = await RssFeedHelper.GetFeedItemsToDownload("http://www.nyaa.se/?page=rss&cats=1_37");
			//	await Application.Current.Dispatcher.BeginInvoke(new Action(() => {
			//		GlobalVariables.RssFeedInternal.Clear();
			//		y.ForEach(a => GlobalVariables.RssFeedInternal.Add(a));
			//		;
			//	}));
			//});
			//FindDefaultTorrentClientHelper.Find();
			ProcessHelper.LookForClient();
			BalloonHelper.Show("Starting in 5 seconds...");
				//await FolderBuilder.BuildTask();
				//await Task.Delay(1000);
			Start();
            Task.Run(async () =>
            {
                while (true)
                {
                    await QbitTorrentHelper.GetTorrentd();
                    await Task.Delay(5000);
                }
                // ReSharper disable once FunctionNeverReturns
            }).ConfigureAwait(false);
            //var array1 =new[] { "f", "g", "h" };
            //var array2 =new[] { "f", "g", "h", "j" };
            //MessageBox.Show(array1.SequenceEqual(array2).ToString());
        }

		public ICommand SettingsCommand { get; }
		public ICommand NyaaCommand { get; }
		public ICommand FolderCommand { get; }
		public ICommand OrderCommand { get; }
		public ICommand CloseCommand { get; }
		public ICommand StartStopCommand { get; }
		public ReadOnlyObservableCollection<AnimeInfoModel> Anime { get; set; }
		public int TransitionerIndex { get; set; }

		public AnimeInfoModel SelectedAnime { get; set; }
		public int SleeperZ { get; set; } = 3;

		public string OrderedText { get; set; } = GlobalVariables.Order[OrderId];

		public PackIconKind PausePlayButtonIcon { get; set; } = PackIconKind.Pause;

		public ICommand RefreshCommand { get; }

		public SnackbarMessageQueue MessageQueue { get; set; }

		private void PausPlay() {
			PausePlayButtonIcon = PausePlayButtonIcon == PackIconKind.Play ? PackIconKind.Pause : PackIconKind.Play;
		}

		private void ShowSettings() {
			if (!TransitionerIndex.Equals(2)) {
				TransitionerIndex = 2;
				return;
			}
			TransitionerIndex = 0;
		}

		private void ShowFolder() {
			if (!TransitionerIndex.Equals(3)) {
				TransitionerIndex = 3;
				return;
			}
			TransitionerIndex = 0;
		}

		private void ShowNyaa() {
			if (!TransitionerIndex.Equals(1)) {
				TransitionerIndex = 1;
				var rssvm = RssFeedViewModel.Instance;
				var secago = DateTime.Now - rssvm.LastRefresh;
				if (secago.TotalSeconds < 10) return;
				GlobalVariables.RssFeedInternal.Clear();
				rssvm.ProgressBarVisibility = Visibility.Visible;
				Task.Run(async () => {
					var y =
						await
							RssFeedHelper.GetFeedItemsToDownload($"https://nyaa.si/?page=rss&c=1_2&f=2&q={rssvm.Filter}");
					await Application.Current.Dispatcher.BeginInvoke(new Action(() => {
						if (y == null) {
							rssvm.ProgressBarVisibility = Visibility.Collapsed;
							return;
						}
						y.ForEach(a => GlobalVariables.RssFeedInternal.Add(a));
						rssvm.ProgressBarVisibility = Visibility.Collapsed;
						rssvm.LastRefresh = DateTime.Now;
					}));
				});
				return;
			}
			TransitionerIndex = 0;
		}

		private static async void ReOrder() {
			await Task.Run(async () => { OrderId = await ListOrderHelper.OrderToggle(OrderId); });
		}

		private async void Start() {
		    try
		    {
				var sites = new List<string> {
				    Settings.Config.Rss,
                    "https://nyaa.si/?page=rss&c=1_2&f=2",
				};
				while (true)
					if (SleeperZ >= 1) {
						await Task.Delay(1000);
						if (PausePlayButtonIcon != PackIconKind.Pause) continue;
						SleeperZ--;
					} else if (SleeperZ <= 1) {
						await FolderBuilder.BuildTask();
						//if (!ProcessHelper.LookForClient()) {
						//	SleeperZ = Settings.Config.RefreshTime;
						//	MessageQueue.Enqueue("Deluge not open... please start Deluge", true);
						//	continue;
						//}
                        MessageQueue.Enqueue($"Checking {Settings.Config.Rss}");
					    var rssItems = new List<RssFeedItemModel>();
					    foreach (var site in sites)
					    {
                            var y = await RssFeedHelper.GetFeedItemsToDownload(site, false);
					        if (y == null)
					        {
                                continue;
					        }
					        rssItems.AddRange(y);
					    }
                        MessageQueue.Enqueue($"{rssItems.Count} Entries in RSS feed");
						if (rssItems.Count <= 0) {
							SleeperZ = Settings.Config.RefreshTime;
							continue;
						}
						foreach (var folderModel in GlobalVariables.FolderPaths)
							foreach (var rssFeedItemModel in rssItems) {
								var arrayequal = folderModel.EpisodeArray.SequenceEqual(rssFeedItemModel.NameArray);
								if (GlobalVariables.AllFiles.Contains(rssFeedItemModel.Name)) continue;
								if (arrayequal) {
									ConvertHelper.RssModelToList(rssFeedItemModel, folderModel.FolderPath);
									//DelugeHelper.Add(rssFeedItemModel.DownloadLink, folderModel.FolderPath);
									QbitTorrentHelper.Add(rssFeedItemModel.DownloadLink, folderModel.FolderPath);
                                    GlobalVariables.AllFiles.Add(rssFeedItemModel.Name);
									BalloonHelper.Show($"Added {rssFeedItemModel.Name}");
									MessageQueue.Enqueue($"Added {rssFeedItemModel.Name}", true);
									await Task.Delay(500);
									break;
								}

							    if (folderModel.Episode.Contains("Chyuu") && rssFeedItemModel.Name.Contains("Chyuu") && folderModel.Episode.Contains("Fate"))
							    {
							        Debug.WriteLine("folder ep: {0} rss ep: {1}", folderModel.Episode, rssFeedItemModel.Name);
							    }
								if (folderModel.EpisodeArray.Length != rssFeedItemModel.NameArray.Length) continue;
								var diff = new List<string>();
								var diffpoistions = new List<int>();
								for (var index = 0; index < folderModel.EpisodeArray.Length; index++) {
									var s = folderModel.EpisodeArray[index];
									var e = rssFeedItemModel.NameArray[index];
									if (s == e) continue;
									diff.Add(rssFeedItemModel.NameArray[index]);
									diffpoistions.Add(index);
								}
								if (diff.Count == 1 && diff[0].Contains("[") &&
								    !GlobalVariables.Resolutions.Any(diff[0].Contains) &&
								    diffpoistions[0] != 0 || diff.Count == 0) {
									ConvertHelper.RssModelToList(rssFeedItemModel, folderModel.FolderPath);
									//DelugeHelper.Add(rssFeedItemModel.DownloadLink, folderModel.FolderPath);
									QbitTorrentHelper.Add(rssFeedItemModel.DownloadLink, folderModel.FolderPath);
                                    GlobalVariables.AllFiles.Add(rssFeedItemModel.Name);
									BalloonHelper.Show($"Added {rssFeedItemModel.Name}");
									MessageQueue.Enqueue($"Added {rssFeedItemModel.Name}", true);
									await Task.Delay(500);
								}
							}
						SleeperZ = Settings.Config.RefreshTime;
						SaveAnimeListHelper.Save();
		            }
		    }
		    catch (Exception e)
		    {
		        Console.WriteLine(e);
		    }
        }

	    public event PropertyChangedEventHandler PropertyChanged;
	}
}