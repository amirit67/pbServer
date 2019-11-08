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
    public class GetMyServicesController : ApiController
    {
        //[SanatyarWebCms.CustomExceptionFilter]
        public HttpResponseMessage Post(ParamsWrapper paramsWrapper)
        {
            var httpRequest = HttpContext.Current.Request;
            var ServicesIds = httpRequest.Form.Get("ServicesIds").Trim();
            var userid = httpRequest.Form.Get("UserId").Trim();
            var token = httpRequest.Form.Get("Token").Trim();
            PayeDBEntities db = new PayeDBEntities();
            var list = db.Users.FirstOrDefault(x => x.UserId.ToString() == userid);
            if(!string.IsNullOrEmpty(token))
                list.Token = token;
            list.ServicesIds = ServicesIds.Trim();               
            db.SaveChanges();
           
            return new HttpResponseMessage()
            {
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(list.ServicesIds), Encoding.UTF8, "application/json")
            };
        }


        public HttpResponseMessage Get(string id)
        {
            var httpRequest = HttpContext.Current.Request;           
            PayeDBEntities db = new PayeDBEntities();
            var list = db.Users.FirstOrDefault(x => x.UserId.ToString() == id);

            return new HttpResponseMessage()
            {
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(list.ServicesIds), Encoding.UTF8, "application/json")
            };
        }     
    }
}