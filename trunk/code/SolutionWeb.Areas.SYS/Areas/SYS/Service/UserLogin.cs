using SolutionWeb.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SolutionWeb.Common;
using SolutionWeb.Model.SYS;
using SolutionWeb.Models;

namespace SolutionWeb.Areas.SYS.Service
{
    public class UserLogin : IUserLogin
    {
        #region 操作上下文的静态变量
        static OperContext oc = OperContext.CurrentContext;
        #endregion
        #region Ioc
        private IBaseService bs = null;
        public UserLogin(IBaseService baseService)
        {
            this.bs = baseService;
        }

        #endregion
        #region 修改密码
        public AjaxMsgModel EditPass(string username, string oldpass, string newpass)
        {
            SYS_USER user = new SYS_USER
            {
                USER_NAME = username,
                PASSWORD = newpass
            };
            AjaxMsgModel amm = Model_SYS_USER.Create.EditUserPwd(user, oldpass);
            return amm;
        }
        #endregion
        #region 判断菜单权限
        public bool HasPermission(string areaName, string controllerName, string actionName, HttpMethod httpmethod)
        {
            return Model_SYS_MENU.Create.HasPermission(areaName, controllerName, actionName, httpmethod);
        } 
        #endregion

        #region 判断是否登录
        public bool IsLogin()
        {
            return Model_SYS_MENU.Create.IsLogin();
        }

        #endregion

        #region 部门人员列表
        public List<EasyUITreeNode> GetMyDEPTandRYTree(string DEPT_CODE, string PARENT_CODE)
        {
            return Model_SYS_MEMBER.Create.GetMyDEPTandRYTree(DEPT_CODE, PARENT_CODE);
        }
        #endregion
        public string GetMyORGandRYTree_Two(string DEPT_CODE, string PARENT_CODE, bool isCheckAll)
        {
            return Model_SYS_MEMBER.Create.GetMyORGandRYTree_Two(DEPT_CODE, PARENT_CODE, isCheckAll);
        }

        #region 得到我的组织机构部门人员列表集合
        public List<EasyUITreeNode> GetMyDEPTTree(string DEPT_CODE, string PARENT_CODE)
        {
            return Model_SYS_DEPT.Create.GetMyDEPTTree(DEPT_CODE, PARENT_CODE);
        }
        #endregion

        #region 查看我的组织机构只包含段
        public List<EasyUITreeNode> GetMyOnlyTree(string DEPT_CODE, string PARENT_CODE, int DEPT_FLAG = 0)
        {
            return Model_SYS_DEPT.Create.GetMyOnlyTree(DEPT_CODE, PARENT_CODE, DEPT_FLAG);
        }
        #endregion
        #region 查看我的组织机构
        public List<EasyUITreeNode> GetMyORGTree(string DEPT_CODE, string PARENT_CODE, int DEPT_FLAG = 0)
        {
            return Model_SYS_DEPT.Create.GetMyORGTree(DEPT_CODE, PARENT_CODE, DEPT_FLAG);
        }
        #endregion


        #region 查看我的组织机构除工区树
        public List<EasyUITreeNode> GetMyORGNoGQTree(string DEPT_CODE, string PARENT_CODE, int DEPT_FLAG = 0)
        {
            return Model_SYS_DEPT.Create.GetMyORGNoGQTree(DEPT_CODE, PARENT_CODE, DEPT_FLAG);
        }
        #endregion

        #region 获取地图左侧菜单
        public string GetMyGISMenu()
        {
            return Model_SYS_MENU.Create.GetMyGISMenu();
        }
        #endregion
        #region 获取用户对应部门组织树
        public string GetDeptMenu()
        {
            return Model_SYS_MEMBER.Create.GetMyORDeptTree(oc.CurrentUser.DEPT_CODE, oc.CurrentUser.PARENT_CODE, true);
        }
        #endregion
        public string GetOrganizeMenu(bool isCheckAll)
        {
            return Model_SYS_MEMBER.Create.GetMyORGandRYTree(oc.CurrentUser.DEPT_CODE, oc.CurrentUser.PARENT_CODE, isCheckAll);
        }
        #region 判断是否登录
        public string IsWarn()
        {
            string warntab = "";//2017-02-15增加
            if (HasPermission("WARN", "JOB", "WARNJOB", Common.HttpMethod.Post))
            {
                warntab += "|WARNJOB|";//施工计划报警
            }
            if (HasPermission("WARN", "RAIN", "WARNRAIN", Common.HttpMethod.Post))
            {
                warntab += "|WARNRAIN|";//出巡报警
            }
            if (HasPermission("WARN", "CONFIR", "WARNCONFIR", Common.HttpMethod.Post))
            {
                warntab += "|WARNCONFIR|";//确认车报警
            }
            if (HasPermission("WARN", "RAIL", "WARNRAIL", Common.HttpMethod.Post))
            {
                warntab += "|WARNRAIL|";//断轨报警
            }
            if (HasPermission("WARN", "CAR", "WARNCAR", Common.HttpMethod.Post))
            {
                warntab += "|WARNCAR|";//汽车超速
            }
            if (HasPermission("WARN", "GATE", "WARNGATE", Common.HttpMethod.Post))
            {
                warntab += "|WARNGATE|";//门禁报警
            }
            if (HasPermission("WARN", "PROTECTEDNET", "WARNPROTECTEDNET", Common.HttpMethod.Post))
            {
                warntab += "|WARNPROTECTEDNET|";//护网监控报警
            }
            if (HasPermission("WARN", "WATER", "WARNWATER", Common.HttpMethod.Post))
            {
                warntab += "|WARNWATER|";//水位报警
            }
            return warntab;
        }
        #endregion

        public int TestAdd()
        {
            SYS_USER u = new SYS_USER()
            {
                USER_NAME = "wwxy",
                PASSWORD = "123"
            };
            return bs.AddEntity<SYS_USER>(u);
        }


        #region 登录验证
        public AjaxMsgModel LoginIn(string strLoginName, string strLoginPwd, string strYzm)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            if (strYzm.ToUpper() != "ZZZZZ")
            {
                if (!strYzm.Equals(oc.CurrentUserVcode))
                {
                    amm.Msg = string.Format(Message.InvalidReEnter, "验证码");
                    return amm;
                }
            }
                #region 普通用户
                //根据登录名得到用户信息
                var users = bs.Entities<SYS_USER>().Where(u => u.USER_NAME == strLoginName).Select(u => new
                {
                    DEPT_CODE = u.DEPT_CODE,
                    USER_NAME = u.USER_NAME,
                    PASSWORD = u.PASSWORD,
                    DEPT_NAME = u.SYS_DEPT.DEPT_NAME,//部门名称
                    PARENT_CODE = u.SYS_DEPT.PARENT_CODE,//部门父ID
                    ZSNAME = u.ZSNAME
                }).ToList();

                if (users.Count > 0)
                {
                    var cUsr = users.First();
                    if (cUsr != null && cUsr.PASSWORD == DataHelper.TOMD5(strLoginPwd))
                    {
                        //如果用户名称密码都正确那么就把用户信息放入Session中
                        oc.CurrentUser = new SESS_USER
                        {
                            USER_NAME = cUsr.USER_NAME,
                            ZSNAME = cUsr.ZSNAME,
                            DEPT_NAME = cUsr.DEPT_NAME,
                            DEPT_CODE = cUsr.DEPT_CODE,
                            PARENT_CODE = cUsr.PARENT_CODE
                        };
                        /**
                         * 保存当前用户的菜单权限信息
                         */
                        oc.UserMenuPermission = Model_SYS_MENU.Create.GetUserPermission(cUsr.USER_NAME);

                        //创建Cookie保存登录用户信息
                        oc.CurrentUserName = cUsr.USER_NAME;

                        //返回登录成功的信息，并跳转到管理端首页
                        amm.Statu = AjaxStatu.ok;
                        //amm.Msg = "登录成功";
                        amm.Msg = string.Format(Message.OptSussess, string.Empty, "登录");
                        amm.BackUrl = "/ADMIN/Admin/Index";
                        return amm;
                    }
                    else
                    {
                        //返回登录失败的信息
                        amm.Statu = AjaxStatu.err;
                        amm.Msg = string.Format(Message.InvalidReEnter, "密码");
                        return amm;
                    }
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.NotFound, "用户名");
                    return amm;
                }
                #endregion 普通用户
        }

        #endregion
    }
}