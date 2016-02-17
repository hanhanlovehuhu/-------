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

namespace WebBll
{
    internal class CreateOrderByUcodeServiceBll : JsonCommand
    {
        LogHelper log = new LogHelper();
        public string execute(string request)
        {
            //1206|1姓名|2客户手机号|3地址|4司机工号|5lat|6lng|7from|8sign|9IMEI|10MD5串|11youhui|12youhuijine
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
                        if (req.Length == 13 && parter.url != null)
                        {
                            ThirdPartyCoupon cc = new ThirdPartyCoupon();
                            if (!cc.check(req[11], req[12], parter.url))
                            {
                                throw new Exception("优惠码无效");
                            }
                            couponCode = cc.addCoupon(parter.Id.ToString(), req[11], req[12], cinfo);
                        }
                        log.log("1206IF:【" + request + "】CLOSESIGN【" + WebUtility.ConfigHelper.CloseSign.ToLower() + "】req[8]【" + req[8].ToLower() + "】", System.Web.HttpContext.Current.Server.MapPath("Log/log.txt"));
                    }
                    else
                    {
                        log.log("1206ELSE:【" + request + "】CLOSESIGN【" + WebUtility.ConfigHelper.CloseSign.ToLower() + "】req[8]【" + req[8].ToLower() + "】", System.Web.HttpContext.Current.Server.MapPath("Log/log.txt"));
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
                        string latStr = req[5].Trim();
                        string lngStr = req[6].Trim();
                        if (parter.Id == 11)//判断是博泰，对坐标进行转换
                        {
                            ChangeCoords.ChangeCoordinate(ref latStr, ref lngStr, 3, 5);
                        }
                        createOrderEntity.Ucode = req[4].Trim();
                        createOrderEntity.Parterid = parter.Id;
                        createOrderEntity.RecommendCode = "[" + couponCode;
                        createOrderEntity.BusinessType = 1; // 酒后代驾
                        createOrderEntity.OrderFrom = 5; // 合作商
                        createOrderEntity.AppointTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        createOrderEntity.Address = req[3].Trim();
                        createOrderEntity.FromCellPhone = req[2].Trim();
                        createOrderEntity.CellPhone = req[2].Trim();
                        double lat;
                        double lng;
                        if (double.TryParse(latStr, out lat)&&double.TryParse(lngStr, out lng))
                        {
                             CoordinateHelper.BaiduToScott(ref lng, ref lat);
                             createOrderEntity.Lat = lat;
                             createOrderEntity.Lng = lng;
                        }
                        createOrderEntity.geoType = 1;
                        createOrderEntity.AllUserCount = 1;
                        createOrderEntity.Remark = string.Empty;
                        createOrderEntity.Status = 0;
                        string url = string.Format("http://{0}/api/createOrder", ConfigHelper.APIServer);
                        result = new CreateOrder().InvokeCreateOrderService(url,createOrderEntity);
                        result = result == null ? "0" : "1";
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

        /// <summary>
        /// 调用web service创建订单
        /// </summary>
        /// <param name="createOrderEntity">创建订单参数</param>
        /// <returns></returns>
        private string InvokeCreateOrderService(CreateOrderServiceRequestEntity createOrderEntity)
        {
            string json = JsonConvert.SerializeObject(createOrderEntity);
            string result = null;

            try
            {
                object[] args = new object[1];
                args[0] = json;
                string createOrderService = ConfigHelper.AdjOrderService;
                object obj = DlWebService.InvokeWebService(createOrderService + "/AidaijiaServer.asmx"
                    , "CreateOrder"
                    , args);
                string[] orderIDs = JsonHelper.GetObject<string[]>(obj.ToString());
                result = orderIDs[0] == null ? "0" : "1";
            }
            catch
            {
                throw new Exception("预约订单服务调用失败!");
            }

            return result;
        }

    }

    /// <summary>
    /// Socket 开发文档V1.7
    /// 用户下单请求信息
    /// </summary>
    public class CreateOrderServiceRequestEntity
    {
        public string OrderId { get; set; }             // 订单id（可空）
        public string Ucode { get; set; }               // 司机id（可空）
        //public string CustomerId { get; set; }          // 用户id
        //public string CustomerName { get; set; }        // 用户名称
        public int Parterid { get; set; }               // 爱代驾自己平台为0其它为合作商ID
        public int BusinessType { get; set; }           // 1，酒后代驾；2，婚庆代驾；3，商务代驾；4，长途代驾；5，旅游代驾
        //public int CustomerType { get; set; }           // 1 个人 2集团
        //public int AccountType { get; set; }            // 1 vip 2普通
        public int OrderFrom { get; set; }              // 1:400,2:android APP,3:ios APP 4:爱推广,5:合作商
        public string AppointTime { get; set; }         // 预约时间（可空）
        //public string AppointTimeStr { get; set; }      // 预约时间
        public string Address { get; set; }             // 地址
        public string FromCellPhone { get; set; }           // 来电号码（可空）
        public string CellPhone { get; set; }           // 客户电话
        public double Lat { get; set; }                 // 纬度
        public double Lng { get; set; }                 // 经度
        public int AllUserCount { get; set; }           // 订单总人数
        public string Remark { get; set; }              // 备注（可空）
        public int Status { get; set; }                 // 状态
        public string RecommendCode { get; set; } //优惠码
        public int geoType { get; set; }     //地图类型标识：0默认百度，1高德。
    }

    public class CreateOrderServiceResponseEntity
    {
        public int Status { get; set; }             // 0 -- 失败，1 -- 成功
        public string Message { get; set; }         // Status = 0 时为错误消息，Status = 1时为订单号
    }
}
