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
    class GetCityPriceBll : JsonCommand
    {
        public string execute(string request)
        {
            // 1205|12312aasdas12312|上海| IMEI |7a31f99279327f8b75506acbf0503973
            string[] req = request.Split('|');
            var sign = EncodingUtil.GetMd5(req[2] + "aidaijia", "utf-8");
            if (sign.ToLower() == req[4].ToLower())
            {
                T_ParterEntity parter = new T_PartyDal().GetParterModelBySign(req[1].ToLower());
                if (parter != null)
                {
                    int countsum = new T_ParterDyLogDal().GetcountBysign(req[1].ToLower());
                    //计算使用次数
                    if (parter.Daycount > countsum)
                    {
                        //添加数据
                        T_ParterDyLog TPL = new T_ParterDyLog();
                        TPL.sign = req[1].ToLower();
                        TPL.lat = "0";
                        TPL.lng = "0";
                        TPL.imei = req[3];

                        TPL.addtime = DateTime.Now;
                        //1:获取司机2:上传通话3:上传预约4:获取评价5:获取城市信息
                        TPL.typeid = 5;
                        new T_ParterDyLogDal().AddParterDyLog(TPL);
                        ChengShiModel cs = new D_CityPriceDal().GetChengShiInfo(req[2].ToLower());
                        if (cs.Name != null)
                        {
                            return JsonConvert.SerializeObject(cs);
                        }
                        else
                        {
                            IList<ChengShiName> csn = new D_CityPriceDal().GetChengShiName(req[2].ToLower());
                            return JsonConvert.SerializeObject(csn);
                        }

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
    }

   
}
