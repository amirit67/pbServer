using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Net.Http.Formatting;
using System.Linq;
using Paye.Models;
using System;

namespace Paye.Controllers
{
    public class PostRefreshTokenController : ApiController
    {            
        //[ResponseType(typeof(Activity))]
        public HttpResponseMessage Post([FromBody] FormDataCollection formDataCollection)
        {
            var userId = formDataCollection.Get("UserId").Trim();
            var token = formDataCollection.Get("RefreshToken").Trim();

            PayeDBEntities db = new PayeDBEntities();

            var list = (from x in db.Users
                        where x.UserId == Guid.Parse(userId)
                        select x).First();

            list.Token = token;
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
