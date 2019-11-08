
using BaseSystemModel.Utilty;
using Paye.Models;
using BaseSystemModel.Helper;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Paye.Controllers
{
    public class getPostDetailsUpdateController : ApiController
    {       
        public HttpResponseMessage Get(string id)
        {
           
            string PostCode = "";
            if (null != id)
                PostCode = id;          

            PayeDBEntities db = new PayeDBEntities();
            var post = db.Posts.FirstOrDefault(x => x.postId.ToString() == PostCode);

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



            post.PersianStartDate = post.startDate.ToString();
            post.PersianFinishDate = post.endDate.ToString();
            post.PersianDeadline = Utilty.ToPersianDateTime(Convert.ToDateTime(post.timeToJoin.ToString())).ToString().Substring(2, 14);
            
            return new HttpResponseMessage()
            {
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json")
            };
        }
        
        public string PersianNumber(string s)
        {
            try {
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
            } catch (Exception e1) { }
           

            return s;
        }      
    }
}
