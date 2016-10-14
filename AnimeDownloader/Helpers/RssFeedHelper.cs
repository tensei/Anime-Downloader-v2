using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using AnimeDownloader.Common;
using AnimeDownloader.Models;
using CloudFlareUtilities;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MaterialDesignThemes.Wpf;
using RestSharp;

namespace AnimeDownloader.Helpers {
	public static class RssFeedHelper {
		private static HttpClient _cloudflareClient;

		private static ProgressDialogController _controller;

		public static async Task<List<AnimeInfoModel>> GetFeedItems(string url) {
			var feed = await GetRssContent(url);
			return feed?.Items.Select(item => new AnimeInfoModel {
				Created = item.PublishDate.DateTime,
				Name = item.Title.Text,
				Status = PackIconKind.EyeOff
			}).ToList();
		}

		public static async Task<List<RssFeedItemModel>> GetFeedItemsToDownload(string url) {
			var feed = await GetRssContent(url);
			return feed?.Items.Select(item => new RssFeedItemModel {
				Released = item.PublishDate.DateTime,
				Name = item.Title.Text,
				Description = item.Summary.Text,
				Quality = NyaaHelper.GetQuality(item.Summary.Text),
				Link = item.Links[0].Uri.AbsoluteUri,
				SavePath = StringParser.SuggestedFolderName(item.Title.Text),
				NameArray = FolderBuilder.SplitName(item.Title.Text)
			}).ToList();
		}

		private static async Task<SyndicationFeed> GetRssContent(string url) {
			try {
				var client = new RestClient(url) {Encoding = Encoding.UTF8};
				var request = new RestRequest();
				var content = await client.ExecuteTaskAsync(request);
				var xmlr = XmlReader.Create(new StringReader(content.Content));
				var feed = SyndicationFeed.Load(xmlr);
				_cloudflareClient = null;
				return feed;
			} catch (Exception) {
				//ShowDialog();
				var cfContent = await HandleCloudflare(url);
				if (cfContent == null) return null;
				var xmlr = XmlReader.Create(new StringReader(cfContent));
				var feed = SyndicationFeed.Load(xmlr);
				if (_controller != null) await _controller.CloseAsync();
				return feed;
			}
		}

		private static async Task<string> HandleCloudflare(string url) {
			try {
				//if (_cloudflareClient != null) return await _cloudflareClient.GetStringAsync(url);
				// Create the clearance handler.
				var handler = new ClearanceHandler();

				// Create a HttpClient that uses the handler.
				_cloudflareClient = new HttpClient(handler);

				// Use the HttpClient as usual. Any JS challenge will be solved automatically for you.
				var content = await _cloudflareClient.GetStringAsync(url);
				return content;

			} catch (Exception) {
				return null;
			}
		}

		private static async void ShowDialog() {
			await Application.Current.Dispatcher.BeginInvoke(new Action(async () => {
				var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
				_controller = await window.ShowProgressAsync("Nyaa.se", "Detected Cloudflare\n" +
				                                                        "trying to solve challenge");
				_controller.SetIndeterminate();
				_controller.SetCancelable(true);
				_controller.Canceled += (sender, args) => { _controller.CloseAsync(); };
				_controller.Closed += (sender, args) => { _controller = null; };
			}));
		}
	}
}