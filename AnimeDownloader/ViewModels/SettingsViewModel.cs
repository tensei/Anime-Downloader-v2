using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using AnimeDownloader.Common;
using Microsoft.Win32;
using PropertyChanged;
using System.Windows;
using System.Windows.Forms;

namespace AnimeDownloader.ViewModels {
	public class SettingsViewModel : INotifyPropertyChanged{
		public SettingsViewModel() {
			AddGroupCommand = new ActionCommand(AddGroup);
			RemoveGroupCommand = new ActionCommand(RemoveGroup);
			SearchOngoingCommand = new ActionCommand(() => {Search("ongoing");});
			SearchClientCommand = new ActionCommand((() => {Search("");}));
			Load();
		}
        

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
			    Config.OngoingFolder = dialog.SelectedPath;
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
			    Config.TorrentClient = dialogfile.FileName;
			}
		}

	    public Config Config { get; set; }
		private void Load()
		{
		    Config = Settings.Config;
		}
        
	    public event PropertyChangedEventHandler PropertyChanged;
	}
}