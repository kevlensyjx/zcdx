using SolutionWeb.Common;
using SolutionWeb.Model.POLICY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

namespace SolutionWeb.Models
{
    public partial class Model_CORPORATION_BASE_INFO
    {
        #region 修改项目
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="RoleInfo">角色</param>
        /// <returns>AjaxMsgModel实体对象</returns>
        public AjaxMsgModel Edit(CORPORATION_BASE_INFO info)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {
                int returnValue = 0;
                using (TransactionScope ts = new TransactionScope())
                {

                    returnValue = bs.UpdateEntity(info, new string[] { "APPLY_RESULT", "CORP_STATUS" });
                    ts.Complete();
                }
                if (returnValue > 0)
                {
                    //SMSHelper helper = new SMSHelper();
                    //helper.SendSMS("", info.LEGAL_PERSON_PHONE);
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = string.Format(Message.OptSussess, "企业注册信息", "审批");
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.OptFail, "企业注册信息", "审批");
                }
            }
            catch (Exception ex)
            {
                RecordLog.RecordError(ex.ToString());
                return amm;
            }

            return amm;
        }
        #endregion
    }
}