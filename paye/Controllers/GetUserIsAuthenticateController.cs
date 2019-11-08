using BaseSystemModel;
using BaseSystemModel.Helper;
using Paye.Models;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
//using BaseSystemModel.Helper;
//using BusinessEmdadExpert.Expert;

namespace Paye.Controllers
{
    public class GetUserIsAuthenticateController : ApiController
    {
        PayeDBEntities db = new PayeDBEntities();

        //[SanatyarWebCms.CustomExceptionFilter]
        public HttpResponseMessage Post([FromBody] FormDataCollection formDataCollection)
        {
            string s = formDataCollection.Get("UserId").ToString().Trim();
            string token = formDataCollection.Get("FbToken").ToString().Trim();
            var record = db.Users.FirstOrDefault(i => i.UserId.ToString() == s);
            record.Token = token;
            db.SaveChanges();
            returnUser item = new returnUser();
            item.FullName = record.Name.ToString() + " " + record.Family.ToString();
            item.IsAuthenticate = record.IsAuthenticate.ToString();
            item.ProfileImage = record.ProfileImage;
            item.ServicesIds = record.ServicesIds;
            item.Instagram = record.Instagram;
            item.Telegram = record.Telegram;
            item.Soroosh = record.Soroosh;
            if ((bool)record.IsMobileAuthenticate)
            {
                item.Mobile = record.Mobile;
                item.MobileTemp = record.Mobile;
            }
                
            else
            {
                item.Mobile = "";
                item.MobileTemp = record.Mobile;
            }
            return new HttpResponseMessage()
            {
                Content =
                    new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(item), Encoding.UTF8)
            };

        }
    }
}