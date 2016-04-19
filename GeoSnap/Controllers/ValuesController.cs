using GeoSnap.Models;
using GeoSnap.Providers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GeoSnap.Controllers
{
    //[Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        public HttpResponseMessage Get()
        {
            var res = ImageDataProvider.GetAll();
            if (res != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        public HttpResponseMessage Get(double lat, double lng) {

            var res = ImageDataProvider.FindNearby(lat, lng);
            if (res != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
        public HttpResponseMessage Get(double lat, double lng, int range)
        {
            //TODO: Implement range 0-100
            var res = ImageDataProvider.FindNearby(lat, lng);
            if (res != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // POST api/values
        public HttpResponseMessage Post(double lat, double lng)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string accessToken = (string)headerValues.FirstOrDefault();
            string userId = GraphProvider.getUserId(accessToken);
            if (userId == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            string username = UserDataProvider.getUsername(Int64.Parse(userId));
            if (username == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            Random rnd = new Random();
            string body = Request.Content.ReadAsStringAsync().Result;
            string converted = body.Replace('-', '+');
            converted = converted.Replace('_', '/');
            //double lat, double lng, string user,
            int id;
            try
            {
                byte[] imgByteArr = Convert.FromBase64String(converted);
                MemoryStream ms = new MemoryStream(imgByteArr);
                BlobProvider provider = new BlobProvider();
                //1 to max int32
                id = rnd.Next(1, 2147483647);
                provider.save(ms, id);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            ImageData d = new ImageData(id, DateTime.Now, lat, lng, username);
            ImageDataProvider.InsertImage(d);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // PUT api/values/5
        public void Put(int NumberOfImages)
        {
            //TODO: MUST PROVIDE IMAGE ON BLOB
            Random r = new Random();
            for (int i = 0; i < NumberOfImages; i++) {
                double lat = r.NextDouble() * (59.136236 - 59.113998) + 59.113998;
                double lng = r.NextDouble() * (11.424700 - 11.339854) + 11.339854;
                ImageData d = new ImageData(r.Next(1,100000), DateTime.Now, lat, lng, "root");
                ImageDataProvider.InsertImage(d);
            }
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
