
using BaseSystemModel.Utilty;
using Paye.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web;

namespace Paye.Controllers
{
    public class getServicesController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Headers["PayeBash"] != null)
            {

                PayeDBEntities db = new PayeDBEntities();
                var list = (from x in db.Services
                            orderby x.Id descending
                            select x).ToList();

                return new HttpResponseMessage()
                {
                    Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(list), Encoding.UTF8, "application/json")
                };
            }
            return null;
        }
    }
}
