using LoadDuLieu.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LoadDuLieu
{
    public class RateDAL
    {
        DBContext db;
        WebClient client;
        public RateDAL()
        {
            db = new DBContext();
            client = new WebClient();
        }
        public void loadRate()
        {
            Console.WriteLine("Load rates");
            string url =
                "C:/Users/Administrator/AppData/Roaming/MetaQuotes/Terminal/Common/Files/Rates.csv";
            
            client.DownloadFile(url, "Rates.txt");
            StreamReader reader = new StreamReader("Rates.txt");
            string line = "";
            while ((line = reader.ReadLine()) != null)
            {
                string[] arr = line.Split('	');
                if (arr.Length == 3)
                {
                    db.InsertSymbol(arr[0]);
                    db.InsertRate(arr[0], double.Parse(arr[2]), double.Parse(arr[1]));
                }
            }
            reader.Close();
        }
    }
}
