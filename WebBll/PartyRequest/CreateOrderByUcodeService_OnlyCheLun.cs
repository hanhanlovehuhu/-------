using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using WebDal;
using WebModel;
using WebUtility;

namespace WebBll.PartyRequest
{
    class CreateOrderByUcodeService_OnlyCheLun : JsonCommand
    {
        /// <summary>
        /// 立即预约，成功返回订单号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string execute(string request)
        {
            //1217|1姓名|2客户手机号|3地址|4司机工号|5lat|6lng|7from|8sign|9IMEI|10MD5串|11remark|12youhui
            string result = "0";
            string[] req = request.Split('|');
            var sign = EncodingUtil.GetMd5(req[2] + "aidaijia", "utf-8");
            if (sign.ToLower() == req[10].ToLower())
            {
                ThirdPartyCoupon cc = new ThirdPartyCoupon();
                //判断T_ClentInfo是否有此手机号相关信息
                //如果用户不存在，创建一个新用户
                CustomerInfoEntity cinfo = new D_CustomerInfoDal().GetClientInfoByPhone(req[2].Trim());

                T_ParterEntity parter = new T_PartyDal().GetParterModelBySign(req[8].ToLower());
                string couponCode = "";
                if (parter != null)
                {
                    #region 调用冯超接口检查是否新用户
                    if (InvokeStateService(req[2].Trim()).Equals(1))
                    {
                        if (parter.url != null)
                        {
                            cc.isCoupon(parter.url);
                            if (!cc.check(req[12], req[13], parter.url))
                            {
                                throw new Exception("优惠码无效");
                            }
                            couponCode = req[11];
                        }
                    } 
                    #endregion
                    #region 优惠码操作
                    
                    //if (req.Length == 14 && parter.url != null)
                    //{
                        
                    //    if (!cc.check(req[12], req[13], parter.url))
                    //    {
                    //        throw new Exception("优惠码无效");
                    //    }
                    //    //couponCode = cc.addCoupon(parter.Id.ToString(), req[12], req[13], cinfo);
                    //}
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
                        createOrderEntity.RecommendCode = couponCode;
                        createOrderEntity.Parterid = parter.Id;
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
                        if (double.TryParse(latStr, out lat))
                        {
                            createOrderEntity.Lat = lat;
                        }

                        double lng;
                        if (double.TryParse(lngStr, out lng))
                        {
                            createOrderEntity.Lng = lng;
                        }
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

        

        
        private string InvokeStateService(string cellPhone)
        {
            string orderInfos = string.Empty;
            try
            {
                WebRequest request = null;
                WebResponse response = null;
                string url = ConfigHelper.AdjOrderService + "/AidaijiaServer.asmx/getIsFirstOrder?cellPhone={0}";
                request = WebRequest.Create(string.Format(url, cellPhone));
                response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string strMsg = reader.ReadToEnd();
                    return strMsg;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("查询订单服务调用失败!");
            }
            
        }

        
        

        public class NewCustomer
        {
            /// <summary>
            /// 优惠码
            /// </summary>
            public string CouponCode { set; get; }
            /// <summary>
            /// 价格
            /// </summary>
            public decimal Price { set; get; }
            /// <summary>
            /// 图片路径
            /// </summary>
            public string ImgPath { set; get; }
            /// <summary>
            /// 图片链接
            /// </summary>
            public string UrlPath { set; get; }
            /// <summary>
            /// 图片说明
            /// </summary>
            public string ImgAlt { set; get; }
            /// <summary>
            /// 图片背影颜色
            /// </summary>
            public string ImgBackColor { set; get; }


        }
        
    }
}
