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
    internal class CreateOrderServiceProxy
    {
        private string[] _req;

        public CreateOrderServiceProxy(string request)
        {
            _req = request.Split('|'); 
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="createType">0-- 创建不指定司机预约订单，1--创建指定司机预约订单</param>
        /// <returns></returns>
        public string CreateOrder(string createType)
        {
            PartCreateOrderEntiry partCreateOrderEntiry = BuildPartCreateOrderEntity(createType);
            return InvokeService(partCreateOrderEntiry);
        }

        /// <summary>
        /// 创建Web Service调用参数
        /// </summary>
        /// <param name="createType"></param>
        /// <returns></returns>
        private PartCreateOrderEntiry BuildPartCreateOrderEntity(string createType)
        {
            PartCreateOrderEntiry partCreateOrderEntity = new PartCreateOrderEntiry();

            if (createType == "1")
            {
                // 指定司机预约订单
                //1206|1姓名|2客户手机号|3地址|4司机工号|5lat|6lng|7from|8sign|9IMEI|10MD5串
                partCreateOrderEntity.Address = _req[3];
                partCreateOrderEntity.Imei = _req[9];
                
                partCreateOrderEntity.Lat = Convert.ToDecimal(_req[5].Trim());
                partCreateOrderEntity.Lng = Convert.ToDecimal(_req[6].Trim());
                
                partCreateOrderEntity.Name = _req[1].Trim();
                partCreateOrderEntity.Phone = _req[2].Trim();
                partCreateOrderEntity.Sign = _req[8].ToLower();
                partCreateOrderEntity.Time = DateTime.MinValue;
                partCreateOrderEntity.UCode = _req[4];
            }
            else
            {
                // 不指定司机预约订单
                // 1202|1张三|21381234567|3莘凌路211号(小锦江酒店)|421:00|5asdfas1231asd|6IMEI|7MD5串|8lat|9lng
                partCreateOrderEntity.Address = _req[3].ToLower();
                partCreateOrderEntity.Imei = _req[6];
                if (_req.Length > 8)
                {
                    partCreateOrderEntity.Lat = Convert.ToDecimal(_req[8].Trim());
                    partCreateOrderEntity.Lng = Convert.ToDecimal(_req[9].Trim());
                }
                partCreateOrderEntity.Name = _req[1].Trim();
                partCreateOrderEntity.Phone = _req[2].Trim();
                partCreateOrderEntity.Sign = _req[5].ToLower();
                partCreateOrderEntity.Time = Convert.ToDateTime(_req[4].Trim());
                partCreateOrderEntity.UCode = string.Empty;
            }

            return partCreateOrderEntity;
        }

        private string InvokeService(PartCreateOrderEntiry partCreateOrderEntiry)
        {
            ////判断T_ClentInfo是否有此手机号相关信息
            ////如果用户不存在，创建一个新用户
            //CustomerInfoEntity cinfo = new D_CustomerInfoDal().GetClientInfoByPhone(partCreateOrderEntiry.Phone);
            //string cusid = string.Empty;

            T_ParterEntity parter = new T_PartyDal().GetParterModelBySign(partCreateOrderEntiry.Sign);
            if (parter != null)
            {
                partCreateOrderEntiry.PartId = parter.Id;
                var invokeResult = InvokeCreateOrderService(partCreateOrderEntiry);
                if (invokeResult.Item1)
                {
                    // return 0 or 1
                    return invokeResult.Item2;
                }
                else
                {
                    // error
                    throw new Exception(invokeResult.Item2);
                }
            }
            else
            {
                throw new Exception("商户标识错误。");
            }
        }

        /// <summary>
        /// 调用web service创建订单
        /// UCode = 空 不指定司机预约
        /// </summary>
        /// <param name="createOrderEntity">创建订单参数</param>
        /// <returns></returns>
        private Tuple<bool, string> InvokeCreateOrderService(PartCreateOrderEntiry createOrderEntity)
        {
            string json = JsonConvert.SerializeObject(createOrderEntity);
            bool hasCreated = false;
            string retResult = string.Empty;
            try
            {
                object[] args = new object[1];
                args[0] = json;
                string createOrderService = ConfigHelper.CreateOrderService;
                object obj = DlWebService.InvokeWebService(createOrderService + "/AidaijiaOrder.asmx"
                    , "CreateOrder"
                    , args);

                retResult = obj.ToString();
                hasCreated = retResult == "0" || retResult == "1";
            }
            catch
            {
                throw new Exception("预约订单服务调用失败!");
            }

            return new Tuple<bool, string>(hasCreated, retResult);
        }
    }

    public class PartCreateOrderEntiry
    {
        public string Name { get; set; } // 预约人称呼
        public string Phone { get; set; } // 预约人手机号
        public string Address { get; set; } // 需要代驾的起点
        public DateTime Time { get; set; } // 需要代驾的时间
        public string Sign { get; set; } // 合作商标识
        public decimal Lat { get; set; } // 纬度
        public decimal Lng { get; set; } // 经度
        public int PartId { get; set; } // 合作商Id
        public string UCode { get; set; } // 司机工号
        public string Imei { get; set; } // 手机IMEI
    }
}
