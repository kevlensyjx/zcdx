using AutoMapper;
using SolutionWeb.Common;
using SolutionWeb.Interface;
using SolutionWeb.Model.POLICY;
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
    public class POLICY_BASE_CONFIGController : BaseController
    {
        #region Identity
        private IBaseService bs = null;
        private IUserLogin ul = null;

        public POLICY_BASE_CONFIGController(IBaseService baseService, IUserLogin userLogin)
        {
            this.bs = baseService;
            this.ul = userLogin;
        }
        #endregion
        // GET: POLICY/POLICY_BASE_CONFIG
        #region 参数设置
        [AjaxRequest]
        [Description("参数设置")]
        [HttpGet]
        public ActionResult Index()
        {
            string ControllerUrl = "/api/POLICY/POLICY_BASE_CONFIG/";
            var viewModel = new
            {
                Permission = new//权限
                {
                    a_list = this.ul.HasPermission("POLICY", "POLICY_BASE_CONFIG", "List", Common.HttpMethod.Post),
                   
                    a_edit = this.ul.HasPermission("POLICY", "POLICY_BASE_CONFIG", "Edit", Common.HttpMethod.Get),
                    
                },
                resx = new
                {
                    listTitle = "您没有【查看参数设置】权限",
                    addTitle = "您没有【新增参数设置】权限",
                    editTitle = "您没有【编辑参数设置】权限！",
                    deleteTitle = "您没有【删除参数设置】权限！"
                },
                urls = new//请求URL
                {
                    save = ControllerUrl + "Save",
                    list = ControllerUrl + "List",
                    edit = ControllerUrl + "Edit",
                    del = ControllerUrl + "Del",
                    add = ControllerUrl + "Add",

                    dataGgridName = "data_grid",//列表ID
                    dataGgridType = "datagrid",//列表类型
                    dataAddName = "data_add",//增加窗口

                    dataFormName = "DataForm",//提交表单
                },
                searchForm = new VIEW_BASE_CONFIG()//查询
                {
                },
                addForm = new VIEW_BASE_CONFIG()
                { //添加修改
                } 
            };
            return View(viewModel);
        }
        #endregion
    }

    public class POLICY_BASE_CONFIGApiController : BaseApiController
    {
        #region Ioc
        private IBaseService bs = null;
        public POLICY_BASE_CONFIGApiController(IBaseService baseService)
        {
            this.bs = baseService;
        }
        #endregion

        #region 查询
        [HttpPost]//查询
        public ViewModel List(VIEW_BASE_CONFIG data)
        {
            int pageSize = int.Parse(data.rows);
            int pageIndex = int.Parse(data.page);
            string sort = data.sort;
            string order = data.order;

            //查询条件
            IQueryable<BASE_CONFIG> VIEW_BASE_CONFIGEntity = bs.Entities<BASE_CONFIG>();
            

            int total = 0;

            total = VIEW_BASE_CONFIGEntity.Count();
            var listproject = VIEW_BASE_CONFIGEntity.OrderBy(u => u.CONFIG_NAME).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

            return ObjToJson.ViewModelToJson(listproject, total);
        }
        #endregion

        #region 修改
        [HttpPost]
        public AjaxMsgModel Edit(VIEW_BASE_CONFIG data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {
                BASE_CONFIG info = bs.Entities<BASE_CONFIG>().Where(m => m.SID == data.SID).FirstOrDefault();

                if (info != null)
                {
                    Mapper.CreateMap<BASE_CONFIG, VIEW_BASE_CONFIG>();
                    VIEW_BASE_CONFIG vm = Mapper.Map<BASE_CONFIG, VIEW_BASE_CONFIG>(info);
                    amm.Statu = AjaxStatu.ok;
                    amm.Data = vm;
                    return amm;
                }
                else
                {
                    amm.Msg = string.Format(Message.NotFound, "参数");
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
    }
}