using SolutionWeb.Common;
using SolutionWeb.Interface;
using SolutionWeb.Model.SYS;
using SolutionWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SolutionWeb.Areas.SYS.Controllers
{
    public class AdminController : BaseController
    {
        // GET: SYS/Admin

        #region Identity
        private IBaseService bs = null;
        public AdminController(IBaseService baseService)
        {
            this.bs = baseService;
        }
        #endregion

        /// <summary>
        /// 获取用户子系统导航(所有)
        /// </summary>
        /// <returns></returns>
        [Skip]
        [AjaxRequest]
        [HttpPost]
        public ActionResult GetMyZeroMenu()
        {
            string strJson = "[]";
            List<SYS_MENU> listMenu = bs.Entities<SYS_MENU>()
                .Where(m => m.PARENT_ID.Equals(Constant.systemParID)).OrderBy(m => m.MENU_ORDER).ToList();
            //2015-11-06 新增不显示菜单
            listMenu = listMenu.Where(u => string.IsNullOrEmpty(u.ISSHOW_FLAG) || (!string.IsNullOrEmpty(u.ISSHOW_FLAG) && !u.ISSHOW_FLAG.Equals("0"))).ToList();
            //List<string> showlistMenu=listMenu.Select(u=>u.MENU_ID).ToList();
            if (listMenu.Count > 0)
            {
                List<EasyUITreeNode> listTreeNodes = Model_SYS_MENU.Create.ConvertTreeNodes(listMenu, Constant.systemParID);
                //获取用户拥有权限的子系统
                List<string> hasPermissionSyss = oc.UserMenuPermission.Where(u => u.PARENT_ID.Equals(Constant.systemParID)).OrderBy(u => u.MENU_ORDER)
                    .Select(u => u.MENU_ID).ToList();
                //获取用户默认子系统
                string defuMenuID = GetDefuMenu();
                foreach (EasyUITreeNode node in listTreeNodes)
                {
                    if (node.id.Equals(defuMenuID))
                    {
                        node.isdefu = "Y";
                    }
                    else
                    {
                        node.isdefu = "N";
                    }
                    if (hasPermissionSyss.Contains(node.id))
                    {
                        node.haspermission = "Y";
                    }
                    else
                    {
                        node.haspermission = "N";
                    }
                }
                strJson = ObjToJson.GetToJson(listTreeNodes.OrderByDescending(m => m.haspermission)).Replace("Checked", "checked");
            }
            return Content(strJson);
        }


        [Skip]
        [AjaxRequest]
        [HttpPost]
        private string GetDefuMenu()
        {
            string resu = string.Empty;
            //获取用户拥有权限的子系统
            List<string> hasPermissionSyss = oc.UserMenuPermission.Where(u => u.PARENT_ID.Equals(Constant.systemParID)).OrderBy(u => u.MENU_ORDER)
                .Select(u => u.MENU_ID).ToList();
            if (hasPermissionSyss.Count > 0)
            {
                SYS_USER_DEFAULTMENU userDefu = bs.Entities<SYS_USER_DEFAULTMENU>().Where(u => u.USER_NAME.Equals(oc.CurrentUser.USER_NAME)).FirstOrDefault();
                string defuSetting = Constant.defuOneMenuParID;
                if (userDefu != null)
                {
                    if (hasPermissionSyss.Contains(userDefu.MENU_ID))
                    {
                        resu = userDefu.MENU_ID;
                    }
                    else
                    {
                        if (hasPermissionSyss.Contains(defuSetting))
                        {
                            resu = defuSetting;
                        }
                        else
                        {
                            resu = hasPermissionSyss[0];
                        }
                    }
                }
                else
                {
                    if (hasPermissionSyss.Contains(defuSetting))
                    {
                        resu = defuSetting;
                    }
                    else
                    {
                        resu = hasPermissionSyss[0];
                    }
                }
            }

            return resu;
        }

        /// <summary>
        /// 获取用户默认的一级导航
        /// </summary>
        /// <returns></returns>
        [Skip]
        [AjaxRequest]
        [HttpPost]
        public ActionResult GetMyOneMenu()
        {
            string strJson = "[]";
            //获取用户默认子系统menuid
            string defuMenuID = GetDefuMenu();
            if (!string.IsNullOrEmpty(defuMenuID))
            {
                SYS_MENU parMenu = bs.Entities<SYS_MENU>().Where(u => u.MENU_ID.Equals(defuMenuID)).FirstOrDefault();
                string defuMenu = string.Empty;
                try
                {
                    if (!string.IsNullOrEmpty(parMenu.DEFMENU_ID))
                    {
                        defuMenu = parMenu.DEFMENU_ID.Substring(0, 4);
                    }
                }
                catch
                {
                    defuMenu = string.Empty;
                }

                List<SYS_MENU> listMenu = oc.UserMenuPermission.Where(m => m.PARENT_ID.Equals(defuMenuID)).OrderBy(m => m.MENU_ORDER)
                           .Select(u=>new SYS_MENU
                           {
                               MENU_ID = u.MENU_ID,
                               MENU_NAME = u.MENU_NAME,
                               C_ICO = u.C_ICO,
                               AREA = u.AREA,
                               CONTROLLER = u.CONTROLLER,
                               ACTION = u.ACTION,
                               PARENT_ID = u.PARENT_ID,
                               MENU_LEVEL = u.MENU_LEVEL,

                               MENU_ORDER = u.MENU_ORDER,
                               GIS_ORDER = u.GIS_ORDER,
                               ISSHOW_FLAG = u.ISSHOW_FLAG,
                               DEFMENU_ID = u.DEFMENU_ID
                           }).ToList();
                if (listMenu.Count > 0)
                {
                    if (string.IsNullOrEmpty(defuMenu))
                    {
                        defuMenu = listMenu[0].MENU_ID;
                    }
                    List<EasyUITreeNode> listTreeNodes = Model_SYS_MENU.Create.ConvertTreeNodes(listMenu, defuMenuID);
                    foreach (EasyUITreeNode node in listTreeNodes)
                    {
                        if (node.id.Equals(defuMenu))
                        {
                            node.isdefuopen = "Y";
                        }
                        else
                        {
                            node.isdefuopen = "N";
                        }
                    }
                    strJson = ObjToJson.GetToJson(listTreeNodes).Replace("Checked", "checked");
                }
            }
            return Content(strJson);
        }

        /// <summary>
        /// 根据用户选择的子系统获取一级导航
        /// </summary>
        /// <returns></returns>
        [Skip]
        [AjaxRequest]
        [HttpPost]
        public ActionResult GetMySelectOneMenu(string id)
        {
            string strJson = "[]";
            SYS_MENU parMenu = bs.Entities<SYS_MENU>().Where(u => u.MENU_ID.Equals(id)).FirstOrDefault();
            string defuMenu = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(parMenu.DEFMENU_ID))
                {
                    defuMenu = parMenu.DEFMENU_ID.Substring(0, 4);
                }
            }
            catch
            {
                defuMenu = string.Empty;
            }
            List<SYS_MENU> listMenu = oc.UserMenuPermission.Where(m => m.PARENT_ID.Equals(id)).OrderBy(m => m.MENU_ORDER)
                           .Select(u => new SYS_MENU
                           {
                               MENU_ID = u.MENU_ID,
                               MENU_NAME = u.MENU_NAME,
                               C_ICO = u.C_ICO,
                               AREA = u.AREA,
                               CONTROLLER = u.CONTROLLER,
                               ACTION = u.ACTION,
                               PARENT_ID = u.PARENT_ID,
                               MENU_LEVEL = u.MENU_LEVEL,

                               MENU_ORDER = u.MENU_ORDER,
                               GIS_ORDER = u.GIS_ORDER,
                               ISSHOW_FLAG = u.ISSHOW_FLAG,
                               DEFMENU_ID = u.DEFMENU_ID
                           }).ToList();
            if (listMenu.Count > 0)
            {
                if (string.IsNullOrEmpty(defuMenu))
                {
                    defuMenu = listMenu[0].MENU_ID;
                }
                List<EasyUITreeNode> listTreeNodes = Model_SYS_MENU.Create.ConvertTreeNodes(listMenu, id);
                foreach (EasyUITreeNode node in listTreeNodes)
                {
                    if (node.id.Equals(defuMenu))
                    {
                        node.isdefuopen = "Y";
                    }
                    else
                    {
                        node.isdefuopen = "N";
                    }
                }
                strJson = ObjToJson.GetToJson(listTreeNodes).Replace("Checked", "checked");
            }
            return Content(strJson);
        }

        /// <summary>
        /// 获取二三级导航
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Skip]
        [AjaxRequest]
        [HttpPost]
        public ActionResult GetMyMenu(string id)
        {
            string strJson = "[]";

            //根据选择的一级菜单找对应模块默认打开的三级页面
            string parID = id.Substring(0, 2);
            SYS_MENU parMenu = bs.Entities<SYS_MENU>().Where(u => u.MENU_ID.Equals(parID)).FirstOrDefault();
            string twoMenuID = string.Empty;
            string threeMenuID = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(parMenu.DEFMENU_ID))
                {
                    twoMenuID = parMenu.DEFMENU_ID.Substring(0, 6);
                    threeMenuID = parMenu.DEFMENU_ID.Substring(0, 8);
                }
            }
            catch
            {
                twoMenuID = string.Empty;
                threeMenuID = string.Empty;
            }

            List<SYS_MENU> listMenu = oc.UserMenuPermission.Where(m => m.MENU_LEVEL != Constant.MyMenuLevel && m.PARENT_ID.StartsWith(id)).OrderBy(m => m.MENU_LEVEL)
                .ThenBy(m => m.MENU_ORDER)
                           .Select(u => new SYS_MENU
                           {
                               MENU_ID = u.MENU_ID,
                               MENU_NAME = u.MENU_NAME,
                               C_ICO = u.C_ICO,
                               AREA = u.AREA,
                               CONTROLLER = u.CONTROLLER,
                               ACTION = u.ACTION,
                               PARENT_ID = u.PARENT_ID,
                               MENU_LEVEL = u.MENU_LEVEL,

                               MENU_ORDER = u.MENU_ORDER,
                               GIS_ORDER = u.GIS_ORDER,
                               ISSHOW_FLAG = u.ISSHOW_FLAG,
                               DEFMENU_ID = u.DEFMENU_ID
                           }).ToList();
            if (listMenu.Count > 0)
            {
                if (string.IsNullOrEmpty(twoMenuID))
                {
                    SYS_MENU firstThreeMenu = listMenu.Where(u => u.MENU_LEVEL == 3).FirstOrDefault();
                    if (firstThreeMenu != null)
                    {
                        twoMenuID = firstThreeMenu.MENU_ID.Substring(0, 6);
                        threeMenuID = firstThreeMenu.MENU_ID;
                    }
                }
                List<EasyUITreeNode> listTreeNodes = Model_SYS_MENU.Create.ConvertTreeNodes(listMenu, id);
                foreach (EasyUITreeNode node in listTreeNodes)
                {
                    if (node.id.Equals(twoMenuID))
                    {
                        node.isdefuopen = "Y";
                    }
                    else
                    {
                        node.isdefuopen = "N";
                    }
                    foreach (EasyUITreeNode Childnode in node.children)
                    {
                        if (Childnode.id.Equals(threeMenuID))
                        {
                            Childnode.isdefuopen = "Y";
                        }
                        else
                        {
                            Childnode.isdefuopen = "N";
                        }
                    }
                }
                strJson = ObjToJson.GetToJson(listTreeNodes).Replace("Checked", "checked");
            }

            return Content(strJson);
        }
        /// <summary>
        /// 设置默认展示系统
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        [Skip]
        [HttpPost]
        [AjaxRequest]
        [Description("设置默认系统")]
        public JsonResult SetDefuSystem(string id)
        {
                if (!string.IsNullOrEmpty(id))
                {
                    return PackagingAjaxmsg(Model_SYS_USER.Create.SetDefuSystem(id));
                }
                return PackagingAjaxmsg(new Message().NewAmm);
        }
        
    }
}