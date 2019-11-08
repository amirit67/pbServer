using Paye.Models;
using BaseSystemModel.Helper;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web;
using System.Collections.Generic;

namespace Paye.Controllers
{
    public class getPostDetailsController : ApiController
    {
       
        public HttpResponseMessage Post(ParamsWrapper paramsWrapper)
        {

            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Headers["PayeBash"] != null)
            {
                var postid = paramsWrapper.PostId;
                var userid = paramsWrapper.UserId;

                

                PayeDBEntities db = new PayeDBEntities();
                var userOwner = db.Users.FirstOrDefault(a => a.UserId == userid);
                Post post = null;
                if (userOwner != null)
                    post = db.Posts.FirstOrDefault(x => x.postId == postid && x.timeToJoin >= DateTime.Now);
                else
                    post = db.Posts.FirstOrDefault(x => x.postId == postid && (x.state == Models.Post.State_Ok || x.state == Models.Post.State_OkEdit) && x.timeToJoin >= DateTime.Now);


                /*var result = from x in post
                             select new Posts
                             {
                                 FullName = db.Users.Where(r => r.Id == x.UserId).FirstOrDefault().Name.Trim() + " " + db.Users.Where(r => r.Id == x.UserId).FirstOrDefault().Family.Trim(),
                                 UserImage = db.Users.Where(r => r.Id == x.UserId).FirstOrDefault().ProfileImage,
                                 Title = PersianNumber(x.Title),
                                 Description = PersianNumber(x.Description),
                                 Subject = x.Subject.Trim(),
                                 City = x.City.Trim(),
                                 ContactWays = x.ContactWays.Trim(),
                                 Cost = PersianNumber(x.Cost).Trim(),
                                 NumberFollowers = PersianNumber(x.NumberFollowers),
                                 Images = Url.Content("~/Images/Paye/") + x.Images.Split(',')[0] + ".jpg",
                                 CreateDate = BaseSystemModel.ResizeImage.GetDateDifferencesAsDescription(Convert.ToDateTime(x.CreateDate.ToString()), DateTime.Now, 0),
                                 //CreateDate = Utilty.ToPersianDateTime(Convert.ToDateTime(x.CreateDate.ToString())).ToString().Substring(2, 14),
                                 StartDate = Utilty.ToPersianDateTime(Convert.ToDateTime(x.StartDate.ToString())).ToString().Substring(2, 14),
                                 finishDate = Utilty.ToPersianDateTime(Convert.ToDateTime(x.FinishDate.ToString())).ToString().Substring(2, 14),
                                 //Deadline = Utilty.ToPersianDateTime(Convert.ToDateTime(x.Deadline.ToString())).ToString().Substring(2, 14),
                                 Deadline = BaseSystemModel.ResizeImage.GetDateDifferencesAsDescription2(DateTime.Now, Convert.ToDateTime(x.Deadline.ToString()), 0),
                                 Tag = x.Tag.Trim(),
                                 Longitude = x.Longitude,
                                 Latitude = x.Latitude,                             
                             
                             };*/


                List<string> BaseProperty = new List<string>();
                if (!string.IsNullOrEmpty(post.phoneNumber))
                    BaseProperty.Add("شماره تماس : " + PersianNumber(post.phoneNumber).Trim());
                if (!string.IsNullOrEmpty(post.link))
                    BaseProperty.Add("وب سایت : " + PersianNumber(post.link).Trim());
                BaseProperty.Add("هزینه : " + PersianNumber(post.cost).Trim());
                BaseProperty.Add("تعداد هم پایه : " + PersianNumber(post.numberFollowers));
                BaseProperty.Add("تاریخ شروع  : " + post.startDate.ToString());
                BaseProperty.Add("تاریخ پایان : " + post.endDate.ToString());
                BaseProperty.Add("مهلت هم پا شدن : " + BaseSystemModel.ResizeImage.GetDateDifferencesAsDescription2(DateTime.Now, Convert.ToDateTime(post.timeToJoin.ToString()), 0));
                if (!string.IsNullOrEmpty(post.tag.Trim()))
                    BaseProperty.Add("هشتگ : " + post.tag.Trim());
                //BaseProperty.Add("راه های ارتباطی : " + post.ContactWays.Trim());
                BaseProperty.Add("توضیحات : " + PersianNumber(post.description));

                returnPostdetails post2 = new returnPostdetails();
                var item = db.Users.Where(r => r.Id == post.userId).FirstOrDefault();
                if (post.state == 9)
                    post2.title = post.title.Trim() + "(این برنامه لغو گردید)";
                else
                    post2.title = post.title.Trim();

                string[] Ids = post.applicants.Split(',');
                var query = (from x in db.Users
                             where
                             Ids.Any(a => a == x.Id.ToString())
                             orderby x.Id descending
                             select new
                             {
                                 UserId = x.UserId.ToString().Trim(),
                                 Name = x.Name.Trim(),
                                 Family = x.Family.Trim(),
                                 ProfileImage = x.ProfileImage.Trim(),
                                 IsMobileAuthenticate = x.IsMobileAuthenticate,
                             }).ToList();

                List<Applicant> result = (from x in query
                                          select new Applicant
                                          {
                                              UserId = x.UserId,
                                              ProfileImage = x.ProfileImage
                                          }).ToList();
                List<string> applicants = new List<string>();
                for (int i = 0; i < result.Count(); i++)
                    applicants.Add(result[i].UserId + "/" + result[i].ProfileImage);

                post2.applicants = applicants;

                post2.isWoman = (bool)post.isWoman;
                post2.userId = item.UserId.ToString().Trim();
                post2.telegram = item.Telegram.ToString().Trim();
                post2.instagram = item.Instagram.ToString().Trim();
                post2.soroosh = item.Soroosh.ToString().Trim();
                post2.gmail = item.Gmail.ToString().Trim();
                post2.city = post.city;
                post2.images = null != post.images.Trim() ? (post.images) : "null";
                post2.createDate = BaseSystemModel.ResizeImage.GetDateDifferencesAsDescription(Convert.ToDateTime(post.createDate.ToString()), DateTime.Now, 0);
                post2.subject = post.subject;
                post2.latitude = post.latitude.Trim();
                post2.longitude = post.longitude.Trim();
                if ((bool)item.IsMobileAuthenticate)
                {
                    post2.state = Dictioanry.GetStatesPayePost[(byte)post.state].ToString()
                    + "-" + Dictioanry.GetStatesDescriptionPayePost[(byte)post.state].ToString()
                    + "-" + Dictioanry.GetStatesColorPayePost[(byte)post.state].ToString();
                }
                else
                {
                    post2.state = "منتظر تایید شماره-لطفا شماره موبایل خود را تایید کنید.-#595FB1";
                }

                var user = db.Users.Where(r => r.Id == post.userId).FirstOrDefault();
                post2.username = user.Name.Trim() + " " + user.Family.Trim();
                post2.token = user.Token.Trim();
                if (user.IsShowMobile != null && (bool)user.IsShowMobile)
                    post2.mobile = user.Mobile.Trim();
                else
                    post2.mobile = "";
                post2.profileimage = !user.ProfileImage.Contains("https://") ? Url.Content("~/Images/Users/") + user.ProfileImage + ".jpg" : user.ProfileImage;
                post2.baseProperty = BaseProperty;

                List<CommentModel> comments = (from x in db.Comments
                                               where
                                               x.postId == postid &&
                                               x.state == true
                                               select new CommentModel
                                               {
                                                   userName = x.userName,
                                                   comment = x.comment
                                               }).ToList();

                List<string> Comments = new List<string>();
                for (int i = 0; i < comments.Count(); i++)
                    Comments.Add(comments[i].userName + " : " + comments[i].comment);

                post2.comments = Comments;

                return new HttpResponseMessage()
                {
                    Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(post2), Encoding.UTF8, "application/json")
                };
            }
            else
                return null;
        }

        public string PersianNumber(string s)
        {
            try
            {
                s = s.Replace("0", "٠")
                .Replace("1", "۱")
                .Replace("2", "۲")
                .Replace("3", "٣")
                .Replace("4", "۴")
                .Replace("5", "۵")
                .Replace("6", "٦")
                .Replace("7", "٧")
                .Replace("8", "٨")
                .Replace("9", "۹");
            }
            catch (Exception e) { }


            return s;
        }
    }
}
