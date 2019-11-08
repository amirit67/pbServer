using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace BaseSystemModel
{
    public static class ResizeImage
    {
        public static int[] GetDateDifferencesAsYearMonthDay(DateTime fromDate, DateTime toDate)
        {
            var fromDateM = (long)(fromDate - new DateTime(1970, 1, 1)).TotalMilliseconds;
            var toDateM = (long)(toDate - new DateTime(1970, 1, 1)).TotalMilliseconds;
            long diff = toDateM - fromDateM;
            if (diff < 0) return new[] { 0, 0, 0, 0, 0, 0, 0 };

            int difInDays = (int)((diff) / (1000 * 60 * 60 * 24));
            int difInHours = (int)((diff - (difInDays * 1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
            int difInMinuts = (int)((diff - (difInHours * 1000 * 60 * 60)) / (1000 * 60));
            int difInSeconds = (int)((diff - (difInMinuts * 1000 * 60)) / (1000));
            int difInMiliSeconds = (int)(diff - (difInSeconds * 1000));

            int year = difInDays / 365;
            int month = (difInDays - year * 365) / 30;

            return new[] { year, month, difInDays, difInHours, difInMinuts, difInSeconds, difInMiliSeconds };
        }
        public static string GetDateDifferencesAsDescription(DateTime fromDate, DateTime toDate, int type)
        {
            int[] diffArr = GetDateDifferencesAsYearMonthDay(fromDate, toDate);
            if (type == 0)
            {
                if (diffArr[0] > 0)
                    return diffArr[0] + " سال پیش";
                if (diffArr[1] > 0)
                    return diffArr[1] + " ماه پیش";
                if (diffArr[2] >= 2)
                    return diffArr[2] + " روز پیش";
                if (diffArr[2] == 1)
                    return "دیروز";
                if (diffArr[3] > 0)
                    return diffArr[3] + " ساعت پیش";
                if (diffArr[4] > 0)
                    return diffArr[4] + " دقیقه پیش";
                if (diffArr[5] > 0)
                    return diffArr[5] + " ثانیه پیش";
                return "لحظاتی پیش";
            }
           
            return "";
        }

        public static string GetDateDifferencesAsDescription2(DateTime fromDate, DateTime toDate, int type)
        {
            int[] diffArr = GetDateDifferencesAsYearMonthDay(fromDate, toDate);
            if (type == 0)
            {
                if (diffArr[0] > 0)
                    return diffArr[0] + " سال مانده";
                if (diffArr[1] > 0)
                    return diffArr[1] + " ماه مانده";
                if (diffArr[2] >= 2)
                    return diffArr[2] + " روز مانده";
                if (diffArr[2] == 1)
                    return "1 روز مانده";
                if (diffArr[3] > 0)
                    return diffArr[3] + " ساعت مانده";
                if (diffArr[4] > 0)
                    return diffArr[4] + " دقیقه مانده";
                if (diffArr[5] > 0)
                    return diffArr[5] + " ثانیه مانده";
                return "لحظاتی مانده";
            }

            return "";
        }
        public static Image ResizeImageByMinRatio(Image image, int minWidth, int minHeight)
        {
            var ratioX = (double)minWidth / image.Width;
            var ratioY = (double)minHeight / image.Height;
            var ratio = Math.Max(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);
            var destRect = new Rectangle(0, 0, newWidth, newHeight);
            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return newImage;
        }
    }
}