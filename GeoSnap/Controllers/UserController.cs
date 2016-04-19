using GeoSnap.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GeoSnap.Controllers
{
    public class UserController : ApiController
    {
        // GET: api/User
        public HttpResponseMessage Get()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string accessToken = (string)headerValues.FirstOrDefault();
            string userId = GraphProvider.getUserId(accessToken);
            if (userId == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            string username = UserDataProvider.getUsername(Int64.Parse(userId));
            if(username == null)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            return Request.CreateResponse(HttpStatusCode.OK, username);
        }

        // POST: api/User
        public HttpResponseMessage Post(string username)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string accessToken = (string)headerValues.FirstOrDefault();
            string userId = GraphProvider.getUserId(accessToken);
            if (userId == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            UserDataProvider.userOperationResult result = UserDataProvider.registerUser(Int64.Parse(userId), username);
            switch (result) {
                case UserDataProvider.userOperationResult.UserRegistrationSuccess:
                    return Request.CreateResponse(HttpStatusCode.Created);
                case UserDataProvider.userOperationResult.UsernameIsTaken:
                    return Request.CreateResponse(HttpStatusCode.Conflict);
                case UserDataProvider.userOperationResult.UserRegistrationFailed:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                case UserDataProvider.userOperationResult.UserAlreadyRegistered:
                    return Request.CreateResponse(HttpStatusCode.MethodNotAllowed);
                default:
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // PUT: api/User/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }
    }
}
