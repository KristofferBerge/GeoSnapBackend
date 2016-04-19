using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoSnap.Providers
{
    public class UserDataProvider
    {
        public enum userOperationResult {
            UserNotFound,
            UserSuspended,
            UserFound,
            UserRegistrationSuccess,
            UserRegistrationFailed,
            UsernameIsTaken,
            UserAlreadyRegistered,
            AuthenticationFailed
        };

        public static string getUsername(long id) {
            string result = RunQuery("SELECT username FROM users WHERE fbId LIKE '" + id + "';");
            if (result == "")
            {
                return null;
            }
            return result;
        }

        public static bool isUsernameTaken(string username) {
            string result = RunQuery("SELECT * FROM users WHERE username LIKE '" + username + "';");
            if (result == "") {
                return false;
            }
            return true;
        }

        public static bool isUserRegistered(long id)
        {
            string result = RunQuery("SELECT * FROM users WHERE fbId LIKE '" + id + "';");
            if (result == "")
            {
                return false;
            }
            return true;
        }

        public static userOperationResult registerUser(long id, string username) {
            if (isUsernameTaken(username)) {
                return userOperationResult.UsernameIsTaken;
            }
            if (isUserRegistered(id)) {
                return userOperationResult.UserAlreadyRegistered;
            }
            string result = RunQuery("INSERT INTO users values (" + id + ", '" + username + "', FALSE );");
            string statsResult = RunQuery("INSERT INTO userStats (username,down,meh,up,castDown,castMeh,castUp) VALUES ('" + username + "',0,0,0,0,0,0);");
            if (result == "")
            {
                return userOperationResult.UserRegistrationSuccess;
            }
            else {
                return userOperationResult.UserRegistrationFailed;
            }
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
                foreach (string s in result) {
                    resultString += s;
                }
                return resultString;
            }
            catch(Exception e)
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