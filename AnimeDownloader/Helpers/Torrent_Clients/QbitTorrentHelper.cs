using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AnimeDownloader.Common;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;

namespace AnimeDownloader.Helpers.Torrent_Clients
{
    public class QbitTorrentHelper
    {
        private static readonly HttpClient Client = new HttpClient();

        public static async void Add(string link, string safeLocation)
        {
            await Login();
            var m = new MultipartFormDataContent(Guid.NewGuid().ToString())
            {
                {new StringContent(link), "urls"},
                {new StringContent(safeLocation), "savepath"},
                {new StringContent("Anime"), "category"}
            };
            var response = await Client.PostAsync(new Uri("http://localhost:1584/api/v2/torrents/add"), m);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        private static bool Logged { get; set; }

        private static async Task Login()
        {
            if (Logged)
            {
                return;
            }
            var datalogin = new Dictionary<string, string>
            {
                {"username", "admin"},
                {"password", "123456"},
            };
            Client.DefaultRequestHeaders.Referrer = new Uri("http://localhost:1584");
            var login = await Client.PostAsync("http://localhost:1584/api/v2/auth/login", new FormUrlEncodedContent(datalogin));
            Logged = login.IsSuccessStatusCode;
        }

        public static async Task GetTorrentd()
        {
            //query/torrents
            await Login();

            var response = await Client.GetStringAsync("http://localhost:1584/api/v2/torrents/info?category=Anime");
            try
            {
                var js = JsonConvert.DeserializeObject<List<Torrent>>(response);
                UpdateAnime(js);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void UpdateAnime(List<Torrent> torrents)
        {
            foreach (var torrent in torrents)
            {
                var anime = GlobalVariables.AnimeInternal.FirstOrDefault(a => a.Name == torrent.name);
                if (anime == null)
                {
                    continue;
                }
                
                anime.Maximum = torrent.size;
                anime.CurrentProgress = torrent.downloaded;
                anime.ProgressVisibility = torrent.state == "downloading" ? Visibility.Visible : Visibility.Collapsed;
                switch (torrent.state)
                {
                    case "downloading":
                    case "stalledDOWN":
                        anime.Downloaded = PackIconKind.ArrowDown;
                        break;
                    case "stalledUP":
                    case "uploading":
                        anime.Downloaded = PackIconKind.ArrowUp;
                        break;
                    default:
                        anime.Downloaded = PackIconKind.Check;
                        break;
                }
            }

        }
    }
    public class Torrent
    {
        public int added_on { get; set; }
        public int amount_left { get; set; }
        public string category { get; set; }
        public long completed { get; set; }
        //public int completion_on { get; set; }
        public int dl_limit { get; set; }
        public int dlspeed { get; set; }
        public long downloaded { get; set; }
        public int downloaded_session { get; set; }
        public int eta { get; set; }
        public bool f_l_piece_prio { get; set; }
        public bool force_start { get; set; }
        public string hash { get; set; }
        public int last_activity { get; set; }
        public string name { get; set; }
        public int num_complete { get; set; }
        public int num_incomplete { get; set; }
        public int num_leechs { get; set; }
        public int num_seeds { get; set; }
        public int priority { get; set; }
        public double progress { get; set; }
        public double ratio { get; set; }
        public double ratio_limit { get; set; }
        public string save_path { get; set; }
        public object seen_complete { get; set; }
        public bool seq_dl { get; set; }
        public long size { get; set; }
        public string state { get; set; }
        public bool super_seeding { get; set; }
        public long total_size { get; set; }
        public string tracker { get; set; }
        public int up_limit { get; set; }
        public long uploaded { get; set; }
        public int uploaded_session { get; set; }
        public int upspeed { get; set; }
    }

}
