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
    public partial class Model_BASE_PROJECT_CONFIG
    {
        public AjaxMsgModel Add(BASE_PROJECT_CONFIG data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {
                if (bs.AddEntity(data) > 0)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = string.Format(Message.OptSussess, "项目配置", Message.AddOpt);
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.OptFail, "项目配置", Message.AddOpt);
                }
                return amm;
            }
            catch (Exception ex)
            {
                RecordLog.RecordError(ex.ToString());
                return amm;
            }
        }

        #region 修改项目
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="RoleInfo">角色</param>
        /// <returns>AjaxMsgModel实体对象</returns>
        public AjaxMsgModel Edit(BASE_PROJECT_CONFIG info)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {
                if (bs.Entities<BASE_PROJECT_CONFIG>().Where(o => o.SID == info.SID).Count() == 0)
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.NotFound, "项目配置");
                    return amm;
                }
                int returnValue = 0;
                using (TransactionScope ts = new TransactionScope())
                {

                    returnValue = bs.UpdateEntity(info, new string[] { "PROJECT_SID", "DEPT_TYPE", "DEPT_CODE", "DEPT_NAME" });
                    ts.Complete();
                }
                if (returnValue > 0)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = string.Format(Message.OptSussess, "项目配置", Message.EditOpt);
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.OptFail, "项目配置", Message.EditOpt);
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

        #region 删除项目
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="RoleInfo">角色</param>
        /// <returns>AjaxMsgModel实体对象</returns>
        public AjaxMsgModel Del(string SID)
        {
            AjaxMsgModel amm = new Message().NewAmm;

            try
            {
                int returnValue = 0;
                using (TransactionScope ts = new TransactionScope())
                {
                    returnValue = bs.DelByWhere<BASE_PROJECT_CONFIG>(m => m.PROJECT_SID == SID);
                    ts.Complete();
                }
                if (returnValue > 0)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = string.Format(Message.OptSussess, "项目配置", Message.DelOpt);
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.OptFail, "项目", Message.DelOpt);
                }
                return amm;
            }
            catch (Exception ex)
            {
                RecordLog.RecordError(ex.ToString());
                return amm;
            }
        }
        #endregion
    }
}