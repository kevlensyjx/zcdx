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
    public partial class Model_BASE_WORKFLOW_INFO
    {
        public AjaxMsgModel Add(BASE_WORKFLOW_INFO data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {
                if (bs.AddEntity(data) > 0)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = string.Format(Message.OptSussess, "流程", Message.AddOpt);
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.OptFail, "流程", Message.AddOpt);
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
        public AjaxMsgModel Edit(BASE_WORKFLOW_INFO info)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {
                if (bs.Entities<BASE_WORKFLOW_INFO>().Where(o => o.SID == info.SID).Count() == 0)
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.NotFound, "流程");
                    return amm;
                }
                int returnValue = 0;
                using (TransactionScope ts = new TransactionScope())
                {

                    returnValue = bs.UpdateEntity(info, new string[] { "ITEM_TYPE", "STATUS_NAME", "STATUS_CODE", "HANDLE_RESULT", "NEXT_STATUS_NAME", "NEXT_STATUS_CODE", "TIME_LIMIT" });
                    ts.Complete();
                }
                if (returnValue > 0)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = string.Format(Message.OptSussess, "流程", Message.EditOpt);
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.OptFail, "流程", Message.EditOpt);
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
                    returnValue = bs.DelByWhere<BASE_WORKFLOW_INFO>(m => m.SID == SID);
                    ts.Complete();
                }
                if (returnValue > 0)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = string.Format(Message.OptSussess, "流程", Message.DelOpt);
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.OptFail, "流程", Message.DelOpt);
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

        #region 查看我的组织机构除工区树
        /// <summary>
        /// 获取组织机构树
        /// </summary>
        /// <returns>PARENT_CODE所在部门的父ID</returns>
        [Skip]
        [AjaxRequest]
        public List<EasyUITreeNode> GetMyORGNoGQTree(string DEPT_CODE, string PARENT_CODE, int DEPT_FLAG = 0)
        {
            string strJson = string.Empty;
            //获得组织
            List<SYS_DEPTANDMEMBER> listOrgMenu = bs.Entities<SYS_DEPT>().Where(m => m.DEL_FLAG == "0" && m.C_ICO == "icon-DepartMent" && m.DEPT_FLAG == DEPT_FLAG && (m.PARENT_CODE.StartsWith(DEPT_CODE) || m.DEPT_CODE == DEPT_CODE))
                .OrderBy(m => m.DEPT_ORDER).ThenBy(m => m.DEPT_CODE)
                                                .Select(org => new SYS_DEPTANDMEMBER
                                                {
                                                    ORGRY_CODE = org.DEPT_CODE,
                                                    PARENT_CODE = org.PARENT_CODE,
                                                    ORGRY_NAME = org.DEPT_NAME,
                                                    ICO = org.C_ICO,
                                                    RYMOBILE = "",
                                                    STATE = org.STATUS_FLAG//0关闭
                                                })
                                                .ToList();
            if (DEPT_FLAG == 1)
            {
                listOrgMenu = GetDeptTree(listOrgMenu);
            }
            List<EasyUITreeNode> listTreeNodes = SYS_DEPTANDMEMBER.ConvertTreeNodes(listOrgMenu, PARENT_CODE, DEPT_CODE, false);
            //strJson = ObjToJson.GetToJson(listTreeNodes).Replace("Checked", "checked");
            return listTreeNodes;
        }
        #endregion

        #region MyRegion
        public List<SYS_DEPTANDMEMBER> GetDeptTree(List<SYS_DEPTANDMEMBER> listOrgMenu)
        {

            List<string> deptcodeList = new List<string>();
            foreach (SYS_DEPTANDMEMBER item in listOrgMenu)
            {
                string deptcode = item.ORGRY_CODE;
                deptcodeList.Add(deptcode);
                if (deptcode.Length > 8) 
                {
                    deptcodeList.Add(deptcode.Substring(0, 8));
                }
                if (deptcode.Length > 6) 
                {
                    deptcodeList.Add(deptcode.Substring(0, 6));
                }
                if (deptcode.Length > 4) 
                {
                    deptcodeList.Add(deptcode.Substring(0, 4));
                }
                if (deptcode.Length > 2) 
                {
                    deptcodeList.Add(deptcode.Substring(0, 2));
                }
            }
            List<SYS_DEPTANDMEMBER> listOrg = bs.Entities<SYS_DEPT>().Where(m => deptcodeList.Contains(m.DEPT_CODE))
                                             .OrderBy(m => m.DEPT_ORDER).ThenBy(m => m.DEPT_CODE)
                                            .Select(org => new SYS_DEPTANDMEMBER
                                            {
                                                ORGRY_CODE = org.DEPT_CODE,
                                                PARENT_CODE = org.PARENT_CODE,
                                                ORGRY_NAME = org.DEPT_NAME,
                                                ICO = org.C_ICO,
                                                RYMOBILE = "",
                                                STATE = org.STATUS_FLAG//0关闭
                                            })
                                            .ToList();
            return listOrg;
        }
        #endregion

        public  List<EasyUIComBoBoxNode> GetFlowStepInfo(VIEW_BASE_WORKFLOW_INFO model)
        {
            List<EasyUIComBoBoxNode> list = new List<EasyUIComBoBoxNode>();
            List<BASE_WORKFLOW_INFO> worlflowEntities = bs.Entities<BASE_WORKFLOW_INFO>().ToList();
            if (model.BEFORE_FLAG == "1")
            {
                //worlflowEntities = worlflowEntities.Where(o =>  o.STATUS_CODE < model.STATUS_CODE && o.ITEM_TYPE == model.ITEM_TYPE);
                foreach (var item in worlflowEntities)
                {
                    int status_code = Convert.ToInt16(item.STATUS_CODE);
                    int status_code_source = Convert.ToInt16(model.STATUS_CODE);
                    if (status_code < status_code_source)
                    {
                        list.Add(new EasyUIComBoBoxNode { id = item.STATUS_CODE, text = item.STATUS_NAME });
                    }
                }
            }
            else if (model.BEFORE_FLAG == "0")
            {
                List<BASE_STATUS_DIC> statusdicEntities =
                    bs.Entities<BASE_STATUS_DIC>().Where(o => o.S_SHOW == "1").ToList();
                foreach (var item in statusdicEntities)
                {
                    int status_code = Convert.ToInt16(item.S_CODE);
                    int status_code_source = Convert.ToInt16(model.STATUS_CODE);
                    if (status_code > status_code_source)
                    {
                        list.Add(new EasyUIComBoBoxNode { id = item.S_CODE, text = item.S_NAME });
                    }
                }
            }
            if (list.Count == 0)
            {
                list.Add(new EasyUIComBoBoxNode { id = "000", text = "---" });
            }
            return list;
        }
    }
}