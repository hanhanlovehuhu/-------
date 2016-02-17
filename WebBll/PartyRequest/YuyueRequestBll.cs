using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDal;
using WebModel;
using WebUtility;

namespace WebBll
{
    internal class YuyueRequestBll : JsonCommand
    {
        public string execute(string request)
        {
            //1202|1张三|21381234567|3莘凌路211号(小锦江酒店)|421:00|5asdfas1231asd|6IMEI|7MD5串|8lat|9lng
            string result = "0";
            string[] req = request.Split('|');
            var sign = EncodingUtil.GetMd5(req[2] + "aidaijia", "utf-8");
            if (sign.ToLower() == req[7].ToLower())
            {

                //判断T_ClentInfo是否有此手机号相关信息
                //如果用户不存在，创建一个新用户
                CustomerInfoEntity cinfo = new D_CustomerInfoDal().GetClientInfoByPhone(req[2].Trim());
                string cusid = string.Empty;
                int CustomerType = 1;
                int AccountType = 2;


                T_ParterEntity parter = new T_PartyDal().GetParterModelBySign(req[5].ToLower());
                if (parter != null)
                {

                    #region 用户不存在则创建

                    if (cinfo == null)
                    {
                        int clentnum = new D_CustomerInfoDal().SelectSeqClent();
                        if (clentnum != 0)
                        {
                            string num = string.Empty;
                            for (int i = 0; i < 8 - clentnum.ToString().Length; i++)
                            {
                                num += @"0";
                            }
                            num = "U" + num + clentnum;
                            cusid = num;
                            CustomerInfoEntity customerEn = new CustomerInfoEntity()
                            {
                                AccountType = 2,
                                Amount = 0,
                                BusinessmanId = 0,
                                Cellphone = req[2].Trim(),
                                ParentPhone = req[2].Trim(),
                                CustomerId = num,
                                CustomerName = req[1].Trim(),
                                CustomerType = 1,
                                Email = string.Empty,
                                HomeAddress = string.Empty,
                                IsNewCustomer = true,
                                LastAppointmentTime = null,
                                LastCalledAddress = string.Empty,
                                RecommendCode =
                                    new D_CustomerInfoDal().GenerateCustomerRecommendCode(
                                        new D_CustomerInfoDal().SelectSeqClent()),
                                RegisterFrom = 8,       //第三方API
                                RegisterVersion = ""
                            };
                            bool isture = new D_CustomerInfoDal().AddCustomerInfo(customerEn);
                            if (!isture)
                            {
                                return "0";
                            }

                        }
                        else
                        {
                            return "0";
                        }

                    }
                    else
                    {
                        cusid = cinfo.CustomerId;
                        CustomerType = cinfo.CustomerType;
                        AccountType = cinfo.AccountType;
                    }
                    #endregion

                    #region 优惠码操作
                    string couponCode = "";
                    if (req.Length == 12 && parter.url != null)
                    {
                        ThirdPartyCoupon cc = new ThirdPartyCoupon();
                        if (!cc.check(req[10], req[11], parter.url))
                        {
                            throw new Exception("优惠码无效");
                        }
                        couponCode = cc.addCoupon(parter.Id.ToString(), req[10], req[11], cinfo);
                    }
                    #endregion

                    #region 创建预约订单

                    int zz = new D_AppointmentOrderInfoDA().SelectSeqOrder();
                    if (zz != 0)
                    {
                        string ordernum = string.Empty;
                        for (int i = 0; i < 8 - zz.ToString().Length; i++)
                        {
                            ordernum += @"0";
                        }
                        ordernum = "Party" + ordernum + zz;
                        string lat = req[8].Trim();
                        string lng = req[9].Trim();
                        if (parter.Id == 11)//判断是博泰，对坐标进行转换
                        {
                            ChangeCoords.ChangeCoordinate(ref lat, ref lng, 3, 5);
                        }
                        AppointmentOrderInfoEntity orderEn = new AppointmentOrderInfoEntity()
                        {
                            AppointAddress = req[3].ToLower(),
                            AppointOrderId = ordernum,
                            AppointOrderFrom = 5, //  第三方合作用户，由8改为5
                            AppointOrderSign = "1",
                            AppointOrderState = 10,
                            AppointOrderType = 1, // 1自己叫
                            AppointTime = Convert.ToDateTime(req[4].Trim()),
                            AppointTimeStr = DateTime.Now.ToString("yyyy-MM-dd-HH-mm"),
                            Cellphone = req[2].Trim(),
                            FromCellphone = req[2].Trim(),
                            CustomerId = cusid,
                            CustomerName = req[1].Trim(),
                            Create_time = DateTime.Now,
                            Create_user = "Party CreateOrder",
                            BusinessType = 1,
                            CustomerType = CustomerType,
                            AccountType = AccountType,
                            PartyId = parter.Id
                        };
                        if (req.Length > 8)
                        {
                            orderEn.Lat = Convert.ToDecimal(lat);
                            orderEn.Lng = Convert.ToDecimal(lng);
                        }
                        bool istrue = new D_AppointmentOrderInfoDA().AddAppointmentOrderInfo(orderEn);
                        if (!istrue)
                        {
                            result = "0";
                        }
                        else
                        {
                            //添加数据
                            T_ParterDyLog tpl = new T_ParterDyLog();
                            tpl.sign = req[5];
                            tpl.lat = "0";
                            tpl.lng = "0";
                            tpl.imei = req[6];
                            tpl.addtime = DateTime.Now;
                            //1:获取司机2:上传通话3:上传预约
                            tpl.typeid = 3;
                            new T_ParterDyLogDal().AddParterDyLog(tpl);
                            result = "1";
                        }


                    }
                    else
                    {
                        throw new Exception("预约订单失败!");
                    }

                    #endregion
                }
                else
                {
                    throw new Exception("商户标识错误。");
                }
            }
            else
            {
                throw new Exception("签名错误。");
            }
            return result;
        }

    }
}
