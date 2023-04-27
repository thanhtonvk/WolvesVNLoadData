using LoadDuLieu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FireSharp;
using FireSharp.Config;
using FireSharp.Extensions;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace LoadDuLieu
{
    public class Program
    {
        public static void run()
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "R20tmZqaTY9WnrsEr9vk5nyzq6rZ6hO4OACKD1Su",
                BasePath = "https://wolfteam-f01f4-default-rtdb.asia-southeast1.firebasedatabase.app/"
            };
            IFirebaseClient client = new FireSharp.FirebaseClient(config);
            BanLenhDAL banLenhDAL = new BanLenhDAL(client);
            NewsDAL newsDal = new NewsDAL(client);
            RateDAL rateDAL = new RateDAL();
            while (true)
            {
                rateDAL.loadRate();
                banLenhDAL.loadBanLenh();
                newsDal.addNormalNews();
                newsDal.addNewsVip();
                Thread.Sleep(3000);
            }
        }

        public static string RemoveNameSubstring(string name)
        {
            int index = name.IndexOf("/Name");
            string uniqueKey = (index < 0) ? name : name.Remove(index, "/Name".Length);
            return uniqueKey;
        }

        private static List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();


        private static List<BanLenh> _banLenhs = new List<BanLenh>();

        public static void addToList()
        {
            Dictionary<string, string> value = list[list.Count - 1];
            BanLenh banLenh = new BanLenh()
            {
                Content = value["Content"],
                Date = DateTime.Parse(value["Date"]),
                Id = int.Parse(value["Id"]),
                Image = value["Image"],
                SL = double.Parse(value["SL"]),
                TP = double.Parse(value["TP"])
            };
            _banLenhs.Add(banLenh);

        }

        public static async void streamData()
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "R20tmZqaTY9WnrsEr9vk5nyzq6rZ6hO4OACKD1Su",
                BasePath = "https://wolfteam-f01f4-default-rtdb.asia-southeast1.firebasedatabase.app/"
            };
            IFirebaseClient client = new FirebaseClient(config);
            string currentDate = "2023-04-25";
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            await client.OnAsync($"BanLenh/{currentDate}", (sender, args) =>
            {
                string dataFromFB = args.Data;
                string paths = args.Path;
                string key = RemoveNameSubstring(paths);
                string uniqueKey = key.Split('/').Last();
                if (dictionary.ContainsKey(uniqueKey))
                {
                    dictionary = new Dictionary<string, string>();
                    dictionary.Add(uniqueKey, dataFromFB);
                    list.Add(dictionary);
                }
                else
                {
                    dictionary.Add(uniqueKey, dataFromFB);
                    if (list.Count == 0)
                    {
                        list.Add(dictionary);
                    }
                    else
                    {
                        list[list.Count - 1] = dictionary;
                    }
                }

                if (dictionary.ContainsKey("Content") && dictionary.ContainsKey("Date") &&
                    dictionary.ContainsKey("Id") && dictionary.ContainsKey("Image") &&
                    dictionary.ContainsKey("SL") && dictionary.ContainsKey("TP"))
                    addToList();
            });
        }

        public static void Main(string[] args)
        {
            // streamData();
            //
            // Console.ReadKey();
            try
            {
                run();
            }
            catch (Exception ex)
            {
                run();
            }
            // FCMPushNotification fcmPush = new FCMPushNotification();
            // var result = fcmPush.SendNotification("your notificatin title", "Your body message", "news");
            // if(result.Successful == true )
            // {
            //     Console.WriteLine("OK");
            // }
            // else
            // {
            //     Console.WriteLine(result.Error.ToString());
            // }
            Console.ReadKey();
        }
    }
}