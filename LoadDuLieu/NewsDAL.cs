using FireSharp.Interfaces;
using LoadDuLieu.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace LoadDuLieu
{
    class NewsResponse
    {
        public DateTime? date { get; set; }
        public TimeSpan? time { get; set; }
        public string content { get; set; }
        public bool? type { get; set; }
    }
    class NewsDAL
    {

        DBContext db;
        WebClient client;
        private IFirebaseClient firebaseClient;
        public NewsDAL(IFirebaseClient firebaseClient)
        {
            db = new DBContext();
            client = new WebClient();
            this.firebaseClient = firebaseClient;
        }
        public void addNormalNews()
        {

            Console.WriteLine("Load news");
            string url =
                "C:/Users/Administrator/AppData/Roaming/MetaQuotes/Terminal/Common/Files/Tin_tuc.csv";

            client.DownloadFile(url, "TinTuc_New.txt");
            var lineCountNewFile = File.ReadLines("TinTuc_New.txt").Count();

            var lineCountOldFile = File.ReadLines("TinTuc.txt").Count();

            if (lineCountNewFile != lineCountOldFile)
            {
                client.DownloadFile(url, "Tintuc.txt");
                string values = "";
                var lines = File.ReadLines("TinTuc.txt").ToList();
                for (int i = lineCountOldFile; i < lines.Count(); i++)
                {
                    values += (lines[i] + "\n");

                }
                values.Replace("****", "**");
                foreach (var value in values.Split(new[] { "**" }, StringSplitOptions.None))
                {
                    if (value.Trim().Length < 1) continue;
                    else
                    {
                        db.AddNews(DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")),
                            TimeSpan.Parse(DateTime.Now.ToString("HH:mm")), value.Trim(), false);
                        string ngay = DateTime.Now.ToString("yyyy-MM-dd");
                        var model = db.News.ToList();
                        model.Reverse();
                        var news = model[0];
                        firebaseClient.Set(@"News/" + ngay + "/" + news.Id, news);
                    }
                }

            }
        }
        public void GetNewsVip()
        {
            Console.WriteLine("Load news vip");
            string url =
                   "C:/Users/Administrator/AppData/Roaming/MetaQuotes/Terminal/Common/Files/Tin_tuc_vip.csv";

            client.DownloadFile(url, "TinTuc_vip_New.txt");
            var lineCountNewFile = File.ReadLines("TinTuc_vip_New.txt").Count();

            var lineCountOldFile = File.ReadLines("Tin_tuc_vip.txt").Count();

            if (lineCountNewFile != lineCountOldFile)
            {
                client.DownloadFile(url, "Tin_tuc_vip.txt");
                string values = "";
                var lines = File.ReadLines("Tin_tuc_vip.txt").ToList();
                for (int i = lineCountOldFile; i < lines.Count(); i++)
                {
                    values += (lines[i] + "\n");

                }
                values.Replace("****", "**");
                foreach (var value in values.Split(new[] { "**" }, StringSplitOptions.None))
                {
                    if (value.Trim().Length < 1) continue;
                    else
                    {
                        db.AddNews(DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")),
                            TimeSpan.Parse(DateTime.Now.ToString("HH:mm")), value.Trim(), true);
                        string ngay = DateTime.Now.ToString("yyyy-MM-dd");
                        var model = db.News.ToList();
                        model.Reverse();
                        var news = model[0];
                        firebaseClient.Set(@"NewsVip/" + ngay + "/" + news.Id, news);
                    }
                }

            }

        }
    }
}
