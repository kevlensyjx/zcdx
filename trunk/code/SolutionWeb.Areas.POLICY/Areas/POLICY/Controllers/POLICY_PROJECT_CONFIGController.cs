using AutoMapper;
using SolutionWeb.Common;
using SolutionWeb.Interface;
using SolutionWeb.Model.POLICY;
using SolutionWeb.Model.SYS;
using SolutionWeb.Models;
using SolutionWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace SolutionWeb.Areas.POLICY.Controllers
{
    public class POLICY_PROJECT_CONFIGController : BaseController
    {
        // GET: POLICY/POLICY_PROJECT_CONFIG
        #region Identity
        private IBaseService bs = null;
        private IUserLogin ul = null;

        public POLICY_PROJECT_CONFIGController(IBaseService baseService, IUserLogin userLogin)
        {
            this.bs = baseService;
            this.ul = userLogin;
        }
        #endregion

        #region 项目信息
        [AjaxRequest]
        [Description("项目配置")]
        [HttpGet]
        public ActionResult Index()
        {
            string ControllerUrl = "/api/POLICY/POLICY_PROJECT_CONFIG/";
            var viewModel = new
            {
                Permission = new//权限
                {
                    a_list = this.ul.HasPermission("POLICY", "POLICY_PROJECT_CONFIG", "List", Common.HttpMethod.Post),
                    a_add = this.ul.HasPermission("POLICY", "POLICY_PROJECT_CONFIG", "Add", Common.HttpMethod.Post),
                    a_del = this.ul.HasPermission("POLICY", "POLICY_PROJECT_CONFIG", "Del", Common.HttpMethod.Get),
                },
                resx = new
                {
                    listTitle = "您没有【查看项目配置】权限",
                    addTitle = "您没有【新增项目配置】权限",
                    editTitle = "您没有【编辑项目配置】权限！",
                    deleteTitle = "您没有【删除项目配置】权限！"
                },
                urls = new//请求URL
                {
                    save = ControllerUrl + "Save",
                    list = ControllerUrl + "List",
                    edit = ControllerUrl + "Edit",
                    del = ControllerUrl + "Del",

                    dataGgridName = "data_grid",//列表ID
                    dataGgridType = "datagrid",//列表类型
                    dataAddName = "data_add",//增加窗口

                    dataFormName = "DataForm",//提交表单
                },
                searchForm = new VIEW_BASE_PROJECT_CONFIG()//查询
                {
                },
                addForm = new VIEW_BASE_PROJECT_CONFIG()
                { //添加修改
                },
                extForm = new//扩展类
                {
                    extA = new List<EasyUIComBoBoxNode>(),
                    extB = new List<EasyUIComBoBoxNode>(),
                    extC = new List<EasyUIComBoBoxNode>(),
                    extD = ul.GetMyORGNoGQTree(oc.CurrentUser.DEPT_CODE, oc.CurrentUser.PARENT_CODE),
                    extE = Model_BASE_PROJECT_INFO.Create.GetProjectTree() 
                }
            };
            return View(viewModel);
        }
        #endregion

        #region 项目信息
        [AjaxRequest]
        [Description("项目配置")]
        [HttpGet]
        [Skip]
        public ActionResult myIndex()
        {
            string ControllerUrl = "/api/POLICY/POLICY_PROJECT_CONFIG/";
            var viewModel = new
            {
                Permission = new//权限
                {
                    a_list = true,
                    a_add = true,
                    a_edit = true,
                    a_del = true,
                },
                resx = new
                {
                    listTitle = "您没有【查看项目配置】权限",
                    addTitle = "您没有【新增项目配置】权限",
                    editTitle = "您没有【编辑项目配置】权限！",
                    deleteTitle = "您没有【删除项目配置】权限！"
                },
                urls = new//请求URL
                {
                    save = ControllerUrl + "Save",
                    list = ControllerUrl + "List",
                    mylist = ControllerUrl + "myList",
                    edit = ControllerUrl + "Edit",
                    del = ControllerUrl + "Del",

                    dataGgridName = "data_grid",//列表ID
                    dataGgridType = "datagrid",//列表类型
                    dataAddName = "data_add",//增加窗口

                    dataFormName = "DataForm",//提交表单
                },
                searchForm = new VIEW_BASE_PROJECT_CONFIG()//查询
                {
                },
                extForm = new//扩展类
                {
                    extA = new List<EasyUIComBoBoxNode>(),
                    extB = new List<EasyUIComBoBoxNode>(),
                    extC = new List<EasyUIComBoBoxNode>(),
                    extD = ul.GetMyORGNoGQTree(oc.CurrentUser.DEPT_CODE, oc.CurrentUser.PARENT_CODE),
                    extE = Model_BASE_PROJECT_INFO.Create.GetProjectTree()
                }
            };
            return View(viewModel);
        }
        #endregion
    }

    public class POLICY_PROJECT_CONFIGApiController : BaseApiController
    {
        #region Ioc
        private IBaseService bs = null;
        public POLICY_PROJECT_CONFIGApiController(IBaseService baseService)
        {
            this.bs = baseService;
        }
        #endregion

        #region 查询
        [HttpPost]//查询
        public ViewModel List(VIEW_BASE_PROJECT_CONFIG data)
        {
            int pageSize = int.Parse(data.rows);
            int pageIndex = int.Parse(data.page);
            string sort = data.sort;
            string order = data.order;

            List<ConfigInfo> list = new List<ConfigInfo>();
            //查询条件
            IQueryable<BASE_PROJECT_CONFIG> BASE_PROJECT_CONFIGEntity = bs.Entities<BASE_PROJECT_CONFIG>();
            if (!string.IsNullOrEmpty(data.ITEM_TYPE))
            {
                BASE_PROJECT_CONFIGEntity = BASE_PROJECT_CONFIGEntity.Where(u => u.BASE_PROJECT_INFO.ITEM_TYPE.Equals(data.ITEM_TYPE));
            }
            

            int total = 0;

           // total = BASE_PROJECT_CONFIGEntity.Count();
            var listproject =
                BASE_PROJECT_CONFIGEntity.OrderBy(u => u.BASE_PROJECT_INFO.ITEM_TYPE)
                    .ThenBy(u => u.BASE_PROJECT_INFO.LAY_ORDER)
                    .Select(o => new VIEW_BASE_PROJECT_CONFIG
                    {
                        SID = o.SID,
                        PROJECT_SID = o.BASE_PROJECT_INFO.SID,
                        ITEM_TYPE = o.BASE_PROJECT_INFO.ITEM_TYPE,
                        ITEM_NAME = o.BASE_PROJECT_INFO.ITEM_NAME,
                        DEPT_TYPE = o.DEPT_TYPE,
                        DEPT_CODE = o.DEPT_CODE,
                        DEPT_NAME = o.DEPT_NAME,
                        CASHING_WAY = o.BASE_PROJECT_INFO.CASHING_WAY
                    }).ToList();
            var tmp = listproject.ToLookup(o => o.ITEM_NAME);
            foreach (var item in tmp)
            {
                if (item.Count() > 0)
                {
                    ConfigInfo info = new ConfigInfo
                    {
                        PROJECT_SID = item.FirstOrDefault().PROJECT_SID,
                        ITEM_NAME = item.Key,
                        ITEM_TYPE = item.FirstOrDefault().ITEM_TYPE,
                        CASHING_WAY = item.FirstOrDefault().CASHING_WAY
                    };
                    var q = item.Where(o => o.DEPT_TYPE == "牵头部门").FirstOrDefault();
                    if (q != null)
                    {
                        info.DEPT_Q = q.DEPT_NAME;
                    }
                    info.DEPT_H = string.Join(",", item.Where(o => o.DEPT_TYPE == "会审部门").Select(o => o.DEPT_NAME));
                    list.Add(info);
                }
            }
            total = list.Count;
            return ObjToJson.ViewModelToJson(list.Skip(pageSize * (pageIndex - 1)).Take(pageSize), total);
        }
        #endregion

        #region 查询
        [HttpPost]//查询
        public ViewModel myList(VIEW_BASE_PROJECT_CONFIG data)
        {
            int pageSize = int.Parse(data.rows);
            int pageIndex = int.Parse(data.page);
            string sort = data.sort;
            string order = data.order;

            //查询条件
            IQueryable<BASE_PROJECT_INFO> BASE_PROJECT_INFOEntity = bs.Entities<BASE_PROJECT_INFO>();
            List<BASE_PROJECT_CONFIG> BASE_PROJECT_CONFIGList= bs.Entities<BASE_PROJECT_CONFIG>().ToList();
            if (!string.IsNullOrEmpty(data.ITEM_TYPE))
            {
                BASE_PROJECT_INFOEntity = BASE_PROJECT_INFOEntity.Where(u => u.ITEM_TYPE.Equals(data.ITEM_TYPE));
            }
            List<BASE_PROJECT_INFO> projectList = BASE_PROJECT_INFOEntity.ToList();


            List<VIEW_BASE_PROJECT_CONFIG> allList_temp = new List<VIEW_BASE_PROJECT_CONFIG>();
            foreach (var item in projectList)
            {
                var project_configList = BASE_PROJECT_CONFIGList.Where(o => o.PROJECT_SID == item.SID).ToList();
                VIEW_BASE_PROJECT_CONFIG model = new VIEW_BASE_PROJECT_CONFIG();
                model.DEPT_NAME = "AAAAAAA";
                foreach (var configitem in project_configList)
                {

                    if (configitem.DEPT_TYPE == "牵头部门")
                    {
                        model.DEPT_TYPE = configitem.DEPT_NAME;
                        model.SID = configitem.SID;
                        model.PROJECT_SID = configitem.PROJECT_SID;
                        model.ITEM_TYPE = configitem.BASE_PROJECT_INFO.ITEM_TYPE;
                        model.ITEM_NAME = configitem.BASE_PROJECT_INFO.ITEM_NAME;
                        model.CASHING_WAY = configitem.BASE_PROJECT_INFO.CASHING_WAY;
                        model.LAY_ORDER = item.LAY_ORDER.ToString();
                    }
                    else if (configitem.DEPT_TYPE == "会审部门")
                    {
                        model.DEPT_NAME = model.DEPT_NAME +"，"+ configitem.DEPT_NAME;
                    }
                    if (!string.IsNullOrEmpty(model.DEPT_NAME))
                        model.DEPT_NAME = model.DEPT_NAME.Replace("AAAAAAA，", "");
                }
                allList_temp.Add(model);

            }

            int total = 0;

            total = allList_temp.Count();
            var listproject =
                allList_temp.OrderBy(u => u.ITEM_TYPE)
                    .ThenBy(u => u.LAY_ORDER)
                    .Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

            return ObjToJson.ViewModelToJson(listproject, total);
        }
        #endregion

        #region 修改
        [HttpPost]
        public AjaxMsgModel Edit(VIEW_BASE_PROJECT_CONFIG data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {
                BASE_PROJECT_CONFIG info = bs.Entities<BASE_PROJECT_CONFIG>().Where(m => m.SID == data.SID).FirstOrDefault();

                if (info != null)
                {
                    Mapper.CreateMap<BASE_PROJECT_CONFIG, VIEW_BASE_PROJECT_CONFIG>();
                    VIEW_BASE_PROJECT_CONFIG vm = Mapper.Map<BASE_PROJECT_CONFIG, VIEW_BASE_PROJECT_CONFIG>(info);
                    amm.Statu = AjaxStatu.ok;
                    amm.Data = vm;
                    return amm;
                }
                else
                {
                    amm.Msg = string.Format(Message.NotFound, "项目");
                    return amm;
                }
            }
            catch (Exception ex)
            {
                RecordLog.RecordError(ex.ToString());
                return amm;
            }
        }
        #endregion

        #region 保存
        [HttpPost]
        public AjaxMsgModel Save(VIEW_BASE_PROJECT_CONFIG data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {
                if (data.IS_VOTE == "true") data.IS_VOTE = "1";
                if (data.IS_VOTE == "false" || data.IS_VOTE == null) data.IS_VOTE = "0";
                Mapper.CreateMap<VIEW_BASE_PROJECT_CONFIG, BASE_PROJECT_CONFIG>();
                BASE_PROJECT_CONFIG u = Mapper.Map<VIEW_BASE_PROJECT_CONFIG, BASE_PROJECT_CONFIG>(data);
                var dept = bs.Entities<SYS_DEPT>().Where(o => o.DEPT_CODE == u.DEPT_CODE).FirstOrDefault();
                if (dept != null)
                {
                    u.DEPT_NAME = dept.DEPT_NAME;

                    if (string.IsNullOrEmpty(u.SID))
                    {
                        u.SID = Guid.NewGuid().ToString();
                        amm = Model_BASE_PROJECT_CONFIG.Create.Add(u);
                    }
                    else
                    {
                        amm = Model_BASE_PROJECT_CONFIG.Create.Edit(u);
                    }
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

        #region 删除
        [HttpPost]
        public AjaxMsgModel Del(VIEW_BASE_PROJECT_CONFIG data)
        {
            return Model_BASE_PROJECT_CONFIG.Create.Del(data.PROJECT_SID);
        }
        #endregion
    }


    public class ConfigInfo
    {
        public string PROJECT_SID { get; set; }
        public string ITEM_TYPE { get; set; }
        public string ITEM_NAME { get; set; }
        public string CASHING_WAY { get; set; }
        public string DEPT_Q { get; set; }
        public string DEPT_H { get; set; }
    }

}