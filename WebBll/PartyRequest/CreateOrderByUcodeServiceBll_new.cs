using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using WebDal;
using WebModel;
using WebUtility;
using System.Net;
using System.IO;

namespace WebBll.PartyRequest
{
    internal class CreateOrderByUcodeServiceBll_new : JsonCommand
    {
        LogHelper log = new LogHelper();

        /// <summary>
        /// 立即预约，成功返回订单号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string execute(string request)
        {
            //1212|1姓名|2客户手机号|3地址|4司机工号|5lat|6lng|7from|8sign|9IMEI|10MD5串|11remark|12youhui|13youhuijine
            string result = "0";
            string[] req = request.Split('|');
            var sign = EncodingUtil.GetMd5(req[2] + "aidaijia", "utf-8");
            if (sign.ToLower() == req[10].ToLower())
            {

                //判断T_ClentInfo是否有此手机号相关信息
                //如果用户不存在，创建一个新用户
                CustomerInfoEntity cinfo = new D_CustomerInfoDal().GetClientInfoByPhone(req[2].Trim());

                T_ParterEntity parter = new T_PartyDal().GetParterModelBySign(req[8].ToLower());
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
                                RegisterFrom = 5,              //第三方API 
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
                    if (!WebUtility.ConfigHelper.CloseSign.ToLower().Contains(req[8].ToLower()))
                    {
                        if (req.Length == 14 && parter.url != null)
                        {
                            ThirdPartyCoupon cc = new ThirdPartyCoupon();
                            if (!cc.check(req[12], req[13], parter.url))
                            {
                                throw new Exception("优惠码无效");
                            }
                            couponCode = cc.addCoupon(parter.Id.ToString(), req[12], req[13], cinfo);
                        }
                        log.log("1212IF:【" + request + "】CLOSESIGN【" + WebUtility.ConfigHelper.CloseSign.ToLower() + "】req[8]【" + req[8].ToLower() + "】", System.Web.HttpContext.Current.Server.MapPath("Log/log.txt"));
                    }
                    else
                    {
                        log.log("1212ELSE:【" + request + "】CLOSESIGN【" + WebUtility.ConfigHelper.CloseSign.ToLower() + "】req[8]【" + req[8].ToLower() + "】", System.Web.HttpContext.Current.Server.MapPath("Log/log.txt"));
                    }
                    #endregion

                    #region 调用web service 处理创建指定司机派单信息

                    DriverInfoEntity driverInfo = new D_DriverInfoDal().GetDriverInfoByUcode(req[4].Trim());
                    if (driverInfo == null)
                    {
                        throw new Exception("指定司机不存在！");
                    }
                    if (driverInfo.Online == 0)
                    {
                        throw new Exception("该司机已经下线，不能接受您指派的任务！");
                    }
                    else if (driverInfo.OnService == 1 || driverInfo.OnDOService == 1)
                    {
                        throw new Exception("该司机已经在服务中了，不能接受您指派的任务！");
                    }
                    else
                    {
                        CreateOrderServiceRequestEntity createOrderEntity = new CreateOrderServiceRequestEntity();
                        createOrderEntity.Ucode = req[4].Trim();
                        createOrderEntity.Parterid = parter.Id;
                        createOrderEntity.RecommendCode = "[" + couponCode;
                        createOrderEntity.BusinessType = 1; // 酒后代驾
                        createOrderEntity.OrderFrom = 5; // 合作商
                        createOrderEntity.AppointTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        createOrderEntity.Address = req[3].Trim();
                        createOrderEntity.FromCellPhone = req[2].Trim();
                        createOrderEntity.CellPhone = req[2].Trim();
                        string latStr = req[5].Trim();
                        string lngStr = req[6].Trim();
                        if (parter.Id == 11)//判断是博泰，对坐标进行转换
                        {
                            ChangeCoords.ChangeCoordinate(ref latStr, ref lngStr, 3, 5);
                        }
                        double lat;                           

                        double lng;                           
                        
                        if (double.TryParse(latStr, out lat)&&double.TryParse(lngStr, out lng))
                        {
                            CoordinateHelper.BaiduToScott(ref lng,ref lat);
                            createOrderEntity.Lat = lat;
                            createOrderEntity.Lng = lng;
                        }
                        createOrderEntity.geoType = 1;
                        createOrderEntity.AllUserCount = 1;
                        createOrderEntity.Remark = req[11].Trim();
                        createOrderEntity.Status = 0;
                        string url = string.Format("http://{0}/api/createOrder", ConfigHelper.APIServer);
                        result = new CreateOrder().InvokeCreateOrderService(url, createOrderEntity);
                        
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
