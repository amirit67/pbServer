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
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
//using BaseSystemModel.Helper;
//using BusinessEmdadExpert.Expert;

namespace Paye.Controllers
{
    public class RegisterController : ApiController
    {
        PayeDBEntities db = new PayeDBEntities();

        //[SanatyarWebCms.CustomExceptionFilter]
        public HttpResponseMessage Post(User user)
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Headers["PayeBash"] != null)
            {


                if (string.IsNullOrEmpty(user.Email))
                    if (string.IsNullOrEmpty(user.Mobile))
                        throw new BusinessException("خطا در پارامترهای ورودی");

                var responseType = HttpStatusCode.OK;
                var res = "";

                string id;
                var r = new Random();
                var smsCode = r.Next(111111, 999999);
                try
                {
                    using (var ctx = new PayeDBEntities())
                    {
                        var applicant = ctx.Users.FirstOrDefault(i => (!string.IsNullOrEmpty(user.UserId.ToString()) && i.UserId == user.UserId));
                        if (applicant == null)
                            applicant = ctx.Users.FirstOrDefault(i => (!string.IsNullOrEmpty(user.Email) && i.Gmail == user.Email));
                        if (applicant == null)
                            applicant = ctx.Users.FirstOrDefault(i => (!string.IsNullOrEmpty(user.Mobile) && i.Mobile == user.Mobile));

                         
                        if (applicant != null)
                            throw new BusinessException("شما قبل عضو شده اید، وارد شوید");
                        else
                        {
                            var sms = ctx.Sms.OrderByDescending(i => i.createdate).FirstOrDefault(i => i.userId == applicant.Id);
                            TimeSpan span = DateTime.Now.Subtract(Convert.ToDateTime(sms.createdate));
                            if (span.TotalSeconds < 120)
                                throw new BusinessException("برای ارسال مجدد پیام لطفا 2 دقیقه منتظر بمانید");

                            else
                            {
                                Sms smsUser = new Sms();
                                smsUser.userId = applicant.Id;
                                smsUser.sms = char.Parse(smsCode.ToString());
                                smsUser.createdate = DateTime.Now;
                                ctx.Sms.Add(smsUser);

                                db.Users.Add(user);
                                db.SaveChanges();

                                id = db.Users
                                .OrderByDescending(p => p.Id).ToList()
                                .FirstOrDefault().UserId.ToString();
                                res = id;

                                SendSms.SendSimpleSms2(user.Mobile, "کد تایید ورود شما در پایه باش : " + smsCode);
                            }


                        }
                    }
                }
                catch (Exception e)
                {
                    if (e.InnerException != null)
                    {
                        res = e.InnerException.Message;
                    }
                    else
                        res = e.Message;
                    responseType = System.Net.HttpStatusCode.InternalServerError;

                    if (res == "برای ارسال مجدد پیام لطفا 2 دقیقه منتظر بمانید")
                        responseType = System.Net.HttpStatusCode.ExpectationFailed;
                    if (res == "شما قبل عضو شده اید، وارد شوید")
                        responseType = System.Net.HttpStatusCode.Forbidden;

                    if (res == "لطفا ابتدا عضو شوید")
                        responseType = System.Net.HttpStatusCode.BadRequest;
                }


                return new HttpResponseMessage(responseType)
                {
                    Content =
                        new StringContent(res, Encoding.UTF8)
                };
            }
            else
                return null;

        }

    }
}