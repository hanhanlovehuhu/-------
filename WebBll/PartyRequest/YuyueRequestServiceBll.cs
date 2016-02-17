using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebDal;
using WebModel;
using WebUtility;
using System.Net;
using System.IO;

namespace WebBll
{
    internal class YuyueRequestServiceBll : JsonCommand
    {
        LogHelper log = new LogHelper();
        public string execute(string request)
        {
            //1202|1张三|21381234567|3莘凌路211号(小锦江酒店)|421:00|5asdfas1231asd|6IMEI|7MD5串|8lat|9lng|10youhui|11youhuijine
            string result = "0";
            string[] req = request.Split('|');
            var sign = EncodingUtil.GetMd5(req[2] + "aidaijia", "utf-8");
            if (sign.ToLower() == req[7].ToLower())
            {

                //判断T_ClentInfo是否有此手机号相关信息
                //如果用户不存在，创建一个新用户
                CustomerInfoEntity cinfo = new D_CustomerInfoDal().GetClientInfoByPhone(req[2].Trim());
              
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
                                //return "0";
                                throw new Exception("添加新用户失败。");
                            }
                            cinfo = customerEn;
                        }
                        else
                        {
                            //return "0";
                            throw new Exception("获取客户种子数失败。");
                        }

                    }
                  
                    #endregion


                    #region 优惠码操作

                    string couponCode = "";
                    if (!WebUtility.ConfigHelper.CloseSign.ToLower().Contains(req[5].ToLower()))
                    {
                        if (req.Length == 12 && parter.url != null)
                        {
                            ThirdPartyCoupon cc = new ThirdPartyCoupon();
                            if (!cc.check(req[10], req[11], parter.url))
                            {
                                throw new Exception("优惠码无效");
                            }
                            couponCode = cc.addCoupon(parter.Id.ToString(), req[10], req[11], cinfo);
                        }
                        log.log("1202IF:【" + request + "】CLOSESIGN【" + WebUtility.ConfigHelper.CloseSign.ToLower() + "】req[5]【" + req[5].ToLower() + "】", System.Web.HttpContext.Current.Server.MapPath("Log/log.txt"));
                    }
                    else
                    {
                        log.log("1202ELSE:【" + request + "】CLOSESIGN【" + WebUtility.ConfigHelper.CloseSign.ToLower() + "】req[5]【" + req[5].ToLower() + "】", System.Web.HttpContext.Current.Server.MapPath("Log/log.txt"));
                    }

                    #endregion



                    #region 调用web service 处理创建预约订单

                    CreateOrderServiceRequestEntity createOrderEntity = new CreateOrderServiceRequestEntity();
                    
                    createOrderEntity.Ucode = string.Empty;
                    createOrderEntity.Parterid = parter.Id;
                    createOrderEntity.BusinessType = 1; // 酒后代驾
                    createOrderEntity.OrderFrom = 5; // 合作商
                    createOrderEntity.AppointTime = string.Empty;
                    createOrderEntity.RecommendCode = "[" + couponCode;
                    DateTime appointTime;
                    if (DateTime.TryParse(req[4].Trim(), out appointTime))
                    {
                        createOrderEntity.AppointTime = appointTime.ToString("yyyy-MM-dd HH:mm");
                    }
                    createOrderEntity.Address = req[3].Trim();
                    createOrderEntity.FromCellPhone = cinfo.Cellphone;
                    createOrderEntity.CellPhone = cinfo.Cellphone;
                    string latStr = req[8].Trim();
                    string lngStr = req[9].Trim();
                    if (parter.Id == 11)//判断是博泰，对坐标进行转换
                    {
                        ChangeCoords.ChangeCoordinate(ref latStr, ref lngStr, 3, 5);
                    }
                    double lat;

                    double lng;

                    if (double.TryParse(latStr, out lat) && double.TryParse(lngStr, out lng))
                    {
                        CoordinateHelper.BaiduToScott(ref lng, ref lat);
                        createOrderEntity.Lat = lat;
                        createOrderEntity.Lng = lng;
                    }
                    createOrderEntity.geoType = 1;
                    createOrderEntity.AllUserCount = 1;
                    createOrderEntity.Remark = string.Empty;
                    createOrderEntity.Status = 0;

                    // result = InvokeCreateOrderService(createOrderEntity);
                    string url = string.Format("http://{0}/api/createOrder", ConfigHelper.APIServer);
                    result = new CreateOrder().InvokeCreateOrderService(url,createOrderEntity);
                    result = result == null ? "0" : "1";
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
