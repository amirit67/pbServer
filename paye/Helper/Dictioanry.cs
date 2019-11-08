using Paye.Models;
using System.Collections.Generic;

namespace BaseSystemModel.Helper
{
    public static class Dictioanry
    {
        public static readonly Dictionary<byte, string> GetPostService = new Dictionary<byte, string>
        {
            {Post.State_Pay_Category, "منتظر پرداخت (دسته بندی غیررایگان)"},
            {Post.State_Pay_Immadiate, "منتظر پرداخت (فوری کردن)"},
            {Post.State_Pay_Vip, "منتظر پرداخت (ویژه)"},
            {Post.State_Pay_Story, "منتظر پرداخت (استوری)"},
            {Post.State_Pay_Nardeban, "منتظر پرداخت (نردیان)"}
        };

        public static readonly Dictionary<byte, string> GetPostServiceDescription = new Dictionary<byte, string>
        {
           {Post.State_Pay_Category, "رویداد شما با موفقیت ثبت شده اما انتشار آن در دسته انتخابی شما رایگان نیست.\nبا پرداخت هزینه آن می توانید آن را برای بررسی ناظر ارسال کنید."},
             {Post.State_Pay_Immadiate, "رویداد شما با موفقیت ثبت شده اما فوری کردن برنامه مستلزم پرداخت هزینه می باشد.\nبا پرداخت هزینه آن می توانید آن را برای بررسی ناظر ارسال کنید."},
        };

        public static readonly Dictionary<byte, string> GetPosetServiceColor = new Dictionary<byte, string>
        {
             {Post.State_Pay_Category, "#fc8042"},
             {Post.State_Pay_Immadiate, "#fc8042"},          
        };




        public static readonly Dictionary<byte, string> GetStatesPayePost = new Dictionary<byte, string>
        {
            {Post.State_Ok, "منتشر شده"},
            {Post.State_OkEdit, "منتشر شده"},

            {Post.State_New, "در انتظار تایید"},
            {Post.State_Edit, "در انتظار تایید"},

            {Post.State_Correction1, "نیاز به اصلاح"},
            {Post.State_Correction2, "نیاز به اصلاح"},
            {Post.State_Correction3, "نیاز به اصلاح"},
            {Post.State_Correction4, "نیاز به اصلاح"},
            {Post.State_Auth_mobile, "منتظر تایید شماره"},

            {Post.State_Delete_Successful, "حذف شده"},
            {Post.State_NokEdit, "رد شده"},
            {Post.State_Nok, "رد شده"},
            {Post.State_Cancel, "لغو شد"},

        };

        public static readonly Dictionary<byte, string> GetStatesDescriptionPayePost = new Dictionary<byte, string>
        {

             {Post.State_Delete_Successful, "فعالیت شما حذف گردید"},
             {Post.State_New, "فعالیت شما در صف انتظار قرار گرفت و پس از بررسی انتشار خواهد یافت."},
             {Post.State_Edit,"فعالیت شما در صف انتظار قرار گرفت و پس از بررسی انتشار خواهد یافت."},
             {Post.State_NokEdit, "فعالیت شما رد شد."},
             {Post.State_Nok, "فعالیت شما رد شد."},
             {Post.State_OkEdit, "فعالیت شما منتشر شده است و در لیست پایه باش قرار گرفته است."},
             {Post.State_Ok, "فعالیت شما منتشر شده است و در لیست پایه باش قرار گرفته است."},
             {Post.State_Cancel, "این برنامه لغو گردید."},
             {Post.State_Correction1, "از تصاویر مناسب استفاده نمایید."},
             {Post.State_Correction2, "توضیحات فعالیت خود را اصلاح نمایید."},
             {Post.State_Correction3, "تاریخ شروع و پایان برنامه را اصلاح نمایید."},
             {Post.State_Correction4, "عنوان برنامه خود را اصلاح نمایید."},
             {Post.State_Auth_mobile, "لطفا شماره موبایل خود را تایید کنید."},
             //پیام کوتاهی شامل کد به منظور تایید شماره تماس شما فرستاده خواهد شد.با استفاده از این کد شماره تلفن خود را تایید کنید.
        };

        public static readonly Dictionary<byte, string> GetStatesColorPayePost = new Dictionary<byte, string>
        {
             
             {Post.State_Delete_Successful, "#fc8042"},
             {Post.State_New, "#fc8042"},
             {Post.State_Edit,"#fc8042"},
             {Post.State_NokEdit, "#ea4335"},
             {Post.State_Nok, "#ea4335"},
             {Post.State_OkEdit, "#34a853"},
             {Post.State_Ok, "#34a853"},
             {Post.State_Cancel, "#fc8042"},
              {Post.State_Correction1, "#fc8042"},
               {Post.State_Correction2, "#fc8042"},
                {Post.State_Correction3, "#fc8042"},
                 {Post.State_Correction4, "#fc8042"},
                 {Post.State_Auth_mobile, "#595FB1"},
        };
        //public static readonly Dictionary<int, string> ZarrinPalDictionary = new Dictionary<int, string>
        //{
        //    {101, "تراكنش انجام شده است. PaymentVerification عمليات پرداخت موفق بوده و قبلا"},
        //    {-1, "اطلاعات ارسال شده ناقص است."},
        //    {-2, "و يا مرچنت كد پذيرنده صحيح نيست. IP"},
        //    {-3, "با توجه به محدوديت هاي شاپرك امكان پرداخت با رقم درخواست شده ميسر نمي باشد."},
        //    {-4, "سطح تاييد پذيرنده پايين تر از سطح نقره اي است."},
        //    {-11, "درخواست مورد نظر يافت نشد."},
        //    {-12, "امكان ويرايش درخواست ميسر نمي باشد."},
        //    {-21, "هيچ نوع عمليات مالي براي اين تراكنش يافت نشد."},
        //    {-22, "تراكنش نا موفق ميباشد."},
        //    {-33, "رقم تراكنش با رقم پرداخت شده مطابقت ندارد."},
        //    {-34, "سقف تقسيم تراكنش از لحاظ تعداد يا رقم عبور نموده است"},
        //    {-40, "اجازه دسترسي به متد مربوطه وجود ندارد."},
        //    {-41, "غيرمعتبر ميباشد. AdditionalData اطلاعات ارسال شده مربوط به"},
        //    {-42, "مدت زمان معتبر طول عمر شناسه پرداخت بايد بين 30 دقيه تا 45 روز مي باشد."},
        //    {-54, "درخواست مورد نظر آرشيو شده است."},
        //    {100, "عمليات با موفقيت انجام گرديده است."}
        //};
    }
}
