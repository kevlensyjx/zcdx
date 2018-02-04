using SolutionWeb.Common;
using SolutionWeb.Model.POLICY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

namespace SolutionWeb.User.Models
{
    public partial class Model_CORPORATION_BASE_INFO
    {
        public AjaxMsgModel Add(CORPORATION_BASE_INFO data, POLICY_APPLY_FILE yyzzfile, POLICY_APPLY_FILE zzjgfile)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {
                int returnValue = 0;
                using (TransactionScope ts = new TransactionScope())
                {
                    returnValue = bs.AddEntity(data);
                    returnValue += bs.AddEntity(yyzzfile);
                    returnValue += bs.AddEntity(zzjgfile);
                    
                    ts.Complete();
                }
                if (returnValue == 3)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = "注册成功，请等待管理员审核，审核通过后会以短信方式通知您！";
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = "注册失败";
                }
                
                return amm;
            }
            catch (Exception ex)
            {
                RecordLog.RecordError(ex.ToString());
                return amm;
            }
        }
        
    }
}