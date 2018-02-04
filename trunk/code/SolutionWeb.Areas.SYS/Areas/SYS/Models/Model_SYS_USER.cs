using SolutionWeb.Common;
using SolutionWeb.Model.SYS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using System.Web;

namespace SolutionWeb.Models
{
    public partial class Model_SYS_USER
    {


        #region 修改用户密码
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="UserInfo">用户</param>
        /// <returns>AjaxMsgModel实体对象</returns>
        public AjaxMsgModel EditUserPwd(SYS_USER UserInfo, string oldpass)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {
                SYS_USER modifyUser = bs.Entities<SYS_USER>().Where(u => u.USER_NAME == UserInfo.USER_NAME).FirstOrDefault();

                if (!DataHelper.TOMD5(oldpass).Equals(modifyUser.PASSWORD))
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.InvalidReEnter, "原密码");
                    return amm;
                }
                modifyUser.PASSWORD = DataHelper.TOMD5(UserInfo.PASSWORD);

                int iret = bs.UpdateEntity(modifyUser, new string[] { "PASSWORD" });
                if (iret > 0)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = string.Format(Message.OptSussess, "用户密码", Message.EditOpt);
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.OptFail, "用户密码", Message.EditOpt);
                }
            }
            catch (Exception)
            {
                return amm;
            }
            return amm;
        }
        #endregion

        #region 设置用户默认展开系统
        /// <summary>
        /// 设置用户默认展开系统
        /// </summary>
        public AjaxMsgModel SetDefuSystem(string menuID)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            //try
            //{
                if (bs.Entities<SYS_USER_DEFAULTMENU>().Where(d => d.USER_NAME.Equals(oc.CurrentUser.USER_NAME)).FirstOrDefault() != null)
                {
                //用户已经设置了子系统
                //Expression<Func<SYS_USER_DEFAULTMENU, object>>[] ignoreProperties = new Expression<Func<SYS_USER_DEFAULTMENU, object>>[] {
                //    p=>p.SYS_MENU,p=>p.SYS_USER
                //    };
                SYS_USER_DEFAULTMENU userDefu = bs.Entities<SYS_USER_DEFAULTMENU>().Where(d => d.USER_NAME.Equals(oc.CurrentUser.USER_NAME))
                        .FirstOrDefault();
                    userDefu.MENU_ID = menuID;
                    if (bs.UpdateEntity(userDefu, new string[] { "MENU_ID" }) > 0)
                    {
                        amm.Statu = AjaxStatu.ok;
                        amm.Msg = string.Format(Message.OptSussess, "默认系统", Message.SetOpt);
                    }
                }
                else
                {
                    SYS_USER_DEFAULTMENU userDefu = new SYS_USER_DEFAULTMENU
                    {
                        MENU_ID = menuID,
                        USER_NAME = oc.CurrentUser.USER_NAME,
                        USER_DEFAULT_ID = DateTime.Now.ToString("yyyyMMddHHmmssfff")
                    };
                    if (bs.AddEntity(userDefu) > 0)
                    {
                        amm.Statu = AjaxStatu.ok;
                        amm.Msg = string.Format(Message.OptSussess, "默认系统", Message.SetOpt);
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

       

        #region 新增用户
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public AjaxMsgModel Add(SYS_USER UserInfo)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            //try
            //{
                if (bs.Entities<SYS_DEPT>().Where(o => o.DEPT_CODE == UserInfo.DEPT_CODE).Count() == 0)
                {
                    //amm.Msg = "该部门不存在或已删除！";
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.NotFound, "部门");
                    return amm;
                }
                if (bs.Entities<SYS_USER>().Where(m => m.USER_NAME == UserInfo.USER_NAME).ToList().Count > 0)
                {
                    //amm.Msg = "用户名已经存在！";
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.YesFound, "用户名");
                    return amm;
                }

                if (bs.AddEntity(UserInfo) > 0)
                {
                    amm.Statu = AjaxStatu.ok;
                    //amm.Msg = "用户新增成功！";
                    amm.Msg = string.Format(Message.OptSussess, "用户", Message.AddOpt);
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    //amm.Msg = "用户新增失败！";
                    amm.Msg = string.Format(Message.OptFail, "用户", Message.AddOpt);
                }
            //}
            //catch (Exception)
            //{
            //    return amm;
            //}
            return amm;
        }
        #endregion

        #region 修改用户
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="UserInfo">用户</param>
        /// <returns>AjaxMsgModel实体对象</returns>
        public AjaxMsgModel Edit(SYS_USER UserInfo, string password)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            //try
            //{
                if (bs.Entities<SYS_DEPT>().Where(o => o.DEPT_CODE == UserInfo.DEPT_CODE).Count() == 0)
                {
                    //amm.Msg = "该部门不存在或已删除！";
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.NotFound, "部门");
                    return amm;
                }

                if (password == "")//如果不修改密码
                {
                    UserInfo.PASSWORD = bs.Entities<SYS_USER>().Where(m => m.USER_NAME == UserInfo.USER_NAME).Select(m => m.PASSWORD).FirstOrDefault();
                }

                int returnValue = 0;
                using (TransactionScope ts = new TransactionScope())
                {
                    //删除用户角色
                    returnValue = bs.DelByWhere<SYS_USER_ROLE_MAP>(m => m.USER_NAME == UserInfo.USER_NAME);
                    //增加新用户角色
                    returnValue = bs.AddListEntity(UserInfo.SYS_USER_ROLE_MAP.ToList());
                    //Expression<Func<SYS_USER, object>>[] ignoreProperties =
                    //                        new Expression<Func<SYS_USER, object>>[] {p=>p.SYS_USER_ROLE_MAP,p=>p.SYS_USER_DEFAULTMENU,p=>p.SYS_DEPT,p=>p.PHONE_USER};
                    returnValue = bs.UpdateEntity(UserInfo, new string[] { "PASSWORD", "DEPT_CODE", "UPDATE_DATE", "UPDATE_USER", "NOTE", "MANAGE_DEPT_CODE", "ZSNAME" });
                    ts.Complete();
                }
                if (returnValue > 0)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = string.Format(Message.OptSussess, "用户", Message.EditOpt);
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.OptFail, "用户", Message.EditOpt);
                }
            //}
            //catch (Exception)
            //{
            //    return amm;
            //}
            return amm;
        }
        #endregion

        #region 删除用户
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="UserInfo">用户</param>
        /// <returns>AjaxMsgModel实体对象</returns>
        public AjaxMsgModel Del(string UserName)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            //try
            //{

                int returnValue = 0;
                using (TransactionScope ts = new TransactionScope())
                {
                    //删除用户角色
                    returnValue = bs.DelByWhere<SYS_USER_ROLE_MAP>(m => m.USER_NAME == UserName);
                    //删除用户模块
                    returnValue = bs.DelByWhere<SYS_USER_DEFAULTMENU>(m => m.USER_NAME == UserName);
                    //删除用户
                    returnValue = bs.DelByWhere<SYS_USER>(m => m.USER_NAME == UserName);
                    ts.Complete();
                }
                if (returnValue > 0)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = string.Format(Message.OptSussess, "用户", Message.DelOpt);
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = string.Format(Message.OptFail, "用户", Message.DelOpt);
                }
            //}
            //catch (Exception)
            //{
            //    return amm;
            //}
            return amm;
        }
        #endregion

        
    }
}