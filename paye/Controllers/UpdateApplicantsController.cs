using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Paye.Controllers
{
    public class UpdateApplicantsController : ApiController
    {

        Paye.Models.PayeDBEntities db = new Paye.Models.PayeDBEntities();
      
        // GET: api/RemovePost/5
        public bool Post()
        {
            var httpRequest = HttpContext.Current.Request;
            var postid = httpRequest.Form.Get("PostId");
            var userId = httpRequest.Form.Get("UserId");

            /*long*/string Id = (from x in db.Users
                        where x.UserId.ToString() == userId
                       select x).FirstOrDefault().Id;

            var item = db.Posts.FirstOrDefault(x => x.postId.ToString() == postid);
            string[] Ids = item.applicants.Split(',');
            if (Ids.Contains(Id.ToString()))
            {
                item.applicants = item.applicants.Replace("," + Id + ",", ",");
                db.SaveChanges();
                return false;
            }
               
            else
            {
                if(string.IsNullOrEmpty(item.applicants))
                    item.applicants += "," + Id + ",";
                else
                    item.applicants += Id + ",";
                db.SaveChanges();
                return true;
            }
          
            return false;
        }
      
    }
}
