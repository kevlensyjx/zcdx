using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SolutionWeb.Interface;
using SolutionWeb.Common;
using SolutionWeb.Model.POLICY;

namespace SolutionWeb.Areas.COMPANY.Service
{
    public class CompanyLogin : ICompanyLogin
    {
        #region 操作上下文的静态变量
        static OperContext oc = OperContext.CurrentContext;
        #endregion
        #region Ioc
        private IBaseService bs = null;
        public CompanyLogin(IBaseService baseService)
        {
            this.bs = baseService;
        }

        #endregion


        #region 登录验证
        public AjaxMsgModel CompanyLoginIn(string strLoginName, string strLoginPwd, string strYzm)
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
            var users = bs.Entities<CORPORATION_BASE_INFO>().Where(u => u.USER_NAME == strLoginName).Select(u => new
            {
                DEPT_CODE = u.SID,
                USER_NAME = u.USER_NAME,
                PASSWORD = u.PASSWORD,
                DEPT_NAME = u.CORP_NAME,//企业名称
                PARENT_CODE = u.LEGAL_PERSON_PHONE,//法人电话
                ZSNAME = u.LEGAL_PERSON,//法人姓名
                APPLY_RESULT = u.APPLY_RESULT
            }).ToList();


            if (users.Count > 0)
            {
                var cUsr = users.First();
                if (cUsr != null && cUsr.PASSWORD == DataHelper.TOMD5(strLoginPwd))
                {
                    if (!"通过".Equals(cUsr.APPLY_RESULT))
                    {
                        amm.Msg = "帐户还未通过审核!";
                        return amm;
                    }
                    //如果用户名称密码都正确那么就把用户信息放入Session中
                    oc.CompanyUser = new SESS_USER
                    {
                        USER_NAME = cUsr.USER_NAME,
                        ZSNAME = cUsr.ZSNAME,
                        DEPT_NAME = cUsr.DEPT_NAME,
                        DEPT_CODE = cUsr.DEPT_CODE,
                        PARENT_CODE = cUsr.PARENT_CODE
                    };

                    //返回登录成功的信息，并跳转到管理端首页
                    amm.Statu = AjaxStatu.ok;
                    //amm.Msg = "登录成功";
                    amm.Msg = string.Format(Message.OptSussess, string.Empty, "登录");
                    amm.BackUrl = "/Users/Index";
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