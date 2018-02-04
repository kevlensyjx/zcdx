using SolutionWeb.Common;
using SolutionWeb.Model.SYS;
using SolutionWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace SolutionWeb.Models
{
    public partial class Model_SYS_MEMBER
    {
        #region 获取用户对应部门人员列表组织树
        /// <summary>
        /// 获取用户对应部门组织树
        /// </summary>
        /// <returns></returns>
        public List<EasyUITreeNode> GetMyDEPTandRYTree(string DEPT_CODE, string PARENT_CODE)
        {
            string strJson = string.Empty;
            List<SYS_DEPTANDMEMBER> listOrgRy = new List<SYS_DEPTANDMEMBER>();
            //获取人员
            List<SYS_MEMBER> listRyMenu = bs.GetListByCondition<SYS_MEMBER,string>(m => m.DEL_FLAG == "0" && m.DEPT_CODE.StartsWith(DEPT_CODE), m => m.PHONE).ToList();
            if (listRyMenu.Count > 0)
            {
                foreach (SYS_MEMBER ry in listRyMenu)
                {
                    listOrgRy.Add(new SYS_DEPTANDMEMBER()
                    {
                        ORGRY_CODE = ry.PHONE,
                        PARENT_CODE = ry.DEPT_CODE,
                        ORGRY_NAME = ry.NAME,
                        ICO = "icon-On-line",
                        RYMOBILE = ry.PHONE,
                        STATE = "0"//关闭
                    });
                }
            }

            //获得组织
            //List<SYS_ORG> listMenu = oc.BllSession.ISYS_ORGService.GetListByCondition<string>(m => m.DEL_FLAG == "0", m => m.DEPT_CODE).Select(mb => mb.ToPOCO()).ToList();
            List<SYS_DEPT> listMenu = bs.Entities<SYS_DEPT>().Where(m => m.DEL_FLAG == "0" && (m.PARENT_CODE.StartsWith(DEPT_CODE) || m.DEPT_CODE == DEPT_CODE))
                .OrderBy(m => m.DEPT_ORDER).ThenBy(m => m.DEPT_CODE).ToList();
            if (listMenu.Count > 0)
            {
                foreach (SYS_DEPT org in listMenu)
                {
                    listOrgRy.Add(new SYS_DEPTANDMEMBER()
                    {
                        ORGRY_CODE = org.DEPT_CODE,
                        PARENT_CODE = org.PARENT_CODE,
                        ORGRY_NAME = org.DEPT_NAME,
                        ICO = org.C_ICO,
                        RYMOBILE = "",
                        STATE = org.STATUS_FLAG//0关闭
                    });
                }
            }
            List<EasyUITreeNode> listTreeNodes = SYS_DEPTANDMEMBER.ConvertTreeNodes(listOrgRy, PARENT_CODE, DEPT_CODE, false);
            //return  ObjToJson.GetToJson(listTreeNodes, true);
            //strJson = ObjToJson.GetToJson(listTreeNodes).Replace("Checked", "checked");
            return listTreeNodes;
        }
        #endregion

        #region 获取用户对应部门组织工务段树
        /// <summary>
        /// 获取用户对应部门组织树
        /// </summary>
        /// <returns></returns>
        public string GetMyORDeptTree(string DEPT_CODE, string PARENT_CODE, bool isCheckAll)
        {
            string strJson = string.Empty;
            List<SYS_DEPTANDMEMBER_TEMP> listOrgRy = new List<SYS_DEPTANDMEMBER_TEMP>();
            List<SYS_DEPT> listMenu = bs.Entities<SYS_DEPT>().Where(m => m.DEL_FLAG == "0" && m.DEPT_CODE.Length <= 8 && (m.PARENT_CODE.StartsWith(DEPT_CODE) || m.DEPT_CODE == DEPT_CODE))
    .OrderBy(m => m.DEPT_ORDER).ThenBy(m => m.DEPT_CODE).ToList();
            if (listMenu.Count > 0)
            {
                foreach (SYS_DEPT org in listMenu)
                {
                    listOrgRy.Add(new SYS_DEPTANDMEMBER_TEMP()
                    {
                        ORGRY_CODE = org.DEPT_CODE,
                        PARENT_CODE = org.PARENT_CODE,
                        ORGRY_NAME = org.DEPT_NAME,
                        ICO = org.C_ICO,
                        RYMOBILE = "",
                        STATE = org.STATUS_FLAG//0关闭
                    });
                }
            }
            List<ZTreeNode> listTreeNodes = SYS_DEPTANDMEMBER_TEMP.ConvertZTreeNodes(listOrgRy, PARENT_CODE, DEPT_CODE, isCheckAll);
            strJson = ObjToJson.GetToJson(listTreeNodes).Replace("Checked", "checked");
            return strJson;
        }
        #endregion


        #region 获取用户对应部门组织树
        /// <summary>
        /// 获取用户对应部门组织树
        /// </summary>
        /// <returns></returns>
        public string GetMyORGandRYTree(string DEPT_CODE, string PARENT_CODE, bool isCheckAll)
        {
            string strJson = string.Empty;
            List<SYS_DEPTANDMEMBER_TEMP> listOrgRy = new List<SYS_DEPTANDMEMBER_TEMP>();
            //获取人员
            List<SYS_MEMBER> listRyMenu = bs.GetListByCondition<SYS_MEMBER,string>(m => m.DEL_FLAG == "0" && m.DEPT_CODE.StartsWith(DEPT_CODE), m => m.PHONE).ToList();
            if (listRyMenu.Count > 0)
            {
                foreach (SYS_MEMBER ry in listRyMenu)
                {
                    string icon = "icon-Off-line";
                    listOrgRy.Add(new SYS_DEPTANDMEMBER_TEMP()
                    {
                        ORGRY_CODE = ry.PHONE,
                        PARENT_CODE = ry.DEPT_CODE,
                        ORGRY_NAME = ry.NAME,
                        ICO = icon,
                        RYMOBILE = ry.PHONE,
                        STATE = "0"//关闭
                    });
                }
            }
            //获得组织
            //List<SYS_ORG> listMenu = oc.BllSession.ISYS_ORGService.GetListByCondition<string>(m => m.DEL_FLAG == "0", m => m.DEPT_CODE).Select(mb => mb.ToPOCO()).ToList();
            List<SYS_DEPT> listMenu = bs.Entities<SYS_DEPT>().Where(m => m.DEL_FLAG == "0" && (m.PARENT_CODE.StartsWith(DEPT_CODE) || m.DEPT_CODE == DEPT_CODE))
                .OrderBy(m => m.DEPT_ORDER).ThenBy(m => m.DEPT_CODE).ToList();
            if (listMenu.Count > 0)
            {
                foreach (SYS_DEPT org in listMenu)
                {
                    listOrgRy.Add(new SYS_DEPTANDMEMBER_TEMP()
                    {
                        ORGRY_CODE = org.DEPT_CODE,
                        PARENT_CODE = org.PARENT_CODE,
                        ORGRY_NAME = org.DEPT_NAME,
                        ICO = org.C_ICO,
                        RYMOBILE = "",
                        STATE = org.STATUS_FLAG//0关闭
                    });
                }
            }
            //List<EasyUITreeNode> listTreeNodes = SYS_DEPTANDMEMBER.ConvertTreeNodes(listOrgRy, PARENT_CODE, DEPT_CODE, isCheckAll);
            List<ZTreeNode> listTreeNodes = SYS_DEPTANDMEMBER_TEMP.ConvertZTreeNodes(listOrgRy, PARENT_CODE, DEPT_CODE, isCheckAll);
            strJson = ObjToJson.GetToJson(listTreeNodes).Replace("Checked", "checked");

            return strJson;
        }
        public  string GetMyORGandRYTree_Two(string DEPT_CODE, string PARENT_CODE, bool isCheckAll)
        {
            string strJson = string.Empty;
            List<SYS_DEPTANDMEMBER> listOrgRy = new List<SYS_DEPTANDMEMBER>();
            //获取人员
            List<SYS_MEMBER> listRyMenu = bs.GetListByCondition<SYS_MEMBER, string>(m => m.DEL_FLAG == "0"  && m.DEPT_CODE.StartsWith(DEPT_CODE), m => m.PHONE).ToList();
            if (listRyMenu.Count > 0)
            {
                foreach (SYS_MEMBER ry in listRyMenu)
                {
                    listOrgRy.Add(new SYS_DEPTANDMEMBER()
                    {
                        ORGRY_CODE = ry.PHONE,
                        PARENT_CODE = ry.DEPT_CODE,
                        ORGRY_NAME = ry.NAME,
                        ICO = "icon-Off-line",
                        RYMOBILE = ry.PHONE,
                        STATE = "0"//关闭
                    });
                }
            }
            //获得组织
            //List<SYS_ORG> listMenu = oc.BllSession.ISYS_ORGService.GetListByCondition<string>(m => m.DEL_FLAG == "0", m => m.DEPT_CODE).Select(mb => mb.ToPOCO()).ToList();
            List<SYS_DEPT> listMenu = bs.Entities<SYS_DEPT>().Where(m => m.DEL_FLAG == "0" && (m.PARENT_CODE.StartsWith(DEPT_CODE) || m.DEPT_CODE == DEPT_CODE))
                .OrderBy(m => m.DEPT_ORDER).ThenBy(m => m.DEPT_CODE).ToList();
            if (listMenu.Count > 0)
            {
                foreach (SYS_DEPT org in listMenu)
                {
                    listOrgRy.Add(new SYS_DEPTANDMEMBER()
                    {
                        ORGRY_CODE = org.DEPT_CODE,
                        PARENT_CODE = org.PARENT_CODE,
                        ORGRY_NAME = org.DEPT_NAME,
                        ICO = org.C_ICO,
                        RYMOBILE = "",
                        STATE = org.STATUS_FLAG//0关闭
                    });
                }
            }
            //List<EasyUITreeNode> listTreeNodes = SYS_DEPTANDMEMBER.ConvertTreeNodes(listOrgRy, PARENT_CODE, DEPT_CODE, isCheckAll);
            List<EasyUITreeNode> listTreeNodes = SYS_DEPTANDMEMBER.ConvertTreeNodes(listOrgRy, PARENT_CODE, DEPT_CODE, isCheckAll);
            strJson = ObjToJson.GetToJson(listTreeNodes).Replace("Checked", "checked");

            return strJson;
        }


        #endregion
        #region 新增人员
        /// <summary>
        /// 新增人员
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public AjaxMsgModel Add(SYS_MEMBER UserInfo)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            //try
            //{
                if (bs.Entities<SYS_DEPT>().Where(o => o.DEPT_CODE == UserInfo.DEPT_CODE).Count() == 0)
                {
                    amm.Msg = string.Format(Message.NotFound, "部门");
                    return amm;
                }
                if (bs.Entities<SYS_MEMBER>().Where(m => m.DEL_FLAG == "0" && m.PHONE == UserInfo.PHONE).Count() > 0)
                {
                    //amm.Msg = "手机号码已经存在！";
                    amm.Msg = string.Format(Message.YesFound, "手机号码");
                    return amm;
                }

                if (bs.AddEntity(UserInfo) > 0)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = string.Format(Message.OptSussess, "人员", Message.AddOpt);
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.OptFail, "人员", Message.AddOpt);
                }
            //}
            //catch (Exception)
            //{
            //    return amm;
            //}
            return amm;
        }
        #endregion

        #region 修改人员
        /// <summary>
        /// 修改人员
        /// </summary>
        /// <param name="UserInfo">用户</param>
        /// <returns>AjaxMsgModel实体对象</returns>
        public  AjaxMsgModel Edit(SYS_MEMBER UserInfo)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            //try
            //{
                if (bs.Entities<SYS_DEPT>().Where(o => o.DEPT_CODE == UserInfo.DEPT_CODE).Count() == 0)
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.NotFound, "部门");
                    return amm;
                }
                if (bs.Entities<SYS_MEMBER>().Where(u => u.DEL_FLAG == "0" && u.MEMBER_ID != UserInfo.MEMBER_ID && u.PHONE == UserInfo.PHONE).Count() > 0)
                {
                    amm.Msg = string.Format(Message.YesFound, "手机号码");
                    return amm;
                }
               Expression<Func<SYS_MEMBER, object>>[] ignoreProperties = new Expression<Func<SYS_MEMBER, object>>[] {
                                                      p=>p.SYS_DEPT};

            //if (bs.UpdateEntity(UserInfo, new string[] { "NAME", "MOBILE", "JOB",
            //    "PHONE","NOTE","UPDATE_DATE","DEPT_CODE","UPDATE_USER","DEL_FLAG","LOCATION_FLAG","INTELLIGENCE","MOBILE_STATE"}) > 0)//,"POS_LEVEL"
                if (bs.UpdateEntity(UserInfo, ignoreProperties) > 0)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = string.Format(Message.OptSussess, "人员", Message.EditOpt);
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.OptFail, "人员", Message.EditOpt);
                }
            //}
            //catch (Exception)
            //{
            //    return amm;
            //}
            return amm;
        }
        #endregion

        #region 逻辑删除人员
        /// <summary>
        /// 逻辑删除人员
        /// </summary>
        /// <param name="MEMBERID">删除人员ID</param>
        /// <returns>AjaxMsgModel实体对象</returns>
        public  AjaxMsgModel Del(string MEMBERID)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            //try
            //{
                SYS_MEMBER member = bs.Entities<SYS_MEMBER>().Where(m => m.DEL_FLAG == "0" && m.MEMBER_ID == MEMBERID).OrderBy(m => m.UPDATE_DATE).FirstOrDefault();

                if (member != null)
                {
                    member.DEL_FLAG = "1";
                    int iret = bs.UpdateEntity(member, new string[] { "DEL_FLAG" });
                    if (iret > 0)//oc.BllSession.ISYS_MEMBERService.DelByWhere(m => m.MEMBER_ID == MEMBERID) > 0
                    {
                        amm.Statu = AjaxStatu.ok;
                        amm.Msg = string.Format(Message.OptSussess, "人员", Message.DelOpt);
                    }
                    else
                    {
                        amm.Statu = AjaxStatu.err;
                        amm.Msg = string.Format(Message.OptFail, "人员", Message.DelOpt);
                    }
                }
                else
                {
                    amm.Msg = string.Format(Message.NotFound, "人员");
                }
                return amm;
            //}
            //catch (Exception)
            //{
            //    return amm;
            //}
        }
        #endregion
        

        
    }
}