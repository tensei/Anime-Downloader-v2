using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using AnimeDownloader.ViewModels;

namespace AnimeDownloader.Views {
	/// <summary>
	///     Interaktionslogik für RssFeedView.xaml
	/// </summary>
	public partial class RssFeedView : UserControl {
		public RssFeedView() {
			InitializeComponent();
			DataContext = new RssFeedViewModel();
		}

		private void UIElement_OnPreviewKeyDown(object sender, KeyEventArgs e) {
			if (e.Key != Key.Enter) return;
			var tBox = (TextBox) sender;
			var prop = TextBox.TextProperty;

			var binding = BindingOperations.GetBindingExpression(tBox, prop);
			binding?.UpdateSource();
			RssFeedViewModel.Instance.Refresh();
		}
	}
}