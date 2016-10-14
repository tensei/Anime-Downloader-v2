using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AnimeDownloader.Common;
using PropertyChanged;

namespace AnimeDownloader.ViewModels {
	[ImplementPropertyChanged]
	public class SettingsViewModel {
		public SettingsViewModel() {
			SaveCommand = new ActionCommand(Save);
			AddGroupCommand = new ActionCommand(AddGroup);
			RemoveGroupCommand = new ActionCommand(RemoveGroup);
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

		private void AddGroup() {
			if (Groups.Contains(GroupBoxText)) return;
			Groups.Add(GroupBoxText);
		}

		private void RemoveGroup() {
			if (!Groups.Contains(GroupBoxText)) return;
			Groups.Remove(GroupBoxText);
			GroupBoxText = Groups.FirstOrDefault();
		}


		private void Load() {
			TorrentClient = Settings.Config.TorrentClient;
			OngoingFolder = Settings.Config.OngoingFolder;
			TorrentFiles = Settings.Config.TorrentFiles;
			Resolution = Settings.Config.Resolution;
			RefreshTime = Settings.Config.RefreshTime;
			Groups = new ObservableCollection<string>(Settings.Config.Groups);
			GroupBoxText = Settings.Config.Groups.FirstOrDefault();
			Rss = Settings.Config.Rss;
		}

		private void Save() {
			Settings.Config.Rss = Rss;
			Settings.Config.TorrentClient = TorrentClient;
			Settings.Config.OngoingFolder = OngoingFolder;
			Settings.Config.TorrentFiles = TorrentFiles;
			Settings.Config.Resolution = Resolution;
			Settings.Config.RefreshTime = RefreshTime;
			Settings.Config.Groups = Groups.ToList();
			Settings.Save();
		}
	}
}