using GeoSnap.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace GeoSnap.Providers
{
    public class ImageDataProvider
    {

        //Probably not in use
        public static List<ImageData> FindById(int id) {
            return RunQuery("SELECT * FROM img WHERE imgId = " + "'" + id + "';");
        }

        public static List<ImageData> FindNearby(double lat, double lng){
            double latRange = 0.05;
            double lngRange = 0.05;
            return RunQuery("SELECT * FROM img WHERE (lat BETWEEN " + (lat - latRange) + " AND " + (lat + latRange) + ") AND (lng BETWEEN " + (lng - lngRange) + " AND " + (lng + lngRange) + ") AND (date > NOW() - INTERVAL 23 HOUR) ;");
        }

        public static List<ImageData> GetAll(){
            return RunQuery("SELECT * FROM img WHERE date > NOW() - INTERVAL 23 HOUR;");
        }


        public static int InsertImage(ImageData d) {

            if (RunQuery("INSERT INTO img (imgId,lat,lng,usr) VALUES ('" + d.ImgId + "', '" + d.Lat + "', '" + d.Lng + "', '" + d.Usr + "');") != null){
                return 200;
            }
            return 400;
        }

        private static List<ImageData> RunQuery(string query) {
            string connectionString = "Database=geosnap;Data Source=40.114.229.128;User Id=***;Password=***";
            MySqlConnection con = new MySqlConnection(connectionString);
            try{
                con.Open();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                MySqlDataReader reader = cmd.ExecuteReader();
                List<ImageData> result = new List<ImageData>();
                while (reader.Read()){
                    result.Add(new ImageData(int.Parse(reader.GetString(0)), DateTime.Parse(reader.GetString(1)), double.Parse(reader.GetString(2)), double.Parse(reader.GetString(3)), reader.GetString(4)));
                }
                return result;
            }
            catch{
                return null;
            }
            finally{
                con.Close();
            }
        }

    }
}