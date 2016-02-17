using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDal;
using WebModel;
using WebUtility;

namespace WebBll.PartyRequest
{
    public class CreateBusinessOrderBll : JsonCommand
    {

        public string execute(string request)
        {
            //1219|1张三|21381234567|3莘凌路211号(小锦江酒店)|42|521:00|613896365986|7沪B195689|81369896522344|9""|10lat|11lng|128|13合作商标识|14IMEI|15MD5串|16Remark|17SendCarUserName|18SendCarPhone|19EndDriveLat|20EndDriveLng|21EndDriveAddress:|22ReceiveCarPhone|23ReceiveCarUserName
            string result = string.Empty;
            string[] req = request.Split('|');
            var sign = EncodingUtil.GetMd5(req[2] + "aidaijia", "utf-8");
            if (sign.ToLower() == req[15].ToLower())
            {
                T_ParterEntity parter = new T_PartyDal().GetParterModelBySign(req[13].ToLower());
                if (parter != null && parter.State.Equals(ConfigHelper.Sign))
                {
                    BusinessOrderServiceRequestEntity model = new BusinessOrderServiceRequestEntity()
                    {
                        Address = req[3],
                        AppointTime = req[5],
                        AllUserCount = req[4],
                        CalledNo = req[6],
                        CarNumber = req[7],
                        CellPhone = req[8],
                        DriverRemark = req[9],
                        EndDriveAddress = req[21],
                        EndDriveLat = Convert.ToDouble(req[19]),
                        EndDriveLng = Convert.ToDouble(req[20]),
                        From = req[12],
                        IMEI = req[14],
                        Lat = Convert.ToDouble(req[10]),
                        Lng = Convert.ToDouble(req[11]),

                        Name = req[1],
                        Phone = req[2],
                        ReceiveCarPhone = req[22],
                        ReceiveCarUserName = req[23],
                        Remark = req[16],
                        SendCarPhone = req[18],
                        SendCarUserName = req[17],
                        Sign = req[13],
                        BusinessType = 6,
                        Smart = 1,
                        OrderFrom = 11,
                        OrderTime = DateTime.Now
                    };
                    double lat=model.Lat;

                    double lng=model.Lng;

                    CoordinateHelper.BaiduToScott(ref lng, ref lat);
                    model.Lat = lat;
                    model.Lng = lng;
                    string url = string.Format("http://{0}/api/createOrder", ConfigHelper.APIServer);

                    result = new CreateBusinessOrder().InvokeCreateOrderService(url, model);

                    return result;
                }
                else
                {
                    throw new Exception("商户标识错误");
                }
            }
            else
            {
                throw new Exception("签名错误。");
            }
        }
    }
}
