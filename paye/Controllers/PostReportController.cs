using Paye.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;

namespace Paye.Controllers
{
    public class PostReportController : ApiController
    {

        // POST: api/Report
        public string Post(ReportWrapper report)
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Headers["PayeBash"] != null)
            {
                PayeDBEntities db = new PayeDBEntities();
                if (report.ComplainantId != null &&
                    report.Type != null)
                {                   
                    var complainantId = db.Users.FirstOrDefault(i => i.UserId.ToString() == report.ComplainantId).Id;
                    /*long*/string userId = "0";
                    long postId = 0;
                    int cnt = 0;
                    if (!string.IsNullOrEmpty(report.UserId))
                    {
                        userId = db.Users.FirstOrDefault(i => i.UserId.ToString() == report.UserId).Id;
                        cnt = db.ReportPosts.Where(i => i.ComplainantId == complainantId && i.UserId == userId).Count();
                    }
                        
                    else if (!string.IsNullOrEmpty(report.PostId))
                    {
                        postId = db.Posts.Where(r => r.postId.ToString() == report.PostId).FirstOrDefault().Id;
                        cnt = db.ReportPosts.Where(i => i.ComplainantId == complainantId && i.PostId == postId).Count();
                    }
                        

                    if (cnt > 0)
                    {
                        var record = db.ReportPosts.FirstOrDefault(i => i.ComplainantId == complainantId && (i.PostId == postId || i.UserId == userId));
                        record.Type = report.Type;
                        record.Modifiedate = DateTime.Now;
                        db.Entry(record).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return "گزارش شما با موفقیت ثبت گردید";
                    }
                    else
                    {
                        try
                        {
                            ReportPost tb = new ReportPost();
                            tb.ComplainantId = complainantId;
                            tb.PostId = postId;
                            tb.UserId = userId;
                            tb.Type = report.Type;
                            tb.Status = false;
                            tb.Modifiedate = DateTime.Now;
                            db.ReportPosts.Add(tb);
                            db.SaveChanges();

                            return "گزارش شما با موفقیت ثبت گردید"; ;
                        }
                        catch (Exception ex)
                        {
                            return "خطا در ارسال";
                        }
                    }
                }
                else
                    return "خطا در ارسال";
            }
            return null;
        }
    }
}
