using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebDal;
using WebModel;
using WebUtility;

namespace WebBll
{
    class GetNearSjListBll : JsonCommand
    {
        #region Command Members

        public string execute(string request)
        {
            //1201|31.118725|121.376808|12312aasdas12312|IMEI|7a31f99279327f8b75506acbf0503973
            string[] req = request.Split('|');
            var sign = EncodingUtil.GetMd5(req[1] + req[2] + "aidaijia", "utf-8");
            if (sign.ToLower() == req[5].ToLower())
            {
                T_ParterEntity parter = new T_PartyDal().GetParterModelBySign(req[3].ToLower());
                if (parter != null)
                {
                    string lat = req[1].Trim();
                    string lng = req[2].Trim();
                    if (parter.Id == 11)//判断是博泰，对坐标进行转换
                    {
                        ChangeCoords.ChangeCoordinate(ref lat, ref lng, 3, 5);
                    }
                    int countsum = new T_PDLoginLogDal().GetcountBysign(req[3].ToLower());
                    //计算使用次数
                    if (parter.Daycount > countsum)
                    {
                        //添加
                        double latNear = Convert.ToDouble(req[1]);
                        double lngNear = Convert.ToDouble(req[2]);
                        CoordinateHelper.BaiduToScott(ref lngNear, ref latNear);
                        req[1] = latNear.ToString();
                        req[2] = lngNear.ToString();

                        List<sjinfo> users = new D_DriverInfoDal().GetNewUserByLatlng(req[1], req[2],parter.Sign);
                        if (users.Count != 0)
                        {
                            //添加数据
                            T_ParterDyLog TPL = new T_ParterDyLog();
                            
                            TPL.sign = req[3].ToLower();
                            TPL.lat = lat;
                            TPL.lng = lng;
                            TPL.imei = req[4];
                            TPL.addtime = DateTime.Now;
                            //1:获取司机2:上传通话3:上传预约
                            TPL.typeid = 1;
                            new T_ParterDyLogDal().AddParterDyLog(TPL);
                        }
                        return JsonConvert.SerializeObject(users);
                    }
                    else
                    {
                        throw new Exception("当天次数已经使用完毕!");
                    }
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
        #endregion


       

    }
}
