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
    public class UpdateUserController : ApiController
    {
        PayeDBEntities db = new PayeDBEntities();

        //[SanatyarWebCms.CustomExceptionFilter]
        public HttpResponseMessage Post(UserItem user)
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
                        var applicant = ctx.Users.FirstOrDefault(i => (!string.IsNullOrEmpty(user.UserId) && i.UserId.ToString() == user.UserId));
                        if (applicant == null)
                            applicant = ctx.Users.FirstOrDefault(i => (!string.IsNullOrEmpty(user.Email) && i.Gmail == user.Email));
                        if (applicant == null)
                            applicant = ctx.Users.FirstOrDefault(i => (!string.IsNullOrEmpty(user.Mobile) && i.Mobile == user.Mobile));

                        
                        if (applicant == null)
                            throw new BusinessException("لطفا ابتدا عضو شوید");
                        
                        else
                        {
                            if (!string.IsNullOrEmpty(user.Token))
                                applicant.Token = user.Token;
                            if (!string.IsNullOrEmpty(user.Name))
                                applicant.Name = user.Name;
                            if (!string.IsNullOrEmpty(user.Family))
                                applicant.Family = user.Family;
                            if (!string.IsNullOrEmpty(user.City))
                                applicant.City = user.City;
                            if (!string.IsNullOrEmpty(user.Age))
                                applicant.Age = user.Age;
                            if (!string.IsNullOrEmpty(user.Email))
                                applicant.Gmail = user.Email;
                            if (!string.IsNullOrEmpty(user.Mobile))
                                applicant.Mobile = user.Mobile;
                            //if (applicant.IsAuthenticate)
                            //{
                            applicant.ModifiedDate = DateTime.Now;
                            ctx.Entry(applicant).State = System.Data.Entity.EntityState.Modified;
                            ctx.SaveChanges();
                            //}
                            id = applicant.UserId.ToString();
                            res = id;
                        }

                    }
                }
                catch (Exception e)
                {
                    if(e.InnerException != null)
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

        [HttpGet]
        public HttpResponseMessage Get(string id)
        {
            var path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory + "/Images/Paye/"+id+".jpg");//Server.MapPath("~/Images/Bisun/Users/user_empty.png");
           var result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream2 = new FileStream(path, FileMode.Open, FileAccess.Read);
            result.Content = new StreamContent(stream2);
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");
            return result;
        }

        public class UserItem
        {
            public string UserId { get; set; }
            public string Name { get; set; }
            public string Family { get; set; }
             public string Mobile { get; set; }
             public string City { get; set; }
             public string Age { get; set; }
             public string Token { get; set; }            
            public string Type { get; set; }
            public string SmsCode { get; set; }
            public string Sign { get; set; }
            public string Email { get; set; }        
            public string Aboutme { get; set; }
            public string Images { get; set; }
            
        }


        public static Image ResizeImageByMinRatio(Image image, int minWidth, int minHeight)
        {
            var ratioX = (double)minWidth / image.Width;
            var ratioY = (double)minHeight / image.Height;
            var ratio = Math.Max(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);
            var destRect = new Rectangle(0, 0, newWidth, newHeight);
            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return newImage;
        }

    }
}