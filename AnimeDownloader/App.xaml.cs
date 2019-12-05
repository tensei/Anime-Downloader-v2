using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using AnimeDownloader.Common;
using AnimeDownloader.ViewModels;

namespace AnimeDownloader {
	/// <summary>
	///     Interaktionslogik für "App.xaml"
	/// </summary>
	public partial class App : Application {
		private void AppStartup(object sender, StartupEventArgs args) {
			if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1) {
#if !DEBUG
                MessageBox.Show(
					AppDomain.CurrentDomain.FriendlyName + " is already running. Application will now close.",
					"Application running!", MessageBoxButton.OK, MessageBoxImage.Stop);
				Current.Shutdown();
#endif
			} else {
				Settings.Load();
				//Waiting for the settings to be loaded
				Thread.Sleep(1000);
				var mainWindow = new MainWindow {
					DataContext = new MainWindowViewModel()
				};
				mainWindow.Show();
			}
		}
	}
}