using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AnimeDownloader.Common;
using AnimeDownloader.Helpers;
using AnimeDownloader.ViewModels;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using PropertyChanged;

namespace AnimeDownloader.Models {
	[ImplementPropertyChanged]
	public class AnimeInfoModel {
		public AnimeInfoModel() {
			DoubleClickCommand = new ActionCommand(DoubleClick);
			ToggleWatchCommand = new ActionCommand(ToggleWatch);
			DeleteFileCommand = new ActionCommand(DeleteFile);
			DeleteEntryCommand = new ActionCommand(DeleteEntry);
		}

		public string Name { get; set; }
		public string DelugeID { get; set; }
		public PackIconKind Status { get; set; } = PackIconKind.EyeOff;

		[JsonIgnore]
		public string Added => DaysSinceUpdate();

		public DateTime Created { get; set; }
		public string FileLocation { get; set; }
		public string FolderLocation { get; set; }
		public float Maximum { get; set; }
		public float CurrentProgress { get; set; }
		public PackIconKind Downloaded { get; set; } = PackIconKind.Download;

		[JsonIgnore]
		public ICommand DoubleClickCommand { get; }

		[JsonIgnore]
		public ICommand ToggleWatchCommand { get; }

		[JsonIgnore]
		public ICommand DeleteFileCommand { get; }

		[JsonIgnore]
		public ICommand DeleteEntryCommand { get; }

		public Visibility ProgressVisibility { get; set; }

		private string DaysSinceUpdate() {
			var dateNow = DateTime.Now;
			var diff = dateNow - Created;
			if (diff.Days < 0)
				return "Unknown";
			return diff.Days == 0 ? "Today" : $"{diff.Days} day(s) ago";
		}

		private void DoubleClick() {
			Status = PackIconKind.Eye;
			if (FileLocation == string.Empty) return;
			if (!File.Exists(FileLocation)) {
				Process.Start(FolderLocation);
				return;
			}
			Process.Start(FileLocation);
		}

		private async void ToggleWatch() {
			Status = Status == PackIconKind.EyeOff ? PackIconKind.Eye : PackIconKind.EyeOff;
			SaveAnimeListHelper.Save();
			if (MainWindowViewModel.OrderId == 2)
				await Task.Run(async () => { await ListOrderHelper.ReOrder(); });
		}

		private bool CheckForFile() {
			return (FileLocation != string.Empty) && File.Exists(FileLocation);
		}


		private void DeleteFile() {
			if (!CheckForFile()) return;
			File.Delete(FileLocation);
			DeleteEntry();
			SaveAnimeListHelper.Save();
		}

		private void DeleteEntry() {
			GlobalVariables.AnimeInternal.Remove(this);
			SaveAnimeListHelper.Save();
		}
	}
}