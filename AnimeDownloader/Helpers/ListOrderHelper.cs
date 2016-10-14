using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AnimeDownloader.Common;
using AnimeDownloader.Models;
using AnimeDownloader.ViewModels;

namespace AnimeDownloader.Helpers {
	public class ListOrderHelper {
		public static async Task<int> OrderToggle(int OrderId) {
			switch (OrderId) {
				case 0: {
					OrderId++;
					await OrderByName(OrderId);
					break;
				}
				case 1: {
					OrderId++;
					await OrderByStatus(OrderId);
					break;
				}
				case 2: {
					OrderId = 0;
					await OrderByCreated(OrderId);
					break;
				}
			}
			return OrderId;
		}

		private static async Task OrderByStatus(int OrderId) {
			await Task.Run(async () => {
				await Task.Delay(250);
				GlobalVariables.AnimeInternal =
					new ObservableCollection<AnimeInfoModel>(GlobalVariables.AnimeInternal.OrderBy(x => x.Status)
						.ThenBy(x => x.Created));
				MainWindowViewModel.Instance.Anime = new ReadOnlyObservableCollection<AnimeInfoModel>(GlobalVariables.AnimeInternal);
				MainWindowViewModel.Instance.OrderedText = GlobalVariables.Order[OrderId];
			});
		}

		private static async Task OrderByName(int OrderId) {
			await Task.Run(async () => {
				await Task.Delay(250);
				GlobalVariables.AnimeInternal =
					new ObservableCollection<AnimeInfoModel>(GlobalVariables.AnimeInternal.OrderBy(x => x.Name));
				MainWindowViewModel.Instance.Anime = new ReadOnlyObservableCollection<AnimeInfoModel>(GlobalVariables.AnimeInternal);
				MainWindowViewModel.Instance.OrderedText = GlobalVariables.Order[OrderId];
			});
		}

		private static async Task OrderByCreated(int OrderId) {
			await Task.Run(async () => {
				await Task.Delay(250);
				GlobalVariables.AnimeInternal =
					new ObservableCollection<AnimeInfoModel>(GlobalVariables.AnimeInternal.OrderBy(x => x.Created));
				MainWindowViewModel.Instance.Anime = new ReadOnlyObservableCollection<AnimeInfoModel>(GlobalVariables.AnimeInternal);
				MainWindowViewModel.Instance.OrderedText = GlobalVariables.Order[OrderId];
			});
		}

		public static async Task ReOrder() {
			var OrderId = MainWindowViewModel.OrderId;
			switch (OrderId) {
				case 0: {
					await OrderByCreated(OrderId);
					break;
				}
				case 1: {
					await OrderByName(OrderId);
					break;
				}
				case 2: {
					await OrderByStatus(OrderId);
					break;
				}
			}
		}
	}
}