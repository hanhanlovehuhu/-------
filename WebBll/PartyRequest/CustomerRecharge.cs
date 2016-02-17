using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDal;
using WebModel;
using WebUtility;

namespace WebBll.PartyRequest
{
    public class CustomerRecharge : JsonCommand
    {

        #region Private Properties
        D_CustomerRechargeDA dcr;
        #endregion

        #region   Constructors
        public CustomerRecharge()
        {
            dcr = new D_CustomerRechargeDA();
        }
        #endregion

        #region public Methods
        public string execute(string request)
        {
            //1220|1|6|U0089023|某某|38.00|MV2JL366U9H1|1|3fe5d0205970e8a732686c7cc2e99999|5874bddc503680232ffab25fbd9f4b6e 
            string[] req = request.Split('|');
            var sign = EncodingUtil.GetMd5(req[6] + "aidaijia", "utf-8");
            if (sign.ToLower() == req[9].ToLower())
            {
                T_ParterEntity parter = new T_PartyDal().GetParterModelBySign(req[8].ToLower());
                if (parter != null)
                {
                    D_CustomerRechargeEntity dcre = new D_CustomerRechargeEntity();
                    CustomerInfoEntity ci = new D_CustomerInfoDal().GetClientInfoByPhone(req[3]);

                    string cn = string.IsNullOrEmpty(ci.CustomerId) ? string.Empty : ci.CustomerId;

                    dcre.RechargeType =Convert.ToInt32(req[1]);
                    dcre.From = Convert.ToInt32(req[2]);
                    dcre.CustomerName = req[4];
                    dcre.CustomerId = cn;
                    dcre.Amount = Convert.ToDecimal(req[5]);
                    dcre.Status = 0;
                    dcre.create_user = "PartyRequest";
                    dcre.OrderId = req[6];
                    dcre.SubRechargeType = Convert.ToInt32(req[7]);
                    string RechargeId = AddCustomerRecharge(dcre);
                    return JsonHelper.ToJson<object>(new { out_trade_no = RechargeId });
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
        #endregion

        #region Private Methods
        private string AddCustomerRecharge(D_CustomerRechargeEntity CostomerEn)
        {
            int seq = dcr.SelectSeq();
            string ICId = string.Empty;
            if (!string.IsNullOrEmpty(CostomerEn.OrderId))
            {
                ICId = CustomerRechargeIdByThirdParty(seq);
            }
            D_CustomerRechargeEntity entity = new D_CustomerRechargeEntity()
            {
                RechargeId = ICId,
                RechargeType = CostomerEn.RechargeType,
                From = CostomerEn.From,
                CustomerId = CostomerEn.CustomerId,
                Amount = CostomerEn.Amount,
                create_user = CostomerEn.create_user,
                Status = CostomerEn.Status,
                CustomerName = CostomerEn.CustomerName,
                OrderId = CostomerEn.OrderId,
                SubRechargeType = CostomerEn.SubRechargeType
            };
            dcr.Insert(entity);
            return ICId;
        }
        /// <summary>
        /// 根据一个种子序列来生成一个充值编号
        /// 以大写字母PC起始，后边为数字，总长为9位，左边不足补0
        /// 例如：PC0000085
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        private string CustomerRechargeIdByThirdParty(int seq)
        {
            string returnStr = string.Empty;
            Number number = new Number("1234567890");
            string code = number.ToString(seq);
            int count = code.Length;
            string buStr = string.Empty;
            for (int i = 0; i < 7 - count; i++)
            {
                buStr += @"0";
            }
            returnStr = "PC" + buStr + code;
            return returnStr;
        }
        #endregion
    }
}
