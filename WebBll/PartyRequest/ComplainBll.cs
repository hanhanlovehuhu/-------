using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDal;
using WebDal.Party;
using WebModel;
using WebUtility;

namespace WebBll.PartyRequest
{
    class ComplainBll : JsonCommand
    {
        // 1214|sign|orderid|CustomerCellPhone|Ucode|DriverName|ComplaintContent|7a31f99279327f8b75506acbf0503973
        public string execute(string request)
        {
            int flag = 0;
            string[] req = request.Split('|');
            var sign = EncodingUtil.GetMd5(req[2] + "aidaijia", "utf-8");
            if (sign.ToLower() == req[7].ToLower())
            {
                T_ParterEntity parter = new T_PartyDal().GetParterModelBySign(req[1].ToLower());
                if (parter != null)
                {
                    string orderCellPhone = "";
                    string orderUcode = "";
                    new D_OrderInfoDal().getOrderCellphone_Ucode(req[2], out orderCellPhone, out orderUcode);
                    if (!orderCellPhone.ToLower().Equals(req[3].ToLower()))
                    {
                        throw new Exception("没有该用户");
                    }
                    if (!orderUcode.ToLower().Equals(req[4].ToLower()))
                    {
                        throw new Exception("没有该司机");
                    }
                    CustomerInfoEntity ci = new D_CustomerInfoDal().GetClientInfoByPhone(orderCellPhone);
                    D_OrderComplaint dc = new D_OrderComplaint()
                    {
                        Cellphone = ci.Cellphone,
                        OrderId = req[2],
                        CustomerId = ci.CustomerId,
                        CustomerName = ci.CustomerName,
                        Ucode = req[4],
                        ComplaintContent = req[6],
                        DriverName = req[5],
                        CreateUser = "Third Party"
                    };
                    flag = new ComplainDAL().addComplain(dc);
                    
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
            return flag.ToString();
        }
    }
}
