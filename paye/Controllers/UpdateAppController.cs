using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Net.Http.Formatting;
using System.Linq;
using Paye.Models;
using System;
using BaseSystemModel.Helper;

namespace Paye.Controllers
{
    public class UpdateAppController : ApiController
    {            
        //[ResponseType(typeof(Activity))]
        public HttpResponseMessage Get()
        {
            UpdateApp item = new UpdateApp();
            item.VersionName = "1.0.0";
            item.DownloadUrl = "http://paye.ariaapps.ir/payebash.apk";
            item.Description = "مایل به بروزرسانی و استفاده از امکانات جدید هستید؟";
            item.Feepayable = "1000";

            //Business.Expert.ExpertPersonBiz.Instance.UpdateRefreshToken(userid, token);
            return new HttpResponseMessage()
            {
                Content =
                    new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json")
            };
        }
    }
}
