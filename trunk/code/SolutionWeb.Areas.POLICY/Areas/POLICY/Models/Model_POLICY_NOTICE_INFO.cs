
using SolutionWeb.Common;
using SolutionWeb.Model.POLICY;
using SolutionWeb.Model.SYS;
using SolutionWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

namespace SolutionWeb.Models
{
    public partial class Model_POLICY_NOTICE_INFO
    {
        public  AjaxMsgModel Edit(VIEW_POLICY_NOTICE_INFO model)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            amm.Statu = AjaxStatu.err;
            amm.Msg = "录入失败！";
            try
            {
                 
                return amm;
            }
            catch (Exception ex)
            {
                RecordLog.RecordError(ex.ToString());
                return amm;
            }
        }

        public AjaxMsgModel AddPublicity(VIEW_POLICY_NOTICE_INFO model)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            amm.Statu = AjaxStatu.err;
            amm.Msg = "录入失败！";
            try
            {
                List<string> listHouseId = model.SID.Split(',').ToList();
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