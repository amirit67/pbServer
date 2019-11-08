
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
    public class getPostsController : ApiController
    {

        public HttpResponseMessage Post(ParamsWrapper formData)
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Headers["PayeBash"] != null)
            {

                int skip = (formData.Skip * 20) + 20;
                Guid UserId = formData.UserId;
                int cityCode = formData.city;
                string contentSearch = formData.contentSearch.Trim();
                int SubjectCode = formData.subject;            
                string ids = formData.ids;

                string[] item = { "" };
                var parentCodesplit = !string.IsNullOrEmpty(ids) ? ids.Split(',').ToArray() : item;
                var parentCodeList = parentCodesplit == item ? new List<string>() : parentCodesplit.ToList();

                PayeDBEntities db = new PayeDBEntities();
                //search
                if (cityCode != 0 || SubjectCode != 0 || !string.IsNullOrEmpty(contentSearch))
                {
                    var query = (from x in db.Posts
                                 where
                                     x.state == Models.Post.State_Ok
                                 &&
                                     x.timeToJoin >= DateTime.Now
                                 &&
                                   (cityCode == 0 || x.city == cityCode)
                                 &&
                                   (SubjectCode == 0 || x.subject == SubjectCode)
                                 &&
                                  (parentCodeList.Count == 0 || parentCodeList.Any(prefix => x.postId.ToString() == prefix))
                                 &&
                                   (string.IsNullOrEmpty(contentSearch) || x.title.Contains(contentSearch) || x.description.Contains(contentSearch) || x.tag.Contains(contentSearch))

                                 orderby x.Id descending
                                 select new
                                 {
                                     postId = x.postId.ToString().Trim(),
                                     title = x.title.Trim(),
                                     city = x.city,
                                     subject = x.subject,
                                     cost = x.cost.Trim(),
                                     isWoman = x.isWoman,
                                     isImmediate = x.isImmediate,
                                     images = null != x.images.Trim() ? (x.images) : "null",
                                     tag = x.tag.Trim(),
                                     createDate = x.createDate.ToString(),
                                     timeToJoin = x.timeToJoin.ToString(),
                                     state = x.state
                                 }).Skip(skip - 20).Take(20).ToList();

                    var result = from x in query
                                 select new postsWrapper
                                 {
                                     postId = x.postId.ToString().Trim(),
                                     title = x.title.Trim(),
                                     city = x.city,
                                     subject = x.subject,
                                     cost = x.cost.Trim(),
                                     images = null != x.images.Trim() ? (x.images) : "null",
                                     tag = x.tag.Trim(),
                                     createDate = BaseSystemModel.ResizeImage.GetDateDifferencesAsDescription(Convert.ToDateTime(x.createDate.ToString()), DateTime.Now, 0),
                                     timeToJoin = BaseSystemModel.ResizeImage.GetDateDifferencesAsDescription2(DateTime.Now, Convert.ToDateTime(x.timeToJoin.ToString()), 0),
                                     state = Dictioanry.GetStatesPayePost[(byte)x.state].ToString()
                                     + "-" + Dictioanry.GetStatesDescriptionPayePost[(byte)x.state].ToString()
                                     + "-" + Dictioanry.GetStatesColorPayePost[(byte)x.state].ToString()
                                 };
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json")
                    };

                }
                else if (Guid.Empty == UserId)
                {
                    var query = (from x in db.Posts
                                 where
                                     x.state == Models.Post.State_Ok
                                 &&
                                     x.timeToJoin >= DateTime.Now
                                 &&
                                  (parentCodeList.Count == 0 || parentCodeList.Any(prefix => x.postId.ToString() == prefix))
                                 orderby x.Id descending
                                 select new
                                 {
                                     postId = x.postId.ToString().Trim(),
                                     title = x.title.Trim(),
                                     city = x.city,
                                     subject = x.subject,
                                     cost = x.cost.Trim(),
                                     isWoman = x.isWoman,
                                     isImmediate = x.isImmediate,
                                     images = null != x.images.Trim() ? (x.images) : "null",
                                     tag = x.tag.Trim(),
                                     createDate = x.createDate.ToString(),
                                     timeToJoin = x.timeToJoin.ToString(),
                                     state = x.state
                                 }).Skip(skip - 20).Take(20).ToList();
                    try
                    {
                        var result = from x in query
                                     select new postsWrapper
                                     {
                                         postId = x.postId.ToString().Trim(),
                                         title = x.title.Trim(),
                                         city = x.city,
                                         subject = x.subject,
                                         isWoman = (bool)x.isWoman,
                                         isImmediate = (bool)x.isImmediate,
                                         cost = x.cost.Trim(),
                                         images = null != x.images.Trim() ? (x.images) : "null",
                                         tag = x.tag.Trim(),
                                         createDate = BaseSystemModel.ResizeImage.GetDateDifferencesAsDescription(Convert.ToDateTime(x.createDate.ToString()), DateTime.Now, 0),
                                         timeToJoin = BaseSystemModel.ResizeImage.GetDateDifferencesAsDescription2(DateTime.Now, Convert.ToDateTime(x.timeToJoin.ToString()), 0),
                                         state = Dictioanry.GetStatesPayePost[(byte)x.state].ToString()
                                         + "-" + Dictioanry.GetStatesDescriptionPayePost[(byte)x.state].ToString()
                                         + "-" + Dictioanry.GetStatesColorPayePost[(byte)x.state].ToString()
                                     };
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json")
                        };
                    }
                    catch (Exception e)
                    {
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(e.InnerException.Message), Encoding.UTF8, "application/json")
                        };
                    }

                }
                else
                    return null;
            }
            else
                return null;
            //CreateDate = BaseSystemModel.ResizeImage.GetDateDifferencesAsDescription(Convert.ToDateTime(x.CreateDate.ToString()), DateTime.Now, 0),
            ////CreateDate = Utilty.ToPersianDateTime(Convert.ToDateTime(x.CreateDate.ToString())).ToString().Substring(2, 14),

        }
    }
}
