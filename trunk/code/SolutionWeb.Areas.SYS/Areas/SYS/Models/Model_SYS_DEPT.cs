using SolutionWeb.Common;
using SolutionWeb.Model.SYS;
using SolutionWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolutionWeb.Models
{
    public partial class Model_SYS_DEPT
    {

        #region 得到我的组织机构集合
        public  List<EasyUITreeNode> GetMyDEPTTree(string DEPT_CODE, string PARENT_CODE, int DEPT_FLAG = 0)
        {
            //string strJson = string.Empty;
            //获得组织
            List<SYS_DEPTANDMEMBER> listOrgMenu = bs.Entities<SYS_DEPT>()
                .Where(m => m.DEL_FLAG == "0" && m.DEPT_FLAG == DEPT_FLAG && (m.PARENT_CODE.StartsWith(DEPT_CODE)
                    || m.DEPT_CODE == DEPT_CODE)).OrderBy(m => m.DEPT_ORDER).ThenBy(m => m.DEPT_CODE)
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

        #region 查看我的组织机构包含工区树
        /// <summary>
        /// 获取组织机构树
        /// </summary>
        /// <returns>PARENT_CODE所在部门的父ID</returns>
        public List<EasyUITreeNode> GetMyORGTree(string DEPT_CODE, string PARENT_CODE, int DEPT_FLAG = 0)
        {
            //string strJson = string.Empty;
            //获得组织
            IQueryable<SYS_DEPT> listOrg = bs.Entities<SYS_DEPT>().Where(m => m.DEL_FLAG == "0" && m.DEPT_FLAG == DEPT_FLAG && (m.PARENT_CODE.StartsWith(DEPT_CODE) || m.DEPT_CODE == DEPT_CODE));


            List<SYS_DEPTANDMEMBER> listOrgMenu = listOrg.OrderBy(m => m.DEPT_ORDER).ThenBy(m => m.DEPT_CODE)
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


        #region 查看我的组织机构除工区树
        /// <summary>
        /// 获取组织机构树
        /// </summary>
        /// <returns>PARENT_CODE所在部门的父ID</returns>
        [Skip]
        [AjaxRequest]
        public  List<EasyUITreeNode> GetMyORGNoGQTree(string DEPT_CODE, string PARENT_CODE, int DEPT_FLAG = 0)
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
                if (deptcode.Length > 8)//工区
                {
                    deptcodeList.Add(deptcode.Substring(0, 8));
                }
                if (deptcode.Length > 6)//车间
                {
                    deptcodeList.Add(deptcode.Substring(0, 6));
                }
                if (deptcode.Length > 4)//虚拟车间
                {
                    deptcodeList.Add(deptcode.Substring(0, 4));
                }
                if (deptcode.Length > 2)//局
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


        #region 将组织机构转为EASYUI树
        private EasyUIDEPTTree TransformTreeNode(SYS_DEPT menu)
        {
            //string orgOpenState = ConfigurationSettings.AppSettings["OrgOpenState"];
            EasyUIDEPTTree easyUITreeNode = new EasyUIDEPTTree()
            {
                DEPT_CODE = menu.DEPT_CODE,
                deptname = menu.DEPT_NAME,
                state = menu.STATUS_FLAG.Equals("1") ? "open" : "closed",//只有存在下级才可设为closed，否则会循环查询
                iconCls = menu.C_ICO,
                Checked = true,//this.STATE == 1 ? true : false,
                deptnote = menu.NOTE,
                children = new List<EasyUIDEPTTree>()
            };
            return easyUITreeNode;
        }
        #endregion


        #region 把组织机构转为符合EASYUI的带有递归关系的集合
        public List<EasyUIDEPTTree> ConvertTreeNodes(List<SYS_DEPT> listMenus, string pid, string id)
        {
            List<EasyUIDEPTTree> listTreeNodes = new List<EasyUIDEPTTree>();
            LoadTreeNode(listMenus, listTreeNodes, pid);
            DelClosed(listTreeNodes, id);
            return listTreeNodes;
        }
        /// <summary>
        /// 根据是否有下级来更改打开关闭状态,这是解决同步加载的问题，如果是异步加载？
        /// </summary>
        /// <param name="listTreeNodes"></param>
        private void DelClosed(List<EasyUIDEPTTree> listTreeNodes, string id)
        {
            foreach (EasyUIDEPTTree uinode in listTreeNodes)
            {
                //只有存在下级才可设为closed，否则会循环查询
                if (uinode.children.Count == 0 && uinode.state == "closed")
                {
                    uinode.state = "open";
                }
                else
                {
                    if (uinode.DEPT_CODE == id)//默认将根结点设为打开
                    {
                        uinode.state = "open";
                    }
                    DelClosed(uinode.children, id);
                }
            }
        }
        private void LoadTreeNode(List<SYS_DEPT> listMenus, List<EasyUIDEPTTree> listTreeNodes, string pid)
        {
            foreach (SYS_DEPT menu in listMenus)
            {
                if (menu.PARENT_CODE == pid)
                {
                    EasyUIDEPTTree node = TransformTreeNode(menu);
                    listTreeNodes.Add(node);
                    LoadTreeNode(listMenus, node.children, node.DEPT_CODE);
                }

            }
        }
        #endregion



        #region 新增部门
        /// <summary>
        /// 新增部门
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public AjaxMsgModel Add(SYS_DEPT orgInfo)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            //try
            //{
                if (bs.Entities<SYS_DEPT>().Where(o => o.DEPT_CODE == orgInfo.PARENT_CODE).Count() == 0)
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.NotFound, "隶属部门");
                    return amm;
                }
                if (bs.Entities<SYS_DEPT>().Where(m => m.DEPT_NAME == orgInfo.DEPT_NAME && m.PARENT_CODE == orgInfo.PARENT_CODE).ToList().Count > 0)
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.YesFound, "部门名称");
                    return amm;
                }

                if (bs.AddEntity(orgInfo) > 0)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = string.Format(Message.OptSussess, "部门", Message.AddOpt);
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.OptFail, "部门", Message.AddOpt);
                }
            //}
            //catch (Exception)
            //{
            //    return amm;
            //}
            return amm;
        }
        #endregion


        #region 修改部门
        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="orgInfo">用户</param>
        /// <returns>AjaxMsgModel实体对象</returns>
        public AjaxMsgModel Edit(SYS_DEPT orgInfo, string upid, string newDeptCode)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            //try
            //{
                if (bs.Entities<SYS_DEPT>().Where(o => o.DEPT_CODE == orgInfo.PARENT_CODE).Count() == 0)
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.NotFound, "隶属部门");
                    return amm;
                }
                SYS_DEPT org = bs.Entities<SYS_DEPT>().Where(m => m.DEPT_CODE == upid).OrderBy(m => m.DEPT_CODE).FirstOrDefault();
                if (org.PARENT_CODE == orgInfo.PARENT_CODE)//如果在同一级下则修改
                {
                    orgInfo.DEPT_CODE = org.DEPT_CODE;

                    if (bs.Entities<SYS_DEPT>().Where(u => u.DEPT_CODE != orgInfo.DEPT_CODE && u.DEPT_NAME == orgInfo.DEPT_NAME && u.PARENT_CODE == orgInfo.PARENT_CODE).ToList().Count > 0)
                    {
                        amm.Statu = AjaxStatu.err;
                        amm.Msg = string.Format(Message.YesFound, "部门名称");
                        return amm;
                    }
                    // Expression<Func<SYS_DEPT, object>>[] ignoreProperties =
                    //     new Expression<Func<SYS_DEPT, object>>[] {
                    //p=>p.SYS_MEMBER,p=>p.SYS_USER,p=>p.JOB_PLAN_APPROVAL,p=>p.GATE_BDATA_CARD,p=>p.GATE_BDATA_CLIENT,
                    //     p=>p.GATE_BDATA_GATE,p=>p.GATE_JOB_PLAN, p=>p.TOOL_BDATE_STOREHOURSE,p=>p.TOOL_JOB_PLAN,
                    //     p=>p.TOOL_JOB_PLAN_IMPORT,p=>p.TOOL_TOOLMNG_INOUT,p=>p.RULE_PROBLEM,
                    //     p=>p.RULE_PROBLEM_BLAME};

                    if (bs.UpdateEntity(orgInfo, new string[] { "DEPT_NAME", "PARENT_CODE", "C_ICO",
                    "STATUS_FLAG","DEL_FLAG","NOTE","DEPT_FLAG"}) > 0)
                    {
                        amm.Statu = AjaxStatu.ok;
                        amm.Msg = string.Format(Message.OptSussess, "部门", Message.EditOpt);
                    }
                    else
                    {
                        amm.Statu = AjaxStatu.err;
                        amm.Msg = string.Format(Message.OptFail, "部门", Message.EditOpt);
                    }
                }
                else//如果更换级别则先删后加
                {
                    if (bs.Entities<SYS_DEPT>().Where(m => m.PARENT_CODE == upid).Count() > 0)
                    {
                        amm.Statu = AjaxStatu.err;
                        amm.Msg = "该部门下已经有下属部门，暂不能更换隶属部门！";

                        return amm;
                    }
                    if (bs.Entities<SYS_MEMBER>().Where(m => m.DEL_FLAG == "0" && m.DEPT_CODE == upid).Count() > 0)
                    {
                        amm.Statu = AjaxStatu.err;
                        amm.Msg = "该部门下已经有人员，暂不能更换隶属部门！";

                        return amm;
                    }
                    if (bs.Entities<SYS_USER>().Where(m => m.DEPT_CODE == upid).Count() > 0)
                    {
                        amm.Statu = AjaxStatu.err;
                        amm.Msg = "该部门下已经有用户，暂不能更换隶属部门！";

                        return amm;
                    }
                    if (bs.Entities<SYS_DEPT>().Where(m => m.DEPT_NAME == orgInfo.DEPT_NAME && m.PARENT_CODE == orgInfo.PARENT_CODE).ToList().Count > 0)
                    {
                        amm.Statu = AjaxStatu.err;
                        amm.Msg = string.Format(Message.YesFound, "部门名称");
                        return amm;
                    }
                    int a = bs.DelByWhere<SYS_DEPT>(m => m.DEPT_CODE == upid);//删除原来的
                    if (a > 0)
                    {
                        orgInfo.DEPT_CODE = newDeptCode;//更换新的编码
                        if (bs.AddEntity(orgInfo) > 0)
                        {
                            amm.Statu = AjaxStatu.ok;
                            amm.Msg = string.Format(Message.OptSussess, "部门", Message.EditOpt);
                        }
                        else
                        {
                            amm.Statu = AjaxStatu.err;
                            amm.Msg = string.Format(Message.OptFail, "部门", Message.EditOpt);
                        }
                    }
                    else
                    {
                        amm.Statu = AjaxStatu.err;
                        amm.Msg = string.Format(Message.OptFail, "部门", Message.EditOpt);
                        return amm;
                    }
                }
            //}
            //catch (Exception)
            //{
            //    return amm;
            //}
            return amm;
        }
        #endregion


        #region 逻辑删除部门
        /// <summary>
        /// 逻辑删除部门
        /// </summary>
        /// <param name="DEPTCODE">逻辑删除部门ID</param>
        /// <returns>AjaxMsgModel实体对象</returns>
        public AjaxMsgModel Del(string DEPTCODE)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            //try
            //{
                if (bs.Entities<SYS_DEPT>().Where(o => o.DEPT_CODE == DEPTCODE).Count() == 0)
                {
                    amm.Msg = string.Format(Message.NotFound, "部门");
                    return amm;
                }
                if (bs.Entities<SYS_DEPT>().Where(m => m.DEL_FLAG == "0" && m.PARENT_CODE == DEPTCODE).Count() > 0)
                {
                    amm.Msg = "请先删除此部门的下属部门！";

                    return amm;
                }

                if (bs.Entities<SYS_MEMBER>().Where(m => m.DEL_FLAG == "0" && m.DEPT_CODE == DEPTCODE).Count() > 0)
                {
                    amm.Msg = "请先删除此部门的下属人员！";

                    return amm;
                }
                if (bs.Entities<SYS_USER>().Where(m => m.DEPT_CODE == DEPTCODE).Count() > 0)
                {
                    amm.Msg = "请先删除此部门的下属用户！";

                    return amm;
                }
                //以下三行是逻辑删除使用
                SYS_DEPT user = bs.Entities<SYS_DEPT>().Where(m => m.DEPT_CODE == DEPTCODE).OrderBy(m => m.DEPT_CODE).FirstOrDefault();
                user.DEL_FLAG = "1";
                if (bs.UpdateEntity(user, new string[] { "DEL_FLAG" }) > 0)
                //int a = oc.BllSession.ISYS_DEPTService.DelByWhere(m => m.DEPT_CODE == DEPTCODE);
                //if (a > 0)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = string.Format(Message.OptSussess, "部门", Message.DelOpt);
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.OptFail, "部门", Message.DelOpt);
                }
            //}
            //catch (Exception)
            //{
            //    return amm;
            //}
            return amm;
        }
        #endregion

        #region 根据部门编号获取部门：段-车间-工区信息

        /// <summary>
        /// 根据部门编号获取部门：段-车间-工区信息
        /// </summary>
        /// <param name="dept_code">部门编码</param>
        /// <param name="deptList">部门列表</param>
        /// <returns>段-车间-工区</returns>
        public string GetDeptInfo(string dept_code, List<SYS_DEPT> deptList)
        {
            string deptInfo = ",,";
            if (dept_code.Length <= 4)
            {
                deptInfo = deptList.Where(o => o.DEPT_CODE == dept_code).FirstOrDefault().DEPT_NAME + ",,";
            }
            else if (dept_code.Length == 6)
            {
                deptInfo = deptList.Where(o => o.DEPT_CODE == dept_code.Substring(0, 4)).FirstOrDefault().DEPT_NAME +
                           ",,";
            }
            else if (dept_code.Length == 8)
            {
                deptInfo = deptList.Where(o => o.DEPT_CODE == dept_code.Substring(0, 4)).FirstOrDefault().DEPT_NAME +
                           "," +
                           deptList.Where(o => o.DEPT_CODE == dept_code.Substring(0, 8)).FirstOrDefault().DEPT_NAME
                           + ",";
            }
            else if (dept_code.Length == 10)
            {
                deptInfo = deptList.Where(o => o.DEPT_CODE == dept_code.Substring(0, 4)).FirstOrDefault().DEPT_NAME +
                           "," +
                           deptList.Where(o => o.DEPT_CODE == dept_code.Substring(0, 8)).FirstOrDefault().DEPT_NAME
                           + "," + deptList.Where(o => o.DEPT_CODE == dept_code).FirstOrDefault().DEPT_NAME;
            }
            return deptInfo;
        }

        #endregion

        
        #region 查看我的组织机构包含路局除工区树
        /// <summary>
        /// 获取组织机构树
        /// </summary>
        /// <returns>PARENT_CODE所在部门的父ID</returns>
        [Skip]
        [AjaxRequest]
        public List<EasyUITreeNode> GetMyORGAllNoGQTree(string DEPT_CODE, string PARENT_CODE, int DEPT_FLAG = 0)
        {
            string strJson = string.Empty;
            //获得组织
            List<SYS_DEPTANDMEMBER> listOrgMenu = bs.Entities<SYS_DEPT>().Where(m => m.DEL_FLAG == "0" && m.C_ICO == "icon-DepartMent" && m.DEPT_FLAG == DEPT_FLAG && !m.PARENT_CODE.EndsWith("00") && (m.PARENT_CODE.StartsWith(DEPT_CODE) || m.DEPT_CODE == DEPT_CODE || m.DEPT_CODE == DEPT_CODE.Substring(0, 2) || m.DEPT_CODE == (DEPT_CODE.Length >= 6 ? DEPT_CODE.Substring(0, 4) : DEPT_CODE)))
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
            List<EasyUITreeNode> listTreeNodes = SYS_DEPTANDMEMBER.ConvertTreeNodes(listOrgMenu, "0", DEPT_CODE, false);
            //strJson = ObjToJson.GetToJson(listTreeNodes).Replace("Checked", "checked");
            return listTreeNodes;
        }
        #endregion

        #region 查看我的组织机构只包含段
        /// <summary>
        /// 获取组织机构树
        /// </summary>
        /// <returns>PARENT_CODE所在部门的父ID</returns>
        [Skip]
        [AjaxRequest]
        public List<EasyUITreeNode> GetMyOnlyTree(string DEPT_CODE, string PARENT_CODE, int DEPT_FLAG = 0)
        {
            string strJson = string.Empty;
            //获得组织
            IQueryable<SYS_DEPT> list = null;
            if (DEPT_CODE.Length == 2)
                list = bs.Entities<SYS_DEPT>().Where(m => m.DEL_FLAG == "0" && m.C_ICO == "icon-DepartMent" && m.DEPT_CODE.Length <= 4 && (m.DEPT_CODE == DEPT_CODE || m.DEPT_CODE.StartsWith(DEPT_CODE)))
                   .OrderBy(m => m.DEPT_ORDER).ThenBy(m => m.DEPT_CODE);
            else
                list = bs.Entities<SYS_DEPT>().Where(m => m.DEL_FLAG == "0" && m.C_ICO == "icon-DepartMent" && m.DEPT_CODE.Length <= 4 && (m.DEPT_CODE == DEPT_CODE || m.PARENT_CODE == "0"))
                .OrderBy(m => m.DEPT_ORDER).ThenBy(m => m.DEPT_CODE);
            List<SYS_DEPTANDMEMBER> listOrgMenu = list
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
            List<EasyUITreeNode> listTreeNodes = SYS_DEPTANDMEMBER.ConvertTreeNodes(listOrgMenu, "0", DEPT_CODE, false);
            //strJson = ObjToJson.GetToJson(listTreeNodes).Replace("Checked", "checked");
            return listTreeNodes;
        }
        #endregion
    }
}