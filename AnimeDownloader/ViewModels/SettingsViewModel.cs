using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AnimeDownloader.Common;
using Microsoft.Win32;
using PropertyChanged;
using System.Windows;
using System.Windows.Forms;

namespace AnimeDownloader.ViewModels {
	[ImplementPropertyChanged]
	public class SettingsViewModel {
		public SettingsViewModel() {
			SaveCommand = new ActionCommand(Save);
			AddGroupCommand = new ActionCommand(AddGroup);
			RemoveGroupCommand = new ActionCommand(RemoveGroup);
			SearchOngoingCommand = new ActionCommand(() => {Search("ongoing");});
			SearchClientCommand = new ActionCommand((() => {Search("");}));
			Load();
		}

		public string Rss { get; set; }

		public string TorrentClient { get; set; }

		public string OngoingFolder { get; set; }

		public string TorrentFiles { get; set; }

		public string Resolution { get; set; }

		public int RefreshTime { get; set; }

		public string GroupBoxText { get; set; }

		public ObservableCollection<string> Groups { get; set; }

		public ICommand SaveCommand { get; }
		public ICommand AddGroupCommand { get; }
		public ICommand RemoveGroupCommand { get; }

		public ICommand SearchClientCommand { get; }

		public ICommand SearchOngoingCommand { get; }

		private void AddGroup() {
			if (Groups.Contains(GroupBoxText)) return;
			Groups.Add(GroupBoxText);
		}

		private void RemoveGroup() {
			if (!Groups.Contains(GroupBoxText)) return;
			Groups.Remove(GroupBoxText);
			GroupBoxText = Groups.FirstOrDefault();
		}

		private void Search(string folder) {
			if (folder == "ongoing") {
				var dialog = new FolderBrowserDialog();
				var result = dialog.ShowDialog();
				if (result != DialogResult.OK) return;
				OngoingFolder = dialog.SelectedPath;
				return;
			}
			var dialogfile = new System.Windows.Forms.OpenFileDialog {
				AddExtension = true,
				Filter = "deluge-console.exe|*.exe",
				FileName = "deluge-console.exe",
				CheckFileExists = true,
				CheckPathExists = true
			};
			var res = dialogfile.ShowDialog();
			if (res != DialogResult.OK) return;
			if (dialogfile.FileName.EndsWith("deluge-console.exe")) {
				TorrentClient = dialogfile.FileName;
			}
		}

		private void Load() {
			TorrentClient = Settings.Config.TorrentClient;
			OngoingFolder = Settings.Config.OngoingFolder;
			//TorrentFiles = Settings.Config.TorrentFiles;
			//Resolution = Settings.Config.Resolution;
			RefreshTime = Settings.Config.RefreshTime;
			//Groups = new ObservableCollection<string>(Settings.Config.Groups);
			//GroupBoxText = Settings.Config.Groups.FirstOrDefault();
			Rss = Settings.Config.Rss;
		}

		private void Save() {
			Settings.Config.Rss = Rss;
			Settings.Config.TorrentClient = TorrentClient;
			Settings.Config.OngoingFolder = OngoingFolder;
			//Settings.Config.TorrentFiles = TorrentFiles;
			//Settings.Config.Resolution = Resolution;
			Settings.Config.RefreshTime = RefreshTime;
			//Settings.Config.Groups = Groups.ToList();
			Settings.Save();
		}
	}
}