
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
    public class InsertImageEventController : ApiController
    {
        PayeDBEntities db = new PayeDBEntities();

        //[SanatyarWebCms.CustomExceptionFilter]
        public HttpResponseMessage Post()
        {

            var httpRequest = HttpContext.Current.Request;


            var postedFile = httpRequest.Files[0];
            var image = Image.FromStream(postedFile.InputStream);
            
            var dir = HttpContext.Current.Server.MapPath("~/Images/PayeBash/");
            var dirThumbnail = HttpContext.Current.Server.MapPath("~/Images/PayeBash/Thumbnail/");

            Random rnd = new Random();
            var imageName = DateTime.Now.Ticks;

            var bmp = ResizeImageByMinRatio(image, 150, 150);
            bmp.Save(dirThumbnail + imageName + ".jpg", ImageFormat.Jpeg);

            var bmp2 = ResizeImageByMinRatio(image, 512, 512);
            bmp2.Save(dir + imageName + ".jpg", ImageFormat.Jpeg);

            return new HttpResponseMessage()
            {
                Content = new StringContent(imageName.ToString())
            };

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