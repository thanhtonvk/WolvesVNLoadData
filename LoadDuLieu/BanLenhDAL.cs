using FireSharp.Interfaces;
using LoadDuLieu.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace LoadDuLieu
{
    class BanLenhDAL
    {
        DBContext db;
        WebClient client;
        private IFirebaseClient firebaseClient;
        private SendNotification sendNotification;

        public BanLenhDAL(IFirebaseClient firebaseClient, SendNotification sendNotification)
        {
            db = new DBContext();
            client = new WebClient();
            this.firebaseClient = firebaseClient;
            this.sendNotification = sendNotification;
        }

        public List<string> splitBanLenh(string value)
        {
            List<string> arr = value.Split(new[] {"/n"}, StringSplitOptions.None).ToList();
            if (arr.Count > 6) arr.RemoveAt(0);
            return arr;
        }

        public void loadBanLenh()
        {
            Console.WriteLine("Load ban lenh");
            string url =
                "C:/Users/Administrator/AppData/Roaming/MetaQuotes/Terminal/Common/Files/Ban_lenh.csv";

            client.DownloadFile(url, "Ban_lenh_New.txt");
            int lineCountNewFile = File.ReadLines("Ban_lenh_New.txt").Count();

            int lineCountOldFile = File.ReadLines("Ban_lenh.txt").Count();
            if (lineCountNewFile != lineCountOldFile)
            {
                client.DownloadFile(url, "Ban_lenh.txt");
                string values = "";
                var lines = File.ReadLines("Ban_lenh.txt").ToList();

                for (int i = lineCountOldFile; i < lines.Count(); i++)
                {
                    values += (lines[i] + "/n");
                }

                foreach (var content in values.Split(new[] {"**"}, StringSplitOptions.None))
                {
                    Console.WriteLine(values);
                    if (content.Trim().Length < 1) continue;
                    else
                    {
                        var value = splitBanLenh(content);
                        string[] date = value[0].Split(':')[1].Replace(" ", "").Split('-');
                        string image = value[4];
                        string fileName = image.Split('/')[1];
                        string urlImage =
                            "C:/Users/Administrator/AppData/Roaming/MetaQuotes/Terminal/9D15457EC01AD10E06A932AAC616DC32/MQL4/Files/" +
                            fileName;
                        client.DownloadFile(urlImage,
                            "D:/WolvesServer/WolvesServer/Image/BanLenh/" + fileName);

                        db.AddBanLenh(DateTime.Parse(date[2] + "-" + date[1] + "-" + date[0]), value[1],
                            float.Parse(value[2].Split(':')[1].Trim()), float.Parse(value[3].Split(':')[1].Trim()),
                            "http://139.99.116.68/Image/BanLenh/" + fileName);
                        var model = db.BanLenhs.ToList();
                        model.Reverse();
                        var banLenh = model[0];
                        string ngay = date[2] + "-" + date[1] + "-" + date[0];
                        firebaseClient.Set(@"BanLenh/" + ngay + "/" + banLenh.Id, banLenh);
                        sendNotification.Send($"{banLenh.Content}");
                    }
                }
            }
        }
    }
}