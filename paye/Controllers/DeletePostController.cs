using Paye.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Paye.Controllers
{
    public class DeletePostController : ApiController
    {

        Models.PayeDBEntities db = new Models.PayeDBEntities();
      
        // GET: api/RemovePost/5
        public string Post(ParamsWrapper paramsWrapper)
        {
            Guid id = paramsWrapper.PostId;
            int state = paramsWrapper.Status;
            var post = db.Posts.FirstOrDefault(x => x.postId == id);              
            post.state = Models.Post.State_Delete_Successful;           
            db.SaveChanges();
            return id.ToString();
        }
      
    }
}
