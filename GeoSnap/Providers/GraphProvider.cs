using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace GeoSnap.Providers{

    public class GraphProvider
    {
        private static readonly string baseUrl = "https://graph.facebook.com/";

        public static string getUserId(string token)
        {
            Console.WriteLine(token);
            string fbId = TokenCacheProvider.getFbId(token);
            if (fbId != "") {
                return fbId;
            }


            string webresponse = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(baseUrl + "v2.5/me");
            req.Headers.Add("Authorization", "Bearer " + token);
            try {
                using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
                using (Stream stream = resp.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    webresponse = reader.ReadToEnd();
                }
            }
            catch {
                return null;
            }

            JObject responseJson = JObject.Parse(webresponse);

            TokenCacheProvider.cacheToken(Int64.Parse((string)responseJson["id"]), token);

            return (string)responseJson["id"];
        }
    }


}