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
    public class InsertCommentController : ApiController
    {
        PayeDBEntities db = new PayeDBEntities();

        //[SanatyarWebCms.CustomExceptionFilter]
        public HttpResponseMessage Post()
        {

            var httpRequest = HttpContext.Current.Request;
            var UserId = httpRequest.Form.Get("UserId").Trim();
            var Comment = httpRequest.Form.Get("Comment").Trim();
            var PostId = httpRequest.Form.Get("PostId").Trim();
            var UserName = httpRequest.Form.Get("UserName").Trim();

            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Comment))
                throw new BusinessException("خطا در پارامترهای ورودی");

            var responseType = System.Net.HttpStatusCode.OK;
            var res = "";
            try
            {
                Comment tb = new Comment();
                tb.userId = db.Users.FirstOrDefault(r => r.UserId.ToString() == UserId).Id;
                tb.postId = Guid.Parse(PostId);
                tb.userName = UserName;
                tb.comment = Comment;
                tb.createDate = DateTime.Now;
                tb.state = true;
                db.Comments.Add(tb);
                db.SaveChanges();
                //res = "با موفقیت ثبت شد";
                return new HttpResponseMessage(responseType)
                {
                    Content =
                    new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(tb), Encoding.UTF8)
                };
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