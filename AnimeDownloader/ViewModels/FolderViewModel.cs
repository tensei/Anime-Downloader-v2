using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using AnimeDownloader.Common;

namespace AnimeDownloader.ViewModels {
	public class FolderViewModel {
		private readonly ObservableCollection<TreeViewItem> _folderTrees = new ObservableCollection<TreeViewItem>();

		public FolderViewModel() {
			FolderTrees = new ReadOnlyObservableCollection<TreeViewItem>(_folderTrees);
			Build();
		}

		public ReadOnlyObservableCollection<TreeViewItem> FolderTrees { get; }

		private void Build() {
			if (!Directory.Exists(Settings.Config.OngoingFolder)) return;
			_folderTrees.Clear();
			var folder = Directory.GetDirectories(Settings.Config.OngoingFolder);
			foreach (var f in folder) {
				var tree = new TreeViewItem {
					ItemsSource = Directory.GetFiles(f).Select(Path.GetFileName).ToArray(),
					Header = Path.GetFileName(f)
				};
				tree.MouseDoubleClick += (sender, args) => { Process.Start(f); };
				_folderTrees.Add(tree);
			}
		}
	}
}