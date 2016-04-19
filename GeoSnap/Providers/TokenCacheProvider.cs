using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoSnap.Providers
{
    public class TokenCacheProvider
    {
        public static string getFbId(string token)
        {
            return RunQuery("SELECT fbId FROM cachedTokens WHERE token = '" + token + "' AND date > DATE_SUB(CURDATE(), INTERVAL 1 DAY);");
        }

        public static void cacheToken(Int64 fbId, string token)
        {
            RunQuery("INSERT INTO cachedTokens (token, fbId) VALUES( '" + token + "', '" + fbId + "');");
        }

        private static string RunQuery(string query)
        {
            string connectionString = "Database=geosnap;Data Source=40.114.229.128; User Id=***;Password=***";
            MySqlConnection con = new MySqlConnection(connectionString);
            try
            {
                con.Open();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                MySqlDataReader reader = cmd.ExecuteReader();
                List<string> result = new List<string>();
                while (reader.Read())
                {
                    result.Add(reader.GetString(0));
                }
                string resultString = "";
                foreach (string s in result)
                {
                    resultString += s;
                }
                return resultString;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                con.Close();
            }
        }
    }
}