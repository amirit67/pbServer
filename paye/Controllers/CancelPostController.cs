using Paye.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Paye.Controllers
{
    public class CancelPostController : ApiController
    {

        Models.PayeDBEntities db = new Models.PayeDBEntities();
      
        // GET: api/RemovePost/5
        public HttpResponseMessage Get(string id)
        {
            var post = (from x in db.Posts
                        where x.postId.ToString() == id
                        select x).FirstOrDefault();

            post.state = Post.State_Cancel;         
            db.SaveChanges();

            string[] tmp = post.applicants.Split(',');

            var result = (from x in db.Users
                        where (tmp.Length == 0 || tmp.Any(prefix => x.Id.ToString() == prefix))
                          select x.Token);

            return new HttpResponseMessage()
            {
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json")
            };
        }
      
    }
}
