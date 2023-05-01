using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using Google.Apis.Auth.OAuth2;

namespace LoadDuLieu
{
    public class SendNotification
    {
        private HttpClient client;
        private IFirebaseClient fbclient;

        public SendNotification()
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "R20tmZqaTY9WnrsEr9vk5nyzq6rZ6hO4OACKD1Su",
                BasePath = "https://wolfteam-f01f4-default-rtdb.asia-southeast1.firebasedatabase.app/"
            };
            fbclient = new FirebaseClient(config);
            client = new HttpClient();
            client.BaseAddress = new Uri("https://fcm.googleapis.com/fcm/send");
            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("key",
                    "=AAAAVjhZjgI:APA91bFLNQ3ETsnhjAUp0L8rNaPcflaBFnY9RuXYpxLXLABfnPIXwBWJ-Lz8dRdG2P49SH1N9kCin4k0N0tWjAUAMgMTUJmlQ6uWpYHX6Jem-SDseMhGDmD754IbZY-1mkmzmg8H42d-");
        }

        public void Send(string body)
        {
            string tokens = GetTokens();
            var json = @"{
    ""registration_ids"":[tokens],
            ""myData"":""bodycontent""
        }";
            json = json.Replace("tokens", tokens).Replace("bodycontent", body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            Console.WriteLine(content);
            var response = client.PostAsync("", content).Result;
            string resultContent = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(resultContent);
        }

        public string GetTokens()
        {
            string result = "";
            var response = fbclient.Get("Token").ResultAs<Dictionary<string, string>>();
            foreach (var value in response.Values)
            {
                result += @"""" + value + @"""" + ",";
            }

            return result;
        }
    }
}