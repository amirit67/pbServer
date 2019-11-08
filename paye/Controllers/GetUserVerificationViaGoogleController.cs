using System;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Web.Http;
using Paye.Models;

namespace Sanatyar.EmdadExpert.Controller.WebApiControllers
{
    public class GetUserVerificationViaGoogleController : ApiController
    {
        public HttpResponseMessage Post(UserItem user)
        {
            PayeDBEntities db = new PayeDBEntities();

            //var res = new BaseSystemModel.ApiResponse { Type = 0 };
            try
            {               
                returnUser r = new returnUser();
                var item = db.Users.FirstOrDefault(i => /*i.Name == GN && i.Family == FN &&*/ i.Gmail == user.Email);
                if (item != null)
                {
                    //item.IsAuthenticate = true;
                    item.Token = user.Token;
                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    r.UserId = item.UserId.ToString();
                    r.FullName = item.Name.ToString() + " " + item.Family.ToString();
                    r.ProfileImage = item.ProfileImage;
                    r.ServicesIds = item.ServicesIds;
                    r.IsAuthenticate = item.IsAuthenticate.ToString();
                    r.Message = "ورود با موفقیت انجام شد";

                    return new HttpResponseMessage()
                    {
                        Content =
                            new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(r), Encoding.UTF8, "application/json")
                    };
                }
                else
                {
                    User tb = new User();

                    tb.Name = user.GivenName.Trim();
                    tb.Family = user.FamilyName.Trim();
                    if (string.IsNullOrEmpty(user.Mobile))
                        tb.Mobile = "";
                    else
                        tb.Mobile = user.Mobile;

                    if (string.IsNullOrEmpty(user.City))
                        tb.City = "";
                    else
                        tb.City = user.City;

                    //tb.SmsCode = smsCode.ToString();
                    tb.Token = string.IsNullOrEmpty(user.Token.Trim()) ? "" : user.Token.Trim();
                    if (string.IsNullOrEmpty(user.Age))
                        tb.Age = "";
                    else
                        tb.Age = user.Age;
                    tb.ServicesIds = "";
                    tb.Instagram = "";
                    tb.Telegram = "";
                    tb.Soroosh = "";
                    tb.CreateDate = DateTime.Now;
                    //tb.IsAuthenticate = false;
                    ////////////////////////////////
                    if (string.IsNullOrEmpty(user.Email))
                        tb.Gmail = "";
                    else
                        tb.Gmail = user.Email;
                    /////////////////////////////////
                    if (string.IsNullOrEmpty(user.Aboutme))
                        tb.AboutMe = "";
                    else
                        tb.AboutMe = user.Aboutme;
                    /////////////////////////////////
                    if (string.IsNullOrEmpty(user.Images))
                        tb.ProfileImage = "";
                    else
                        tb.ProfileImage = user.Images.Replace("lh4", "lh3").Replace("?sz=50", "");

                    db.Users.Add(tb);
                    db.SaveChanges();

                    var endUser = db.Users
                                   .OrderByDescending(p => p.Id).ToList()
                                   .FirstOrDefault();

                    r.UserId = endUser.UserId.ToString();
                    r.FullName = endUser.Name.ToString() + " " + endUser.Family.ToString();
                    r.ProfileImage = endUser.ProfileImage;
                    r.ServicesIds = endUser.ServicesIds;
                    r.IsAuthenticate = endUser.IsAuthenticate.ToString();
                    r.Message = "ثبت نام با موفقیت انجام شد";

                    return new HttpResponseMessage()
                    {
                        Content =
                            new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(r), Encoding.UTF8, "application/json")
                    };
                }

            }
            catch (Exception ex)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message), Encoding.UTF8, "application/json")
                };
            }

        }
    }

    public class UserItem
    {
        public string UserId { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
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
}
