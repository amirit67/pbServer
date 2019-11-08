using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Linq;
using System.Web.Http;
using Paye.Models;

namespace Sanatyar.EmdadExpert.Controller.WebApiControllers
{
    public class GetUserVerificationController : ApiController
    {
        public HttpResponseMessage Post([FromBody] FormDataCollection formDataCollection)
        {
            PayeDBEntities db = new PayeDBEntities();

            //var res = new BaseSystemModel.ApiResponse { Type = 0 };
            try
            {
                var mobile = formDataCollection.Get("Mobile").Trim();
                var smsCode = formDataCollection.Get("SmsCode").Trim();
                var UserId = formDataCollection.Get("UserId").Trim();
                if (string.IsNullOrEmpty(mobile))
                {
                    return new HttpResponseMessage()
                    {
                        Content =
                               new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject("خطا در پارامترهای ورودی"), Encoding.UTF8, "application/json")
                    };
                }

                else
                {
                    returnUser r = new returnUser();
                    //var item = db.Users.FirstOrDefault(i => i.UserId.ToString() == UserId);
                    var item = db.Users.FirstOrDefault(i => i.Mobile.ToString() == mobile);
                    var smsUser = db.Sms.FirstOrDefault(i => i.userId.ToString() == UserId);
                    if (item != null)
                    {

                        if (smsUser.sms.ToString() != smsCode.Trim())
                        {
                            r.UserId = "0";
                            r.FullName = "";
                            r.Message = "کد وارد شده اشتباه است";
                            return new HttpResponseMessage()
                            {
                                Content =
                                    new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(r), Encoding.UTF8, "application/json")
                            };
                        }
                        else if (smsUser.sms.ToString().Trim() == smsCode.Trim())
                        {
                            /*var list = db.Posts.Where(x => x.UserId == item.Id).ToList();
                            foreach (var room in list)
                            {
                                //db.Posts.Attach(room);
                                if((bool)room.IsImmediate && room.State == 1)
                                { 
                                    room.State = 1;
                                    db.SaveChanges();
                                }
                                else if (room.State == 22 && (bool)room.)
                                {
                                    room.State = 2;
                                    db.SaveChanges();
                                }

                            }*/

                            item.IsAuthenticate = true;
                            item.IsMobileAuthenticate = true;
                            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            r.UserId = item.UserId.ToString();
                            r.FullName = item.Name.ToString() + " " + item.Family.ToString();
                            r.ProfileImage = item.ProfileImage;
                            r.ServicesIds = item.ServicesIds;
                            r.Message = "ورود با موفقیت انجام شد";

                            return new HttpResponseMessage()
                            {
                                Content =
                                    new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(r), Encoding.UTF8, "application/json")
                            };
                        }
                    }

                    r.UserId = "0";
                    r.FullName = "";
                    r.Message = "این شماره موبایل در سیستم وجود ندارد";

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
}
