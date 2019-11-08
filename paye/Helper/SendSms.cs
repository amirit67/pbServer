using System;
using System.Threading.Tasks;
using RestSharp;

namespace BaseSystemModel
{
   public static class SendSms
    {
        public static async Task SendSimpleSms2(string tel, string content, bool isflash=false)//,ref string ans)
        {
            if (string.IsNullOrEmpty(content))
                return;
            if (string.IsNullOrEmpty(tel))
                return;
            if (!System.Text.RegularExpressions.Regex.IsMatch(tel, @"09\d{9}"))
                return;
            string ans;
            try
            {
                var client = new RestClient("http://rest.payamak-panel.com/api/SendSMS/SendSMS");
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("postman-token", "fcddb5f4-dc58-c7d5-4bf9-9748710f8789");
                request.AddHeader("cache-control", "no-cache");
                request.AddParameter("application/x-www-form-urlencoded", "username=mahdimajd1368&password=9447&to=" + tel + "&from=50004000540540&text=" + content + "&isflash=false", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                /* var endpointAdress = new EndpointAddress("http://api.payamak-panel.com/post/Send.asmx");
                 var binding = new BasicHttpBinding();
                 var result = new ApiPayamakPanel.SendSoapClient(binding, endpointAdress);
                 var ansSendSms =await result.SendSimpleSMS2Async(username, password, tel, from, content, isflash);//10000000003019*/

                switch (Convert.ToInt32(response.StatusCode))
                {
                    case 0:
                        ans = "نام کاربری یا رمز عبور اشتباه می باشد";
                        break;
                    case 1:
                        ans = "درخواست با موفقیت انجام شد";
                        break;
                    case 2:
                        ans = "اعتبار کافی نمی باشد";
                        break;
                    case 3:
                        ans = "محدودیت در ارسال روزانه";
                        break;
                    case 4:
                        ans = "محدودیت در حجم ارسال";
                        break;
                    case 5:
                        ans = "شماره فرستنده معتبر نمی باشد";
                        break;
                    case 6:
                        ans = "سامانه در حال بروزرسانی می باشد";
                        break;
                    case 7:
                        ans = "متن حاوی کلمه فیلتر شده می باشد";
                        break;
                    case 9:
                        ans = "ارسال از خطوط عمومی از طریق وب سرویس امکان پذیر نمی باشد";
                        break;
                    case 10:
                        ans = "کاربر مورد نظر فعال نمی باشد";
                        break;
                    case 11:
                        ans = "ارسال نشده";
                        break;
                    case 12:
                        ans = "مدارک کاربر کامل نمی باشد";
                        break;
                    default:
                        ans = "درخواست با موفقیت انجام شد";
                        break;
                }
            }
            catch (Exception ex)
            {
                ans = ex.Message;
                //throw new BusinessException("خطا در ارسال پیامک");
            }
            var message = ans;
        }
    }
}
