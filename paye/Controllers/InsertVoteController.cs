
using BaseSystemModel.Helper;
using Paye.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
//using BaseSystemModel.Helper;
//using BusinessEmdadExpert.Expert;

namespace Paye.Controllers
{
    public class InsertVoteController : ApiController
    {
        PayeDBEntities db = new PayeDBEntities();
    
        public HttpResponseMessage Post()
        {

            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Headers["PayeBash"] != null)
            {
                var VoterUserId = httpRequest.Form.Get("VoterUserId");
                var VoteReciverUserId = httpRequest.Form.Get("VoteReciverUserId");

                if (string.IsNullOrEmpty(VoterUserId) || string.IsNullOrEmpty(VoteReciverUserId))
                    throw new BusinessException("خطا در پارامترهای ورودی");

                VoterUserId = db.Users.Where(r => r.UserId.ToString() == VoterUserId).FirstOrDefault().Id.ToString().Trim();
                VoteReciverUserId = db.Users.FirstOrDefault(r => r.UserId.ToString() == VoteReciverUserId).Id.ToString().Trim();

                var res = false;

                try
                {
                    var record = db.TrustVotes.Where(i => i.VoterUserId.ToString() == VoterUserId && i.VoteReciverUserId.ToString() == VoteReciverUserId).FirstOrDefault();
                    if (record != null)
                    {
                        if (record.State == true)
                        {
                            record.State = false;
                            res = false;
                        }
                        else if (record.State == false)
                        {
                            record.State = true;
                            res = true;
                        }

                        record.ModeifidDate = DateTime.Now;
                        db.Entry(record).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        TrustVote tb = new TrustVote();
                        tb.VoterUserId = Convert.ToInt64(VoterUserId);
                        tb.VoteReciverUserId = Convert.ToInt64(VoteReciverUserId);
                        tb.State = true;
                        tb.ModeifidDate = DateTime.Now;

                        db.TrustVotes.Add(tb);
                        db.SaveChanges();
                        res = true;
                    }
                }
                catch (Exception e)
                {
                    return new HttpResponseMessage()
                    {
                        Content =
                            new StringContent(e.Message, Encoding.UTF8)
                    };
                }
                return new HttpResponseMessage()
                {
                    Content =
                        new StringContent(res.ToString(), Encoding.UTF8)
                };
            }
            return null;

        }
    }
}