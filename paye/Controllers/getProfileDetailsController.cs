using BaseSystemModel.Helper;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Linq;
using System.Web.Http;
using Paye.Models;

namespace Sanatyar.EmdadExpert.Controller.WebApiControllers
{
    public class getProfileDetailsController : ApiController
    {
        public HttpResponseMessage Post(Trust trust)
        {
            PayeDBEntities db = new PayeDBEntities();
            if (trust.VoteReciverUserId != null)
            {
                //var res = new BaseSystemModel.ApiResponse { Type = 0 };                         
                var VoteReciverUserId = db.Users.FirstOrDefault(r => r.UserId.ToString() == trust.VoteReciverUserId).Id.ToString();
                var item = db.Users.FirstOrDefault(i => i.UserId.ToString() == (null == trust.VoteReciverUserId ? trust.VoterUserId : trust.VoteReciverUserId));
                try { item.TrustedVoteCount = db.TrustVotes.Where(j => j.VoteReciverUserId.ToString() == VoteReciverUserId && j.State == true).Count().ToString(); }
                catch (Exception e) { item.TrustedVoteCount = "0"; }
                TimeSpan t = (TimeSpan)(DateTime.Now - item.CreateDate);
                item.UserAge = ((int)t.TotalDays).ToString();
                item.ActivityState = db.Posts.Where(i => i.userId.ToString() == VoteReciverUserId).Count().ToString() + "," + db.Posts.Where(i => i.applicants.Contains("," + item.Id + ",")).Count().ToString();
                var VoterUserId = db.Users.Where(r => r.UserId.ToString() == trust.VoterUserId).FirstOrDefault().Id.ToString();
                //VoteReciverUserId = db.Users.Where(r => r.UserId.ToString() == trust.VoteReciverUserId).FirstOrDefault().Id.ToString();
                try { item.IsTrust = (bool)db.TrustVotes.FirstOrDefault(j => j.VoteReciverUserId.ToString() == VoteReciverUserId && j.VoterUserId.ToString() == VoterUserId).State; }
                catch (Exception e) { item.IsTrust = false; }
                return new HttpResponseMessage()
                {
                    Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json")
                };
            }
            else
            {
                var item = db.Users.FirstOrDefault(i => i.UserId.ToString() == (null == trust.VoterUserId ? trust.VoterUserId : trust.VoterUserId));
                return new HttpResponseMessage()
                {
                    Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json")
                };
            }


        }

        public class Trust
        {
            public string VoteReciverUserId { get; set; }
            public string VoterUserId { get; set; }
        }
    }
}
