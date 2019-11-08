using BaseSystemModel.Helper;
using Paye.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Paye.Controllers
{
    public class getWebPostDetailsController : Controller
    {
        // GET: getWebPostDetails
        public ActionResult Index(string id)
        {
            PayeDBEntities db = new Paye.Models.PayeDBEntities();
            var post = (from x in db.Posts
                        where x.postId.ToString() == id
                        select x).ToList().FirstOrDefault();

            List<string> BaseProperty = new List<string>();
            if (!string.IsNullOrEmpty(post.phoneNumber))
                BaseProperty.Add("شماره تماس : " + PersianNumber(post.phoneNumber).Trim() + "\n");
            if (!string.IsNullOrEmpty(post.link))
                BaseProperty.Add("وب سایت : " + PersianNumber(post.link).Trim() + "\n");
            BaseProperty.Add("هزینه : " + PersianNumber(post.cost).Trim() + "\n");
            BaseProperty.Add("تعداد هم پایه : " + PersianNumber(post.numberFollowers) + "\n");
            BaseProperty.Add("تاریخ شروع  : " + post.startDate.ToString() + "\n");
            BaseProperty.Add("تاریخ پایان : " + post.endDate.ToString() + "\n");
            BaseProperty.Add("مهلت هم پا شدن : " + BaseSystemModel.ResizeImage.GetDateDifferencesAsDescription2(DateTime.Now, Convert.ToDateTime(post.timeToJoin.ToString()), 0) + "\n");
            BaseProperty.Add("هشتگ : " + post.tag.Trim() + "\n");
            //BaseProperty.Add("راه های ارتباطی : " + post.ContactWays.Trim());
            BaseProperty.Add("توضیحات : " + PersianNumber(post.description) + "\n");

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
            post2.state = Dictioanry.GetStatesPayePost[(byte)post.state].ToString()
                + "-" + Dictioanry.GetStatesDescriptionPayePost[(byte)post.state].ToString()
                + "-" + Dictioanry.GetStatesColorPayePost[(byte)post.state].ToString();
            var user = db.Users.Where(r => r.Id == post.userId).FirstOrDefault();
            post2.username = user.Name.Trim() + " " + user.Family.Trim();
            post2.token = user.Token.Trim();
            if (user.IsShowMobile != null && (bool)user.IsShowMobile)
                post2.mobile = user.Mobile.Trim();
            post2.profileimage = Url.Content("~/Images/Users/") + user.ProfileImage + ".jpg";
            post2.baseProperty = BaseProperty;


            return View(post2);
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