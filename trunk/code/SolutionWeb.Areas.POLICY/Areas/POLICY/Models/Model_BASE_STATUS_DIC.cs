using SolutionWeb.Common;
using SolutionWeb.Model.POLICY;
using SolutionWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

namespace SolutionWeb.Models
{
    public partial class Model_BASE_STATUS_DIC
    {
        public List<EasyUIComBoBoxNode> GetAllStatusList()
        {
            List<EasyUIComBoBoxNode> list = new List<EasyUIComBoBoxNode>();
            list = bs.Entities<BASE_STATUS_DIC>().Where(o => o.S_SHOW == "1").OrderBy(o => o.S_CODE)
                .Select(o => new EasyUIComBoBoxNode {id = o.S_CODE, text = o.S_NAME})
                .ToList();
            return list;
        }

        public BASE_STATUS_DIC GetStatusInfoByCode(string s_code)
        {
            BASE_STATUS_DIC model = new BASE_STATUS_DIC();
            model = bs.Entities<BASE_STATUS_DIC>().FirstOrDefault(o => o.S_CODE==s_code);
            return model;
        }

        #region 计算当前状态以保持时长
        public string GetCalculateStatusTime(DateTime createdt, DateTime handledt)
        {
            string timelong = "";
            TimeSpan ts = new TimeSpan();
            if (handledt == DateTime.MinValue)
            {
                ts = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) -
                     Convert.ToDateTime(createdt.ToString("yyyy-MM-dd"));
            }
            else
            {
                ts = Convert.ToDateTime(handledt.ToString("yyyy-MM-dd")) -
                     Convert.ToDateTime(createdt.ToString("yyyy-MM-dd"));
            }
            return timelong = ts.Days.ToString();
        }
        #endregion
        //public AjaxMsgModel Add(BASE_PROJECT_INFO data)
        //{
        //    AjaxMsgModel amm = new Message().NewAmm;
        //    try
        //    {
        //        if (bs.AddEntity(data) > 0)
        //        {
        //            amm.Statu = AjaxStatu.ok;
        //            amm.Msg = string.Format(Message.OptSussess, "项目", Message.AddOpt);
        //        }
        //        else
        //        {
        //            amm.Statu = AjaxStatu.err;
        //            amm.Msg = string.Format(Message.OptFail, "项目", Message.AddOpt);
        //        }
        //        return amm;
        //    }
        //    catch (Exception ex)
        //    {
        //        RecordLog.RecordError(ex.ToString());
        //        return amm;
        //    }
        //}


    }
}