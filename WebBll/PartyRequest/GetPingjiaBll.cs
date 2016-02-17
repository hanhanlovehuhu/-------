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
    class GetPingjiaBll : JsonCommand
    {
        public string execute(string request)
        {
            // 1207|12312aasdas12312|ucode| IMEI |7a31f99279327f8b75506acbf0503973
            string[] req = request.Split('|');
            var sign = EncodingUtil.GetMd5(req[2] + "aidaijia", "utf-8");
            if (sign.ToLower() == req[4].ToLower())
            {
                T_ParterEntity parter = new T_PartyDal().GetParterModelBySign(req[1].ToLower());
                if (parter != null)
                {
                    int countsum = new T_PDLoginLogDal().GetcountBysign(req[1].ToLower());
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
                        //1:获取司机2:上传通话3:上传预约4:获取评价
                        TPL.typeid = 4;
                        new T_ParterDyLogDal().AddParterDyLog(TPL);
                        List<SjComment> tdpl = new D_DriverCommentDal().GetPingLuns(req[2]);
                        List<SjComment> tdpl2 = new List<SjComment>();
                        if (tdpl != null)
                        {
                            for (int i = 0; i < tdpl.Count; i++)
                            {
                                SjComment temp = new SjComment();
                                temp.Create_time = tdpl[i].Create_time;
                                if (tdpl[i].Cellphone.Length > 8) // Regex.IsMatch(tdpl[i].Cellphone, "^1[3-9]{1}[0-9*]{9}") 
                                    temp.Cellphone = tdpl[i].Cellphone.Substring(0, 4) + "****" + tdpl[i].Cellphone.Substring(8);
                                else
                                    temp.Cellphone = "1000****000"; //电话号码长度不匹配处理
                                temp.Comment = tdpl[i].Comment;
                                temp.CustomerName = tdpl[i].CustomerName;
                                temp.Evaluate = tdpl[i].Evaluate;
                                temp.Ucode = tdpl[i].Ucode;
                                tdpl2.Add(temp);

                            }
                        }
                        return JsonConvert.SerializeObject(tdpl2);


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
