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
    public partial class Model_BASE_PROJECT_INFO
    {
        public AjaxMsgModel Add(BASE_PROJECT_INFO data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {
                if (bs.AddEntity(data) > 0)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = string.Format(Message.OptSussess, "项目", Message.AddOpt);
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.OptFail, "项目", Message.AddOpt);
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
        public AjaxMsgModel Edit(BASE_PROJECT_INFO info)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {
                if (bs.Entities<BASE_PROJECT_INFO>().Where(o => o.SID == info.SID).Count() == 0)
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.NotFound, "项目");
                    return amm;
                }
                int returnValue = 0;
                using (TransactionScope ts = new TransactionScope())
                {

                    returnValue = bs.UpdateEntity(info, new string[] { "ITEM_TYPE", "ITEM_NAME", "CASHING_WAY", "LAY_ORDER", "ITEM_CODE" });
                    ts.Complete();
                }
                if (returnValue > 0)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = string.Format(Message.OptSussess, "项目", Message.EditOpt);
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.OptFail, "项目", Message.EditOpt);
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
                    returnValue = bs.DelByWhere<BASE_PROJECT_INFO>(m => m.SID == SID);
                    ts.Complete();
                }
                if (returnValue > 0)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = string.Format(Message.OptSussess, "项目", Message.DelOpt);
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

        public List<EasyUITreeNode> GetProjectTree()
        {
            //string strJson = string.Empty;
            //获得组织
            List<string> typeList = bs.Entities<BASE_PROJECT_INFO>().Select(o => o.ITEM_TYPE).Distinct().ToList();
            List<SYS_DEPTANDMEMBER> listOrgMenu = bs.Entities<BASE_PROJECT_INFO>().OrderBy(o=>o.LAY_ORDER)
                                                .Select(org => new SYS_DEPTANDMEMBER
                                                {
                                                    ORGRY_CODE = org.SID,
                                                    PARENT_CODE = org.ITEM_TYPE,
                                                    ORGRY_NAME = org.ITEM_NAME,
                                                    ICO = "",
                                                    RYMOBILE = "",
                                                    STATE = "0"
                                                }).ToList();
            List<EasyUITreeNode> listTreeNodes = new List<EasyUITreeNode>();
            foreach (var item in typeList)
            {
                listTreeNodes.Add(new EasyUITreeNode
                {
                     id = item,
                     text = item,
                     children = SYS_DEPTANDMEMBER.ConvertTreeNodes(listOrgMenu, item, "", false)
                });
            }
            
            //strJson = ObjToJson.GetToJson(listTreeNodes).Replace("Checked", "checked");
            return listTreeNodes;
        }
    }
}