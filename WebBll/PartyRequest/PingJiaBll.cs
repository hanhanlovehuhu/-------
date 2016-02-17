using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebDal;
using WebModel;
using WebUtility;

namespace WebBll.PartyRequest
{
    public class PingJiaBll : JsonCommand
    {
        public string execute(string request)
        {
            // 1204|12312aasdas12312|uid| IMEI |7a31f99279327f8b75506acbf0503973
            string[] req = request.Split('|');
            var sign = EncodingUtil.GetMd5(req[2] + "aidaijia", "utf-8");
            if (sign.ToLower() == req[4].ToLower())
            {
                #region old代码
                //T_Parter parter = new ParterDal().GetParterModelBySign(req[1].ToLower());
                //if (parter != null)
                //{
                //    int countsum = new ParterDyLogDal().GetcountBysign(req[1].ToLower());
                //    //计算使用次数
                //    if (parter.daycount > countsum)
                //    {
                //        //添加数据
                //        T_ParterDyLog TPL = new T_ParterDyLog();
                //        TPL.sign = req[1].ToLower();
                //        TPL.lat = "0";
                //        TPL.lng = "0";
                //        TPL.imei = req[3];
                //        TPL.addtime = DateTime.Now;
                //        //1:获取司机2:上传通话3:上传预约4:获取评价
                //        TPL.typeid = 4;
                //        new ParterDyLogDal().AddParterDyLog(TPL);
                //        List<Pinglun> tdpl = new PingJiaDal().GetPingLuns(req[2]);
                //        return JsonConvert.SerializeObject(tdpl);
                //    }
                //    else
                //    {
                //        throw new Exception("当天次数已经使用完毕!");
                //    } 

                //}
                //else
                //{
                //    throw new Exception("商户标识错误");
                //}
                #endregion

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
                        List<Pinglun> tdpl2 = new List<Pinglun>();
                        string sjucode = new D_DriverInfoDal().GetUcodeById(req[2]);
                        if (sjucode != "")
                        {
                            List<SjComment> tdpl = new D_DriverCommentDal().GetPingLuns(sjucode);
                            if (tdpl != null)
                            {
                                for (int i = 0; i < tdpl.Count; i++)
                                {
                                    Pinglun temp = new Pinglun();
                                    temp.addtime = Convert.ToDateTime(tdpl[i].Create_time);
                                    temp.Info = tdpl[i].Comment;
                                    temp.name = tdpl[i].CustomerName;

                                    if (tdpl[i].Evaluate == 3)
                                    {
                                        temp.plstate = 0;
                                    }
                                    else if (tdpl[i].Evaluate == 2)
                                    {
                                        temp.plstate = 1;
                                    }
                                    else
                                    {
                                        temp.plstate = 2;
                                    }
                                    // 0差评1中评2好评  //新版（1，好评；2，中评；3，差评）
                                    temp.uid = Convert.ToInt32(req[2].Trim());
                                    tdpl2.Add(temp);
                                }
                            }
                            return JsonConvert.SerializeObject(tdpl2);

                        }
                        else
                        {
                            return JsonConvert.SerializeObject(tdpl2);
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
        private class Pinglun
        {
            public int uid { get; set; }
            public string name { get; set; }
            public string Info { get; set; }
            public int plstate { get; set; }
            public DateTime addtime { get; set; }
        }
    }
}
