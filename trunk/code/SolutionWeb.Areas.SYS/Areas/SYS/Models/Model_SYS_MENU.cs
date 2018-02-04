using SolutionWeb.Common;
using SolutionWeb.Model.SYS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolutionWeb.Models
{
    public partial class Model_SYS_MENU
    {
        #region 根据用户名得到对应的权限信息
        /// <summary>
        /// 根据用户编号得到对应的权限信息
        /// </summary>
        /// <param name="USER_NAME">用户名</param>
        /// <returns>权限集合</returns>
        public List<SESS_MENU> GetUserPermission(string USER_NAME)
        {
            //获取不显示的菜单编号(集合)
            List<SYS_MENU> AllMenu = bs.Entities<SYS_MENU>().ToList();
            List<string> noShowIDtemp = AllMenu.Where(u => (!string.IsNullOrEmpty(u.ISSHOW_FLAG) && u.ISSHOW_FLAG.Equals("0")))
                .Select(u => u.MENU_ID).ToList();
            List<string> noShowIDs = new List<string>();
            foreach (string menuid in noShowIDtemp)
            {
                List<string> noShowTempIDList = AllMenu.Where(u => u.MENU_ID.StartsWith(menuid)).Select(u => u.MENU_ID).ToList();
                noShowIDs.AddRange(noShowTempIDList);
            }

            //--根据用户名得到角色编号(集合)
            List<string> listRoleId = bs.Entities<SYS_USER_ROLE_MAP>().Where(u => u.USER_NAME == USER_NAME).Select(u => u.ROLE_ID).ToList();
            #region 新写法
            //根据角色编号得到菜单权限编号(集合)

            List<SYS_ROLE_MENU_MAP> listMenus = bs.Entities<SYS_ROLE_MENU_MAP>().ToList();
            List<string> listMenuIds = (from menuMap in listMenus
                                        join roleid in listRoleId on menuMap.ROLE_ID equals roleid
                                        select menuMap.MENU_ID).Distinct().ToList();
            for (int i = 0; i < noShowIDs.Count; i++)
            {
                if (listMenuIds.Contains(noShowIDs[i]))
                {
                    listMenuIds.Remove(noShowIDs[i]);
                }
            }



            //根据权限编号得到权限的具体信息
            List<SYS_MENU> listsysMenus = bs.Entities<SYS_MENU>().ToList();
            List<SYS_MENU> listMenu_temp =
                (from sys_menu in listsysMenus
                 join menuid in listMenuIds on sys_menu.MENU_ID equals menuid
                 select sys_menu).ToList();

            List<SYS_MENU> listMenu = new List<SYS_MENU>();
            SYS_MENU modelMenu = new SYS_MENU();
            for (int i = 0; i < listMenu_temp.Count; i++)
            {
                modelMenu = listMenu_temp[i];
                if (!noShowIDs.Contains(modelMenu.MENU_ID))
                {
                    listMenu.Add(modelMenu);
                }
            }


            //根据角色编号得到菜单按纽权限编号(集合)
            List<SYS_ROLE_MENUOPT_MAP> listmMenuoptMaps = bs.Entities<SYS_ROLE_MENUOPT_MAP>().ToList();
            List<string> listMenuOptIds = (from menuopt in listmMenuoptMaps
                                           join roleid in listRoleId on menuopt.ROLE_ID equals roleid
                                           select menuopt.MENUOPT_ID).Distinct().ToList();



            //根据权限编号得到权限的具体信息

            List<SYS_MENUOPT> listmenuopt = bs.Entities<SYS_MENUOPT>().ToList();
            List<SYS_MENU> listBM_temp = (from u in listmenuopt
                                          join menuoptid in listMenuOptIds on u.MENUOPT_ID equals menuoptid
                                          select new SYS_MENU
                                          {
                                              MENU_ID = u.MENUOPT_ID,
                                              AREA = u.AREA,
                                              CONTROLLER = u.CONTROLLER,
                                              ACTION = u.ACTION,
                                              MENU_NAME = u.MENUOPT_NAME,
                                              C_ICO = u.C_ICO,
                                              MENU_LEVEL = 100,
                                              PARENT_ID = "100"
                                          }).ToList();

            List<SYS_MENU> listBM = new List<SYS_MENU>();
            for (int i = 0; i < listBM_temp.Count; i++)
            {
                modelMenu = listBM_temp[i];
                if (!noShowIDs.Contains(modelMenu.MENU_ID))
                {
                    listBM.Add(modelMenu);
                }
            }
            listMenu.AddRange(listBM);

            #endregion
            
            List<SESS_MENU> listSessMenu = listMenu.OrderBy(u => u.MENU_ID)
                .Select(u =>
                        new SESS_MENU
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
        
                        }
                    ).ToList();

            return listSessMenu;
        }

        #endregion


        #region  获取隐藏超级用户权限
        /// <summary>
        /// 隐藏超级用户权限
        /// </summary>
        /// <returns></returns>
        public List<SESS_MENU> GetSuperAdminPermission()
        {
            //获取所有菜单
            List<SYS_MENU> listMenu = bs.Entities<SYS_MENU>().ToList();
            List<SYS_MENU> listthreemenu = listMenu.Where(m => m.MENU_LEVEL == 3).ToList();
            foreach (SYS_MENU menu in listthreemenu)
            {
                //手动添加三级菜单权限
                if (menu.MENU_LEVEL == 3)
                {
                    SYS_MENU menuopt_add = new SYS_MENU
                    {
                        MENU_ID = menu + "001",
                        AREA = menu.AREA,
                        CONTROLLER = menu.CONTROLLER,
                        ACTION = "Add",
                        MENU_NAME = menu.MENU_NAME + Message.AddOpt,
                        C_ICO = menu.C_ICO,
                        MENU_LEVEL = 100,
                        PARENT_ID = "100"
                    };
                    listMenu.Add(menuopt_add);
                    SYS_MENU menuopt_list = new SYS_MENU
                    {
                        MENU_ID = menu + "002",
                        AREA = menu.AREA,
                        CONTROLLER = menu.CONTROLLER,
                        ACTION = "List",
                        MENU_NAME = menu.MENU_NAME + Message.ReadOpt,
                        C_ICO = menu.C_ICO,
                        MENU_LEVEL = 100,
                        PARENT_ID = "100"
                    };
                    listMenu.Add(menuopt_list);
                    SYS_MENU menuopt_edit = new SYS_MENU
                    {
                        MENU_ID = menu + "003",
                        AREA = menu.AREA,
                        CONTROLLER = menu.CONTROLLER,
                        ACTION = "Edit",
                        MENU_NAME = menu.MENU_NAME + Message.EditOpt,
                        C_ICO = menu.C_ICO,
                        MENU_LEVEL = 100,
                        PARENT_ID = "100"
                    };
                    listMenu.Add(menuopt_edit);
                    SYS_MENU menuopt_del = new SYS_MENU
                    {
                        MENU_ID = menu + "004",
                        AREA = menu.AREA,
                        CONTROLLER = menu.CONTROLLER,
                        ACTION = "Del",
                        MENU_NAME = menu.MENU_NAME + Message.DelOpt,
                        C_ICO = menu.C_ICO,
                        MENU_LEVEL = 100,
                        PARENT_ID = "100"
                    };
                    listMenu.Add(menuopt_del);
                }
            }
            List<SESS_MENU> listSessMenu = listMenu.OrderBy(u => u.MENU_ID)
                .Select(u =>
                        new SESS_MENU
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
                        }
                    ).ToList();

            return listSessMenu;
        }
        #endregion


        #region 判断菜单权限
        public bool HasPermission(string areaName, string controllerName, string actionName, HttpMethod httpmethod)
        {
            bool flag = true;
            if (oc.UserMenuPermission == null)
            {
                flag = IsLogin();
            }
            if (flag)
            {
                var listP = from per in oc.UserMenuPermission
                            where
                            string.Equals(per.AREA, areaName, StringComparison.CurrentCultureIgnoreCase)
                        && string.Equals(per.CONTROLLER, controllerName, StringComparison.CurrentCultureIgnoreCase)
                        && string.Equals(per.ACTION, actionName, StringComparison.CurrentCultureIgnoreCase)
                            //&& per.pFormMethod == (int)httpmethod
                            select per;

                return listP.Count() > 0;
            }
            return flag;
        }
        #endregion

        #region 判断是否登录
        public bool IsLogin()
        {

            if (oc.CurrentUser == null)
            {

                if (oc.CurrentUserName != "")
                {
                    ////根据登录名得到用户信息
                    var users = bs.Entities<SYS_USER>().Where(u => u.USER_NAME == oc.CurrentUserName).Select(u => new
                    {
                        u.DEPT_CODE,
                        u.USER_NAME,
                        u.PASSWORD,
                        u.SYS_DEPT.DEPT_NAME,//部门名称
                        u.SYS_DEPT.PARENT_CODE,//部门父ID
                        u.MANAGE_DEPT_CODE,//管理部门ID
                        ZSNAME = u.ZSNAME
                    }).ToList();

                    if (users.Count > 0)
                    {
                        var cUsr = users.First();
                        ///*把用户信息再次放入Session*/
                        oc.CurrentUser = new SESS_USER
                        {
                            USER_NAME = cUsr.USER_NAME,
                            MANAGE_DEPT_CODE = cUsr.MANAGE_DEPT_CODE,
                            ZSNAME = cUsr.ZSNAME,
                            DEPT_NAME = cUsr.DEPT_NAME,
                            DEPT_CODE = cUsr.DEPT_CODE,
                            PARENT_CODE = cUsr.PARENT_CODE
                        };


                        ///**
                        // * 保存当前用户的菜单权限信息
                        // */
                        oc.UserMenuPermission = Model_SYS_MENU.Create.GetUserPermission(cUsr.USER_NAME);
                    }
                    return true;

                }
                return false;
            }

            return true;
        }

        #endregion
        #region 将权限菜单转为EASYUI树
        public EasyUITreeNode TransformTreeNode(SYS_MENU menu)
        {
            //int menuCloseState =Convert.ToInt32(ConfigurationSettings.AppSettings["MenuCloseState"]);
            EasyUITreeNode easyUITreeNode = new EasyUITreeNode()
            {
                id = menu.MENU_ID,
                text = menu.MENU_NAME,
                state = menu.MENU_LEVEL == 3 ? "closed" : "open",//只有存在下级才可设为closed，否则会循环查询
                iconCls = menu.C_ICO,
                Checked = true,//是否选中,
                area = menu.AREA,
                haspermission = string.Empty,//用于平台子系统权限判断
                isdefu = string.Empty,//用于平台获取默认子系统
                isdefuopen = string.Empty,//用于打开默认二，三级菜单
                gisorder = menu.GIS_ORDER == null ? string.Empty : menu.GIS_ORDER,//用于加载二三级菜单
                attributes = new { url = GetUrl(menu), menulevel = menu.MENU_LEVEL },
                children = new List<EasyUITreeNode>()
            };
            return easyUITreeNode;
        }
        private string GetUrl(SYS_MENU menu)
        {
            return FormatUrl(menu.AREA)
                  + FormatUrl(menu.CONTROLLER)
                  + FormatUrl(menu.ACTION);
        }
        private string FormatUrl(string name)
        {
            return string.IsNullOrEmpty(name) ? "" : "/" + name;
        }
        #endregion

        #region 把权限菜单转为符合EASYUI的带有递归关系的集合
        public List<EasyUITreeNode> ConvertTreeNodes(List<SYS_MENU> listMenus, string pid)
        {
            List<EasyUITreeNode> listTreeNodes = new List<EasyUITreeNode>();
            LoadTreeNode(listMenus, listTreeNodes, pid);
            DelClosed(listTreeNodes);
            return listTreeNodes;
        }
        /// <summary>
        /// 根据是否有下级来更改打开关闭状态,这是解决同步加载的问题，如果是异步加载？
        /// </summary>
        /// <param name="listTreeNodes"></param>
        private void DelClosed(List<EasyUITreeNode> listTreeNodes)
        {
            foreach (EasyUITreeNode uinode in listTreeNodes)
            {
                //只有存在下级才可设为closed，否则会循环查询
                if (uinode.children.Count == 0 && uinode.state == "closed")
                {
                    uinode.state = "open";
                }
                else
                {
                    DelClosed(uinode.children);
                }
            }
        }
        private void LoadTreeNode(List<SYS_MENU> listMenus, List<EasyUITreeNode> listTreeNodes, string pid)
        {
            foreach (SYS_MENU menu in listMenus)
            {
                if (menu.PARENT_ID.ToString() == pid)
                {
                    EasyUITreeNode node = TransformTreeNode(menu);
                    //if (!string.IsNullOrEmpty(node.gisorder)) {
                    //    node.text = "GIS监控";
                    //}
                    listTreeNodes.Add(node);
                    LoadTreeNode(listMenus, node.children, node.id);
                }

            }
        }
        #endregion


        #region 获取地图左侧菜单
        public string GetMyGISMenu()
        {
            string strJson = "[]";
            List<SYS_MENU> listMenu = oc.UserMenuPermission.Where(m => m.MENU_LEVEL != Constant.MyMenuLevel).OrderBy(m => m.GIS_ORDER)
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
                List<EasyUITreeNode> listTreeNodes = new List<EasyUITreeNode>();
                //获取用户默认子系统
                string defuMenuID = Constant.defuOneMenuParID;
                if (bs.Entities<SYS_USER_DEFAULTMENU>().Where(u => u.USER_NAME.Equals(oc.CurrentUser.USER_NAME)).FirstOrDefault() != null)
                {
                    defuMenuID = bs.Entities<SYS_USER_DEFAULTMENU>().Where(u => u.USER_NAME.Equals(oc.CurrentUser.USER_NAME)).FirstOrDefault().MENU_ID;
                }
                foreach (SYS_MENU menu in listMenu)
                {
                    EasyUITreeNode node = TransformTreeNode(menu);
                    if (defuMenuID.Contains(menu.PARENT_ID))
                    {
                        node.isdefu = "Y";
                    }
                    listTreeNodes.Add(node);
                }
                strJson = ObjToJson.GetToJson(listTreeNodes).Replace("Checked", "checked");
            }
            return strJson;
        } 
        #endregion
    }
}