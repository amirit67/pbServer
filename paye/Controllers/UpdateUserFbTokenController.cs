using Paye.Models;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
//using BaseSystemModel.Helper;
//using BusinessEmdadExpert.Expert;

namespace Paye.Controllers
{
    public class UpdateUserFbTokenController : ApiController
    {
        //[SanatyarWebCms.CustomExceptionFilter]
        public HttpResponseMessage Post()
        {
            var httpRequest = HttpContext.Current.Request;
            var FbToken = httpRequest.Form.Get("FbToken").Trim();
            var userid = httpRequest.Form.Get("UserId").Trim();
          
            Paye.Models.PayeDBEntities db = new Paye.Models.PayeDBEntities();
            var list = db.Users.FirstOrDefault(x => x.UserId.ToString() == userid);
            list.Token = FbToken.Trim();               
            db.SaveChanges();
           
            return new HttpResponseMessage()
            {
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(list.ServicesIds), Encoding.UTF8, "application/json")
            };
        }
    }
}