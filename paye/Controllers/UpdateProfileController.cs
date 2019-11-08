using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Net.Http.Formatting;
using System.Linq;
using Paye.Models;
using System;
using System.Web;

namespace Paye.Controllers
{
    public class UpdateProfileController : ApiController
    {            
        //[ResponseType(typeof(Activity))]
        public HttpResponseMessage Post()
        {
            PayeDBEntities db = new PayeDBEntities();
            var httpRequest = HttpContext.Current.Request;
            var UserId = httpRequest.Form.Get("UserId");
            var Name = httpRequest.Form.Get("Name");
            var Family = httpRequest.Form.Get("Family");
            var Telegram = httpRequest.Form.Get("Telegram");
            var Instagram = httpRequest.Form.Get("Instagram");
            var Soroosh = httpRequest.Form.Get("Soroosh");
            var Gmail = httpRequest.Form.Get("Gmail");
            var IsShowMobile = httpRequest.Form.Get("IsShowMobile");
            var Age = httpRequest.Form.Get("Age");
            var City = httpRequest.Form.Get("City");
            var AboutMe = httpRequest.Form.Get("AboutMe");
            var Favorites = httpRequest.Form.Get("Favorites");

            var list = db.Users.FirstOrDefault(x => x.UserId.ToString() == UserId);

            list.Name = !string.IsNullOrEmpty(Name) ? Name : list.Name;
            list.Family = !string.IsNullOrEmpty(Family) ? Family : list.Family;
            list.City = !string.IsNullOrEmpty(City) ? City : list.City;
            list.Age = !string.IsNullOrEmpty(Age) ? Age : list.Age;
            list.Favorites = !string.IsNullOrEmpty(Favorites) ? Favorites : (string.IsNullOrEmpty(list.Favorites) ? "" : list.Favorites);
            list.AboutMe = !string.IsNullOrEmpty(AboutMe) ? AboutMe : (string.IsNullOrEmpty(list.AboutMe) ? "" : list.AboutMe);
            list.Telegram = !string.IsNullOrEmpty(Telegram) ? Telegram : (string.IsNullOrEmpty(list.Telegram) ? "" : list.Telegram);
            list.Gmail = !string.IsNullOrEmpty(Gmail) ? Gmail : (string.IsNullOrEmpty(list.Gmail) ? "" : list.Gmail);
            list.Instagram = !string.IsNullOrEmpty(Instagram) ? Instagram : (string.IsNullOrEmpty(list.Instagram) ? "" : list.Instagram);
            list.IsShowMobile = Convert.ToBoolean(IsShowMobile);
            list.Soroosh = !string.IsNullOrEmpty(Soroosh) ? Soroosh : (string.IsNullOrEmpty(list.Soroosh) ? "" : list.Soroosh);
            list.ModifiedDate = DateTime.Now;
            db.SaveChanges();

            //Business.Expert.ExpertPersonBiz.Instance.UpdateRefreshToken(userid, token);
            return new HttpResponseMessage()
            {
                Content =
                    new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject("عملیات با موفقیت انجام شد"), Encoding.UTF8, "application/json")
            };
        }
    }
}
