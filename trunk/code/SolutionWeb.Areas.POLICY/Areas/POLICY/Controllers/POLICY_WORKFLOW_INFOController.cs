using AutoMapper;
using SolutionWeb.Common;
using SolutionWeb.Interface;
using SolutionWeb.Model.POLICY;
using SolutionWeb.Models;
using SolutionWeb.Model.SYS;
using SolutionWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Transactions;
using System.Web;
using System.Web.Mvc;


namespace SolutionWeb.Areas.POLICY.Controllers
{
    public class POLICY_WORKFLOW_INFOController : BaseController
    {
        // GET: POLICY/POLICY_WORKFLOW_INFO
        #region Identity
        private IBaseService bs = null;
        private IUserLogin ul = null;

        public POLICY_WORKFLOW_INFOController(IBaseService baseService, IUserLogin userLogin)
        {
            this.bs = baseService;
            this.ul = userLogin;
        }
        #endregion

        #region 流程信息
        [AjaxRequest]
        [Description("流程信息")]
        [HttpGet]
        public ActionResult Index()
        {
            string ControllerUrl = "/api/POLICY/POLICY_WORKFLOW_INFO/";
            var viewModel = new
            {
                Permission = new//权限
                {
                    a_list = this.ul.HasPermission("POLICY", "POLICY_WORKFLOW_INFO", "List", Common.HttpMethod.Post),
                    a_add = this.ul.HasPermission("POLICY", "POLICY_WORKFLOW_INFO", "Add", Common.HttpMethod.Post),
                    a_edit = this.ul.HasPermission("POLICY", "POLICY_WORKFLOW_INFO", "Edit", Common.HttpMethod.Get),
                    a_del = this.ul.HasPermission("POLICY", "POLICY_WORKFLOW_INFO", "Del", Common.HttpMethod.Get),
                },
                resx = new
                {
                    listTitle = "您没有【查看流程信息】权限",
                    addTitle = "您没有【新增流程信息】权限",
                    editTitle = "您没有【编辑流程信息】权限！",
                    deleteTitle = "您没有【删除流程信息】权限！"
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
                searchForm = new VIEW_BASE_WORKFLOW_INFO()//查询
                {
                },
                addForm = new VIEW_BASE_WORKFLOW_INFO()
                { //添加修改
                },
                extForm = new//扩展类
                {
                    extA = new List<EasyUIComBoBoxNode>(),
                    extB = new List<EasyUIComBoBoxNode>(),
                    extC = Model_BASE_STATUS_DIC.Create.GetAllStatusList(),
                    extD = Model_BASE_WORKFLOW_INFO.Create.GetMyORGNoGQTree(oc.CurrentUser.DEPT_CODE, oc.CurrentUser.PARENT_CODE),//部门列表
                    extE = new List<EasyUIComBoBoxNode>(),
                    extF = new List<EasyUIComBoBoxNode>(),
                }
            };
            return View(viewModel);
        }
        #endregion

        #region 根据所选部门信息联动考核典库
        [HttpPost]
        [AjaxRequest]
        [Skip]
        [Description("根据所选部门信息联动考核典库")]
        public ActionResult GetFlowStepInfo(VIEW_BASE_WORKFLOW_INFO workflow)
        {
            return Json(Model_BASE_WORKFLOW_INFO.Create.GetFlowStepInfo(workflow), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

    public class POLICY_WORKFLOW_INFOApiController : BaseApiController
    {
        #region Ioc
        private IBaseService bs = null;
        public POLICY_WORKFLOW_INFOApiController(IBaseService baseService)
        {
            this.bs = baseService;
        }
        #endregion

        #region 查询
        [HttpPost]//查询
        public ViewModel List(VIEW_BASE_WORKFLOW_INFO data)
        {
            int pageSize = int.Parse(data.rows);
            int pageIndex = int.Parse(data.page);
            string sort = data.sort;
            string order = data.order;

            //查询条件
            IQueryable<BASE_WORKFLOW_INFO> BASE_WORKFLOW_INFOEntity = bs.Entities<BASE_WORKFLOW_INFO>();
            if (!string.IsNullOrEmpty(data.ITEM_TYPE))
            {
                BASE_WORKFLOW_INFOEntity = BASE_WORKFLOW_INFOEntity.Where(u => u.ITEM_TYPE.Equals(data.ITEM_TYPE));
            }

            int total = 0;

            total = BASE_WORKFLOW_INFOEntity.Count();
            var listproject = BASE_WORKFLOW_INFOEntity.OrderBy(u => u.ITEM_TYPE).ThenBy(u => u.STATUS_CODE)
                                                       .Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

            return ObjToJson.ViewModelToJson(listproject, total);
        }
        #endregion

        #region 修改
        [HttpPost]
        public AjaxMsgModel Edit(VIEW_BASE_WORKFLOW_INFO data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {
                BASE_WORKFLOW_INFO info = bs.Entities<BASE_WORKFLOW_INFO>().Where(m => m.SID == data.SID).FirstOrDefault();

                if (info != null)
                {
                    
                    Mapper.CreateMap<BASE_WORKFLOW_INFO, VIEW_BASE_WORKFLOW_INFO>();
                    VIEW_BASE_WORKFLOW_INFO vm = Mapper.Map<BASE_WORKFLOW_INFO, VIEW_BASE_WORKFLOW_INFO>(info);
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
        public AjaxMsgModel Save(VIEW_BASE_WORKFLOW_INFO data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {

                Mapper.CreateMap<VIEW_BASE_WORKFLOW_INFO, BASE_WORKFLOW_INFO>();
                BASE_WORKFLOW_INFO u = Mapper.Map<VIEW_BASE_WORKFLOW_INFO, BASE_WORKFLOW_INFO>(data);

                if (string.IsNullOrEmpty(u.SID))
                {
                    u.SID = Guid.NewGuid().ToString();
                    //amm = Model_BASE_WORKFLOW_INFO.Create.Add(u);
                }
                else
                {
                    amm = Model_BASE_WORKFLOW_INFO.Create.Edit(u);
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
        public AjaxMsgModel Del(VIEW_BASE_WORKFLOW_INFO data)
        {
            return Model_BASE_WORKFLOW_INFO.Create.Del(data.SID);
        }
        #endregion
        
        private void SetStatusCode(string name1, string name2)
        {
            FieldInfo fi = typeof(Constant.POLICY_STATUS).GetField(name1, BindingFlags.Public | BindingFlags.Instance);
            
        }
    }
}