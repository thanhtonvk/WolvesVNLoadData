using LoadDuLieu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FireSharp.Config;
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

        public static void Main(string[] args)
        {
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