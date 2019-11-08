
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
    public class getMyPostsController : ApiController
    {

        public HttpResponseMessage Post(ParamsWrapper formData)
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Headers["PayeBash"] != null)
            {

                int skip = (formData.Skip * 20) + 20;
                Guid UserId = formData.UserId;
                int cityCode = formData.city;
                int SubjectCode = formData.subject;
                string ids = formData.ids;
                string[] item = { "" };
                var parentCodesplit = !string.IsNullOrEmpty(ids) ? ids.Split(',').ToArray() : item;
                var parentCodeList = parentCodesplit == null ? new List<string>() : parentCodesplit.ToList();

                PayeDBEntities db = new PayeDBEntities();


                //my posts
                var user = db.Users.FirstOrDefault(r => r.UserId == UserId);
                var query = (from x in db.Posts
                             where
                                     x.userId == user.Id
                                &&
                                    (x.state == Models.Post.State_New || x.state == Models.Post.State_Ok || x.state == Models.Post.State_OkEdit)
                                &&
                                  (parentCodeList.Count == 0 || parentCodeList.Any(prefix => x.postId.ToString() == prefix))
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
                                 timeToJoin = x.timeToJoin.ToString(),
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
                                 timeToJoin = BaseSystemModel.ResizeImage.GetDateDifferencesAsDescription2(DateTime.Now, Convert.ToDateTime(x.timeToJoin.ToString()), 0),
                                 state = ((bool)user.IsMobileAuthenticate) ? Dictioanry.GetStatesPayePost[(byte)x.State].ToString()
                                 + "-" + Dictioanry.GetStatesDescriptionPayePost[(byte)x.State].ToString()
                                 + "-" + Dictioanry.GetStatesColorPayePost[(byte)x.State].ToString() : "منتظر تایید شماره-لطفا شماره موبایل خود را تایید کنید.-#595FB1"
                             };
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
