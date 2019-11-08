using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Paye.Controllers
{
    public class UpdatePostStateController : ApiController
    {

        Models.PayeDBEntities db = new Models.PayeDBEntities();
      
        // GET: api/RemovePost/5
        public bool Post()
        {
             var httpRequest = HttpContext.Current.Request;
             var postid = httpRequest.Form.Get("PostId");
             var state = httpRequest.Form.Get("State");
             var list = db.Posts.FirstOrDefault(x => x.postId.ToString() == postid);

             list.state = Convert.ToByte(state);         
             db.SaveChanges();
             return true;
        }
      
    }
}
