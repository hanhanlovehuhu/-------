using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace WebUtility
{
    public class ChangeCoords
    {

        /// <summary>
        /// 坐标转换
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="from">
        /// 1：GPS设备获取的角度坐标;
        /// 2：GPS获取的米制坐标、sogou地图所用坐标;
        /// 3：google地图、soso地图、aliyun地图、mapabc地图和amap地图所用坐标
        /// 4：3中列表地图坐标对应的米制坐标
        /// 5：百度地图采用的经纬度坐标
        /// 6：百度地图采用的米制坐标
        /// 7：mapbar地图坐标;
        /// 8：51地图坐标 
        /// </param>
        /// <param name="to">
        /// 5：bd09ll(百度经纬度坐标),
        /// 6：bd09mc(百度米制经纬度坐标); 
        /// </param>
        public static void ChangeCoordinate(ref string lat, ref string lng, int from, int to) 
        {
            WebRequest request = null;
            WebResponse response = null;
            string oldlat = lat;
            string oldlng = lng;
            try
            {
                string url = "http://api.map.baidu.com/geoconv/v1/?coords={0},{1}&from={2}&to={3}&ak=WQLnCTINLVGuzmKEO7o7ADjh";
                request = WebRequest.Create(string.Format(url,lat,lng,from,to));
                response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string strMsg = reader.ReadToEnd();
                    JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();
                    CoordsEntity coords = (CoordsEntity)jsonSerialize.Deserialize<CoordsEntity>(strMsg);
                    lat = coords.result[0].x.ToString();
                    lng = coords.result[0].y.ToString();
                }
            }
            catch (Exception ex)
            {
                lat = oldlat;
                lng = oldlng;
            }
        }
    }

    [Serializable]
    public class CoordsEntity
    {
        public int status { get; set; }
        public Coords[] result { get; set; } 
    }

    [Serializable]
    public class Coords
    {
        public double x { get; set; }
        public double y { get; set; }
    }
}
