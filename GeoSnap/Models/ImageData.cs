using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoSnap.Models
{
    public class ImageData
    {
        public int ImgId;
        public DateTime Timestamp;
        public double Lat;
        public double Lng;
        public string Usr;

        public ImageData(int imgId, DateTime timestamp, double lat, double lng, string usr) {
            ImgId = imgId;
            Timestamp = timestamp;
            Lat = lat;
            Lng = lng;
            Usr = usr;
        }
    }
}