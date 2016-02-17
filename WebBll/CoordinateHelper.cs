using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebBll
{
    public class CoordinateHelper
    {
        private static double rad(double d)
        {
            return d * Math.PI / 180.0f;
        }

        /// <summary>
        /// 得到两个经纬度之间的距离，返回米
        /// </summary>
        /// <param name="lng1"></param>
        /// <param name="lat1"></param>
        /// <param name="lng2"></param>
        /// <param name="lat2"></param>
        /// <returns></returns>
        public static double getDistance(double lng1, double lat1, double lng2, double lat2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);

            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
                                               Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));

            s = s * 6371004;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }

        const double x_pi = 3.14159265358979324 * 3000.0 / 180.0;

        /// <summary>
        /// 百度坐标 转 高德坐标
        /// </summary>
        /// <param name="lng">坐标经度</param>
        /// <param name="lat">坐标纬度</param>
        public static void BaiduToScott(ref double lng, ref double lat)
        {
            try
            {
                double x = lng - 0.0065;
                double y = lat - 0.006;
                double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_pi);
                
                double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_pi);
                lng = z * Math.Cos(theta);
                lat = z * Math.Sin(theta);
            }
            catch(Exception ex)
            {
                throw new Exception("百度转高德出错:" + ex.Message);
            }
        }

        /// <summary>
        /// 高德坐标 转 百度坐标
        /// </summary>
        /// <param name="lng">坐标经度</param>
        /// <param name="lat">坐标纬度</param>
        public static void ScottToBaidu(ref double lng, ref double lat)
        {
            try
            {
                double x = lng;
                double y = lat;
                double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * x_pi);
                double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * x_pi);
                lng = z * Math.Cos(theta) + 0.0065;
                lat = z * Math.Sin(theta) + 0.006;
            }
            catch (Exception ex)
            {
                throw new Exception("高德转百度出错:" + ex.Message);
            }
        }
    }
}