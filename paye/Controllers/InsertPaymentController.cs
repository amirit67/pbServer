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
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
//using BaseSystemModel.Helper;
//using BusinessEmdadExpert.Expert;

namespace Paye.Controllers
{
    public class InsertPaymentController : ApiController
    {
        //[SanatyarWebCms.CustomExceptionFilter]
        public HttpResponseMessage Post()
        {
            Paye.Models.PayeDBEntities db = new Paye.Models.PayeDBEntities();
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Headers["PayeBash"] != null)
            {
                var postid = httpRequest.Form.Get("PostId");
                var userid = httpRequest.Form.Get("UserId");
                var refID = httpRequest.Form.Get("refID");
                var Amount = httpRequest.Form.Get("Amount");
                var TypeOfPay = httpRequest.Form.Get("TypeOfPay");


                if (postid != null)
                {
                    Payment tb = new Payment();
                    tb.UserId = Guid.Parse(userid);
                    tb.PostId = Guid.Parse(postid);
                    tb.refID = refID == null ? "" : refID.Trim();
                    tb.Amount = Amount.Trim();
                    tb.CreateDate = DateTime.Now;
                    db.Payments.Add(tb);
                    db.SaveChanges();

                                   
                    var list = (from x in db.Posts
                                where x.postId.ToString() == postid
                                select x).FirstOrDefault();
                    list.state = 2;
                    db.SaveChanges();
                }

                return new HttpResponseMessage()
                {
                    Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(postid), Encoding.UTF8, "application/json")
                };
            }
            else
                throw new BusinessException("خطا در پارامترهای ورودی");
        }
    }
}