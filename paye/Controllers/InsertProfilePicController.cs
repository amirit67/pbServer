using Paye.Models;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Linq;
//using BaseSystemModel.Helper;
//using Sanatyar.EmdadExpert.Business.Expert;

namespace Paye.Controllers
{
    public class InsertProfilePicController : ApiController
    {
        //[SanatyarWebCms.CustomExceptionFilter]
        
        public HttpResponseMessage Post()
        {
            var res = "";
            try
            {
                var httpRequest = HttpContext.Current.Request;

                var userId = httpRequest.Form.Get("UserId").Trim();

                var postedFile = httpRequest.Files[0];
                var image = Image.FromStream(postedFile.InputStream);

                var dir = HttpContext.Current.Server.MapPath("~/Images/Users/");

                Random rnd = new Random();
                var imageName = DateTime.Now.Ticks + rnd.Next(10000, 99999);

                string imagesName = "";
                var bmp2 = ResizeImageByMinRatio(image, 200, 200);
                bmp2.Save(dir + imageName + ".jpg", ImageFormat.Jpeg);
                imagesName += imageName;

                PayeDBEntities db = new PayeDBEntities();
                var list = db.Users.FirstOrDefault(x => x.UserId.ToString() == userId);

                list.ProfileImage = imagesName;
                db.SaveChanges();
                return new HttpResponseMessage()
                {
                    Content = new StringContent(imageName.ToString())
                };
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    res = e.InnerException.Message;
                }
                else
                    res = e.Message;
                return new HttpResponseMessage()
                {
                    Content = new StringContent(res.ToString())
                };
            }            
           

                     
        }
        /*[HttpGet]
        public HttpResponseMessage Get(long id)
        {
          
            PayeDBEntities db = new PayeDBEntities();
            
            var img = db.Users.Where(p => p.ProfileImage == id.ToString()).FirstOrDefault().ProfileImage;
            HttpResponseMessage result;
            if (img != null && img.Length > 0)
            {
                var stream = new MemoryStream();
                stream.Write(img, 0, img.Length);
                result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(stream.ToArray())
                };
                result.Content.Headers.ContentDisposition =
                    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "Profile.jpg"
                    };
                result.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");
                return result;
            }
            var path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory + "/Images/Paye/636382241011527886.jpg");//Server.MapPath("~/Images/Bisun/Users/user_empty.png");
            result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream2 = new FileStream(path, FileMode.Open, FileAccess.Read);
            result.Content = new StreamContent(stream2);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
           
            return result;
        }*/




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