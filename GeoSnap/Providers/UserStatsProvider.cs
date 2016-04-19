using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoSnap.Providers
{
    public class UserStatsProvider
    {
        public static bool castVote(string username, string voter, int vote) {
            string userField;
            string voterField;
            if (vote == -1) {
                userField = "down";
                voterField = "castDown";
            }
            else if (vote == 0) {
                userField = "meh";
                voterField = "castMeh";
            }
            else {
                userField = "up";
                voterField = "castUp";
            }
            if (
            RunQuery("UPDATE userStats SET " + userField + " = " + userField + " +1 WHERE username = '" + username + "';",false) == "" &&
            RunQuery("UPDATE userStats SET " + voterField + " = " + voterField + " +1 WHERE username = '" + voter + "';",false) == "") {
                return true;
            }
            return false;
        }


        public static string getUserRating(string username)
        {
            return RunQuery("SELECT down, up FROM userStats WHERE username = '" + username + "';",true);
        }

        private static string RunQuery(string query,Boolean addresult)
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
                    if (addresult) {
                        int rating = Int32.Parse(reader.GetString(1));
                        rating -= Int32.Parse(reader.GetString(0));
                        result.Add(rating.ToString());
                        break;
                    }
                    result.Add(reader.GetString(0));
                    result.Add(reader.GetString(1));
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