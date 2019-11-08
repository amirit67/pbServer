using BaseSystemModel.Helper;
using Paye.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
//using BaseSystemModel.Helper;
//using BusinessEmdadExpert.Expert;

namespace Paye.Controllers
{
    public class PostFeedbackController : ApiController
    {
        PayeDBEntities db = new PayeDBEntities();

        //[SanatyarWebCms.CustomExceptionFilter]
        public HttpResponseMessage Post()
        {

            var httpRequest = HttpContext.Current.Request;
            var UserId = httpRequest.Form.Get("UserId").Trim();
            var Message = httpRequest.Form.Get("Message").Trim();


            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Message))
                throw new BusinessException("خطا در پارامترهای ورودی");

            var responseType = HttpStatusCode.OK;
            var res = "";
            try
            {
                PayeDBEntities db = new PayeDBEntities();

                if (Message != null)
                {
                    FeedbackSuggestion tb = new FeedbackSuggestion();
                    tb.UserId = db.Users.FirstOrDefault(r => r.UserId.ToString() == UserId).Id.ToString();
                    tb.Message = Message;
                    tb.CreateDate = DateTime.Now;
                    tb.Status = false;
                    db.FeedbackSuggestions.Add(tb);
                    db.SaveChanges();
                    res = "با موفقیت ثبت شد";
                }
            }
            catch (Exception e)
            {
                res = e.Message;
                responseType = System.Net.HttpStatusCode.InternalServerError;

                if (res == "خطا در اطلاعات ورودی!")
                    responseType = System.Net.HttpStatusCode.Forbidden;
            }


            return new HttpResponseMessage(responseType)
            {
                Content =
                    new StringContent(res, Encoding.UTF8)
            };

        }
    }
}