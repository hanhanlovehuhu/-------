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
    class CreateOrderByUcodeBll : JsonCommand
    {
        public string execute(string request)
        {
            //1206|1姓名|2客户手机号|3地址|4司机工号|5lat|6lng|7from|8sign|9IMEI|10MD5串
            string result = "0";
            string[] req = request.Split('|');
            var sign = EncodingUtil.GetMd5(req[2] + "aidaijia", "utf-8");
            if (sign.ToLower() == req[10].ToLower())
            {

                //判断T_ClentInfo是否有此手机号相关信息
                //如果用户不存在，创建一个新用户
                CustomerInfoEntity cinfo = new D_CustomerInfoDal().GetClientInfoByPhone(req[2].Trim());
                string cusid = string.Empty;
                int CustomerType = 1;
                int AccountType = 2;


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
                                RegisterFrom = 5,              //第三方API 
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

                    #region 创建指定司机派单信息

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
                        //直接派单
                        string distributeOrderId = new DistributeOrderInfoDAL().GenerateDistributeOrderId(new DistributeOrderInfoDAL().SelectSeqOrder());
                        string lat = req[5].Trim();
                        string lng = req[6].Trim();
                        if (parter.Id == 11)//判断是博泰，对坐标进行转换
                        {
                            ChangeCoords.ChangeCoordinate(ref lat, ref lng, 3, 5);
                        }
                        DistributeOrderInfoEntity model = new DistributeOrderInfoEntity()
                        {
                            Address = req[3].Trim(),
                            AppointOrderId = null,
                            BusinessType = 10,            //10酒后代驾
                            Cellphone = req[2].Trim(),        //客户手机号
                            CustomerId = cusid,
                            CustomerName = req[1].Trim(),
                            CustomerType = CustomerType,
                            AccountType = AccountType,
                            AppointmentTime = DateTime.Now,
                            DistributeOrderId = distributeOrderId,
                            DistributeUser = cusid,
                            DistributeUserType = 3,
                            Fromcellphone = req[2].Trim(),
                            Lat = float.Parse(lat),
                            Lng = float.Parse(lng),
                            OrderFrom = 5,//由8改为5
                            OrderId = null,
                            OrderType = 1,
                            Status = 11,
                            Create_user = "ThirdPartyAPI",
                            Create_time = DateTime.Now,
                            Remark = "",
                            Ucode = req[4].Trim(),
                            PartyId = parter.Id
                        };


                        bool istrue = new DistributeOrderInfoDAL().AddDistributeOrderInfo(model);

                        //修改派单状态：派单中
                        bool isupdate = new DistributeOrderInfoDAL().UpdateOnDoService(1, req[4].Trim());


                        DistributeOrderDetailEntity detail = new DistributeOrderDetailEntity()
                        {
                            DistributeOrderId = distributeOrderId,
                            Status = 0,
                            Ucode = req[4].Trim(),
                            Create_user = "Party CreateOrder"
                        };
                        bool OrderDetail = new D_DistributeOrderDetailDal().CreateDistributeOrderDetail(detail);

                        DistributeOrderStatusChangeLogEntity log = new DistributeOrderStatusChangeLogEntity()
                        {
                            DistributeOrderId = distributeOrderId,
                            Status = 11,
                            StatusText = "派单已由第三方合作商接口分配给指定司机工号" + req[4].Trim(),
                            Create_user = "Party CreateOrder"
                        };
                        bool OrderStatusChangeLog = new D_DistributeOrderStatusChangeLogDal().CreateDistributeOrderStatusChangeLog(log);

                        if (!istrue || !isupdate)
                        {
                            return "0";
                        }
                        else
                        {
                            //添加数据
                            T_ParterDyLog tpl = new T_ParterDyLog();
                            tpl.sign = req[8];
                            tpl.lat = "0";
                            tpl.lng = "0";
                            tpl.imei = req[9];
                            tpl.addtime = DateTime.Now;
                            //1:获取司机2:上传通话3:上传预约
                            tpl.typeid = 3;
                            new T_ParterDyLogDal().AddParterDyLog(tpl);
                            result = "1";
                        }
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
