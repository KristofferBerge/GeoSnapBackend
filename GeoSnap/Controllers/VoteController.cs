using GeoSnap.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GeoSnap.Controllers
{
    public class VoteController : ApiController
    {
        public HttpResponseMessage Get()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string accessToken = (string)headerValues.FirstOrDefault();
            string userId = GraphProvider.getUserId(accessToken);
            if (userId == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            string user = UserDataProvider.getUsername(Int64.Parse(userId));
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            return Request.CreateResponse(HttpStatusCode.OK, UserStatsProvider.getUserRating(user));
        }
        // GET: api/Vote/5
        public HttpResponseMessage Get(string username)
        {
            return Request.CreateResponse(HttpStatusCode.OK, UserStatsProvider.getUserRating(username));
        }

        // POST: api/Vote
        public HttpResponseMessage Post(string username, int vote)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string accessToken = (string)headerValues.FirstOrDefault();
            string userId = GraphProvider.getUserId(accessToken);
            if (userId == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            string user = UserDataProvider.getUsername(Int64.Parse(userId));
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            if (!(vote == 1 || vote == 0 || vote == -1)) {
                return Request.CreateResponse(HttpStatusCode.MethodNotAllowed);
            }
            if (UserStatsProvider.castVote(username, user, vote))
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            } 
        }
    }
}
