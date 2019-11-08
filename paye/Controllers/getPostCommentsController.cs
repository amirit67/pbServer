
using BaseSystemModel.Utilty;
using Paye.Models;
using BaseSystemModel.Helper;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web;
using System.Collections.Generic;

namespace Paye.Controllers
{
    public class getPostCommentsController : ApiController
    {
        public HttpResponseMessage Get(string id)
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Headers["PayeBash"] != null)
            {
                PayeDBEntities db = new PayeDBEntities();             
                var result = (from x in db.Comments
                              join c in db.Users
                              on x.userId equals c.Id
                              where
                              x.postId.ToString() == id
                              &&
                              x.state == true
                            
                 select new
                 {
                     Comment1 = x.comment,
                     UserName = x.userName,
                     Image = c.ProfileImage 
                 }).ToList();
                return new HttpResponseMessage()
                    {
                        Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json")
                    };
                
            }
            else
                return null;
                             //CreateDate = BaseSystemModel.ResizeImage.GetDateDifferencesAsDescription(Convert.ToDateTime(x.CreateDate.ToString()), DateTime.Now, 0),
                             ////CreateDate = Utilty.ToPersianDateTime(Convert.ToDateTime(x.CreateDate.ToString())).ToString().Substring(2, 14),
       
        }
    }
}
