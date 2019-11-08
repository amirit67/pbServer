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
    public class InsertPostController : ApiController
    {
        //[SanatyarWebCms.CustomExceptionFilter]
        public HttpResponseMessage Post()
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Headers["PayeBash"] != null)
            {
                var postid = httpRequest.Form.Get("postId");
                var userid = httpRequest.Form.Get("userId");
                int subject = Convert.ToInt32(httpRequest.Form.Get("subject"));
                var title = httpRequest.Form.Get("title");
                var description = httpRequest.Form.Get("description");
                int city = Convert.ToInt32(httpRequest.Form.Get("city"));
                var isWoman = httpRequest.Form.Get("isWoman");
                var isImmediate = httpRequest.Form.Get("isImmediate");
                var phoneNumber = httpRequest.Form.Get("phoneNumber");
                var link = httpRequest.Form.Get("link");
                var cost = httpRequest.Form.Get("cost");
                var numberFollowers = httpRequest.Form.Get("numberFollowers");
                var startDate = httpRequest.Form.Get("startDate");
                var endDate = httpRequest.Form.Get("endDate");
                var deadline = httpRequest.Form.Get("timeToJoin");
                var tag = httpRequest.Form.Get("tag");
                var longitude = httpRequest.Form.Get("longitude");
                var latitude = httpRequest.Form.Get("latitude");
                var isCommercial = httpRequest.Form.Get("isCommercial");

                string imagesName = httpRequest.Form.Get("images");
                foreach (string postedFile in httpRequest.Files)
                {
                    var file = httpRequest.Files[postedFile];
                    if (file.ContentLength > 0 && !string.IsNullOrEmpty(file.FileName) && file.ContentType.Contains("image"))
                    {
                        var image = Image.FromStream(file.InputStream);

                        var dir = HttpContext.Current.Server.MapPath("~/Images/PayeBash/");
                        var dirThumbnail = HttpContext.Current.Server.MapPath("~/Images/PayeBash/Thumbnail/");

                        Random rnd = new Random();
                        var imageName = DateTime.Now.Ticks;

                        var bmp = ResizeImageByMinRatio(image, 150, 150);
                        bmp.Save(dirThumbnail + imageName + ".jpg", ImageFormat.Jpeg);

                        var bmp2 = ResizeImageByMinRatio(image, 512, 512);
                        bmp2.Save(dir + imageName + ".jpg", ImageFormat.Jpeg);

                        imagesName += imageName + ",";
                    }
                }
                PayeDBEntities db = new PayeDBEntities();

                if (postid == null)
                {
                    Post tb = new Post();
                    var user = db.Users.Where(r => r.UserId.ToString() == userid).ToList().FirstOrDefault();
                    tb.userId = user.Id;                    
                    tb.title = title.Trim();
                    tb.description = description == null ? "" : description.Trim();
                    tb.subject = subject;
                    tb.city = city;
                    tb.isWoman = Convert.ToBoolean(isWoman);
                    tb.isImmediate = Convert.ToBoolean(isImmediate);
                    tb.phoneNumber = phoneNumber == null ? "" : phoneNumber.Trim();
                    tb.link = link == null ? "" : link.Trim();
                    tb.cost = cost == null ? "" : cost.Trim();
                    tb.numberFollowers = numberFollowers == null ? "" : numberFollowers.Trim();
                    tb.images = imagesName == null ? "" : imagesName.Trim();
                    tb.createDate = DateTime.Now;
                    tb.applicants = "";
                    tb.createDate = DateTime.Now;
                    tb.startDate = startDate;
                    tb.endDate = endDate;
                    tb.timeToJoin = Convert.ToDateTime(deadline);
                    tb.tag = tag == null ? "" : tag.Trim();
                    tb.longitude = longitude == null ? "" : longitude.Trim();
                    tb.latitude = latitude == null ? "" : latitude.Trim();
                    /*if (!(bool)user.IsMobileAuthenticate)
                    {
                        tb.State = 22;
                    }
                        
                    else*/ 
                    if(isCommercial == "true")
                        tb.service = Models.Post.State_Pay_Category;
                    if (isImmediate == "true")
                        tb.service = Models.Post.State_Pay_Immadiate;

                    tb.state = Models.Post.State_New;


                    db.Posts.Add(tb);
                    db.SaveChanges();
                }
                else
                {
                    var list = db.Posts.FirstOrDefault(x => x.postId.ToString() == postid);
                    list.state = Models.Post.State_Edit;

                    list.title = title.Trim();
                    list.description = description == null ? "" : description.Trim();
                    //list.Subject = subject.Trim();
                    list.city = city;
                    list.isWoman = Convert.ToBoolean(isWoman);
                    if (list.isImmediate != true)
                        list.isImmediate = Convert.ToBoolean(isImmediate);
                    list.phoneNumber = phoneNumber == null ? "" : phoneNumber.Trim();
                    list.link = link == null ? "" : link.Trim();
                    list.cost = cost == null ? "" : cost;
                    list.numberFollowers = numberFollowers == null ? "" : numberFollowers.Trim();
                    list.images = imagesName == null ? "" : imagesName.Trim();
                    //list.Applicants = list.Applicants;
                    list.startDate = startDate;
                    list.endDate = endDate;
                    list.timeToJoin = Convert.ToDateTime(deadline);
                    list.tag = tag == null ? "" : tag.Trim();
                    list.longitude = longitude == null ? "" : longitude.Trim();
                    list.latitude = latitude == null ? "" : latitude.Trim();                                      
                    list.modifiedDate = DateTime.Now;

                    db.SaveChanges();
                }


                //System.Collections.Generic.List<returnPost> map = new System.Collections.Generic.List<returnPost>();
                returnPost item = new returnPost();
                var a = db.Posts
                           .OrderByDescending(p => p.Id)
                           .FirstOrDefault();

                item.postId = a.postId.ToString();
                try
                {
                    if ("" == a.images.ToString().Split(',')[0])
                        item.postImage = "null";
                    else
                        item.postImage = Url.Content("~/Images/Paye/") + a.images.ToString().Split(',')[0];
                }
                catch (Exception e)
                {
                    string s = e.Message;
                    item.postImage = "null";
                }
                //map.Add(item);

                return new HttpResponseMessage()
                {
                    Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json")
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