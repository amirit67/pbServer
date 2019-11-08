
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
    public class getUncomingPostsController : ApiController
    {
        public HttpResponseMessage Post([FromBody]FormDataCollection formDataCollection)
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Headers["PayeBash"] != null)
            {

                int skip = (Convert.ToInt32(formDataCollection.Get("Skip")) * 20) + 20;
                Guid UserId = Guid.Empty;
                if (null != formDataCollection.Get("UserId"))
                    UserId = Guid.Parse(formDataCollection.Get("UserId").Trim());
                          
                PayeDBEntities db = new PayeDBEntities();
                {
                    var userId = db.Users.FirstOrDefault(r => r.UserId == UserId).Id;
                    var query = (from x in db.Posts
                                 where
                                         x.applicants.Contains("," + userId.ToString() + ",")
                                    &&
                                        (x.state != 5 && x.state != 9)
                                    &&
                                        x.timeToJoin >= DateTime.Now
                                 orderby x.Id descending
                                 select new
                                 {
                                     PostId = x.postId.ToString().Trim(),
                                     Title = x.title.Trim(),
                                     City = x.city,
                                     IsWoman = x.isWoman,
                                     IsImmediate = x.isImmediate,
                                     Subject = x.subject,
                                     Cost = x.cost.Trim(),
                                     Images = null != x.images.Trim() ? (x.images) : "null",
                                     Tag = x.tag.Trim(),
                                     CreateDate = x.createDate.ToString(),
                                     Deadline = x.timeToJoin.ToString(),
                                     State = x.state
                                 }).Skip(skip - 20).Take(20).ToList();
                    var result = from x in query
                                 select new postsWrapper
                                 {
                                     postId = x.PostId.ToString().Trim(),
                                     title = x.Title.Trim(),
                                     city = x.City,
                                     subject = x.Subject,
                                     isWoman = (bool)x.IsWoman,
                                     isImmediate = (bool)x.IsImmediate,
                                     cost = x.Cost.Trim(),
                                     images = null != x.Images.Trim() ? (x.Images) : "null",
                                     tag = x.Tag.Trim(),
                                     createDate = BaseSystemModel.ResizeImage.GetDateDifferencesAsDescription(Convert.ToDateTime(x.CreateDate.ToString()), DateTime.Now, 0),
                                     timeToJoin = BaseSystemModel.ResizeImage.GetDateDifferencesAsDescription2(DateTime.Now, Convert.ToDateTime(x.Deadline.ToString()), 0),
                                     state = Dictioanry.GetStatesPayePost[(byte)x.State].ToString()
                                     + "-" + Dictioanry.GetStatesDescriptionPayePost[(byte)x.State].ToString()
                                     + "-" + Dictioanry.GetStatesColorPayePost[(byte)x.State].ToString()
                                 };
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json")
                    };
                }
            }
            else
                return null;
                             //CreateDate = BaseSystemModel.ResizeImage.GetDateDifferencesAsDescription(Convert.ToDateTime(x.CreateDate.ToString()), DateTime.Now, 0),
                             ////CreateDate = Utilty.ToPersianDateTime(Convert.ToDateTime(x.CreateDate.ToString())).ToString().Substring(2, 14),
       
        }
    }
}
