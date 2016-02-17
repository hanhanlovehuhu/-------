using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModel;
using WebUtility;

namespace WebBll.PartyRequest
{
    public class ParterCallRequestBll : JsonCommand
    {
        #region Command Members

        public string execute(string request)
        {
            //1203|13812345678|13112345678|adsafsd123123asdas|calltime|IMEI|MD5串
            string result = "0";
            string[] req = request.Split('|');
            var sign = EncodingUtil.GetMd5(req[1] + "aidaijia", "utf-8");

            if (sign.ToLower() == req[6].ToLower())
            {
                #region old代码
                //T_Parter parter = new ParterDal().GetParterModelBySign(req[3].ToLower());
                //if (parter != null)
                //{
                //    T_ParterCalls yuyue = new T_ParterCalls();
                //    yuyue.fphone = req[1].Trim().Length > 13 ? req[1].Trim().Substring(0, 12) : req[1].Trim();
                //    yuyue.tphone = req[2].Trim();
                //    yuyue.sign = req[3].Trim();
                //    yuyue.calltime = Convert.ToDateTime(req[4].Trim());
                //    yuyue.addtime = DateTime.Now;
                //    if (new ParterCallDal().AddParterCalls(yuyue))
                //    {
                //        //添加数据
                //        T_ParterDyLog tpl = new T_ParterDyLog();
                //        tpl.sign = req[3];
                //        tpl.lat = "0";
                //        tpl.lng = "0";
                //        tpl.imei = req[5];
                //        tpl.addtime = DateTime.Now;
                //        //1:获取司机2:上传通话3:上传预约
                //        tpl.typeid = 2;
                //        new ParterDyLogDal().AddParterDyLog(tpl);
                //        result = "1";
                //    }
                //}
                //else
                //{
                //    throw new Exception("商户标识错误。");
                //} 
                #endregion

                return "1";
            }
            else
            {
                throw new Exception("签名错误。");
            }
            return result;
        }
        #endregion
    }
}
