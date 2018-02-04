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
    public class POLICY_PROJECT_INFOController : BaseController
    {
        // GET: POLICY/POLICY_PROJECT_INFO
        #region Identity
        private IBaseService bs = null;
        private IUserLogin ul = null;

        public POLICY_PROJECT_INFOController(IBaseService baseService, IUserLogin userLogin)
        {
            this.bs = baseService;
            this.ul = userLogin;
        }
        #endregion
        #region 项目信息
        [AjaxRequest]
        [Description("项目信息")]
        [HttpGet]
        public ActionResult Index()
        {
            string ControllerUrl = "/api/POLICY/POLICY_PROJECT_INFO/";
            var viewModel = new
            {
                Permission = new//权限
                {
                    a_list = this.ul.HasPermission("POLICY", "POLICY_PROJECT_INFO", "List", Common.HttpMethod.Post),
                    a_add = this.ul.HasPermission("POLICY", "POLICY_PROJECT_INFO", "Add", Common.HttpMethod.Post),
                    a_edit = this.ul.HasPermission("POLICY", "POLICY_PROJECT_INFO", "Edit", Common.HttpMethod.Get),
                    a_del = this.ul.HasPermission("POLICY", "POLICY_PROJECT_INFO", "Del", Common.HttpMethod.Get),
                },
                resx = new
                {
                    listTitle = "您没有【查看项目信息】权限",
                    addTitle = "您没有【新增项目信息】权限",
                    editTitle = "您没有【编辑项目信息】权限！",
                    deleteTitle = "您没有【删除项目信息】权限！"
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
                searchForm = new VIEW_BASE_PROJECT_INFO()//查询
                {
                },
                addForm = new VIEW_BASE_PROJECT_INFO()
                { //添加修改
                },
                extForm = new//扩展类
                {
                    extA = new List<EasyUIComBoBoxNode>(),
                    extB = new List<EasyUIComBoBoxNode>()
                }
            };
            return View(viewModel);
        }
        #endregion
    }

    public class POLICY_PROJECT_INFOApiController : BaseApiController
    {
        #region Ioc
        private IBaseService bs = null;
        public POLICY_PROJECT_INFOApiController(IBaseService baseService)
        {
            this.bs = baseService;
        }
        #endregion

        #region 查询
        [HttpPost]//查询
        public ViewModel List(VIEW_BASE_PROJECT_INFO data)
        {
            int pageSize = int.Parse(data.rows);
            int pageIndex = int.Parse(data.page);
            string sort = data.sort;
            string order = data.order;

            //查询条件
            IQueryable<BASE_PROJECT_INFO> BASE_PROJECT_INFOEntity = bs.Entities<BASE_PROJECT_INFO>();
            if (!string.IsNullOrEmpty(data.ITEM_TYPE))
            {
                BASE_PROJECT_INFOEntity = BASE_PROJECT_INFOEntity.Where(u => u.ITEM_TYPE.Equals(data.ITEM_TYPE));
            }
            
            int total = 0;

            total = BASE_PROJECT_INFOEntity.Count();
            var listproject = BASE_PROJECT_INFOEntity.OrderBy(u => u.ITEM_TYPE).ThenBy(u => u.LAY_ORDER)
                .Skip(pageSize*(pageIndex - 1)).Take(pageSize).Select(o=> new
                {
                    SID = o.SID,
                    ITEM_TYPE = o.ITEM_TYPE,
                    ITEM_NAME = o.ITEM_NAME,
                    CASHING_WAY = o.CASHING_WAY,
                    ITEM_CODE = o.ITEM_CODE
                }).ToList();


            return ObjToJson.ViewModelToJson(listproject, total);
        }
        #endregion

        #region 修改
        [HttpPost]
        public AjaxMsgModel Edit(VIEW_BASE_PROJECT_INFO data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {
                BASE_PROJECT_INFO info = bs.Entities<BASE_PROJECT_INFO>().Where(m => m.SID == data.SID).FirstOrDefault();

                if (info != null)
                {
                    Mapper.CreateMap<BASE_PROJECT_INFO, VIEW_BASE_PROJECT_INFO>();
                    VIEW_BASE_PROJECT_INFO vm = Mapper.Map<BASE_PROJECT_INFO, VIEW_BASE_PROJECT_INFO>(info);
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
        public AjaxMsgModel Save(VIEW_BASE_PROJECT_INFO data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {
                Mapper.CreateMap<VIEW_BASE_PROJECT_INFO, BASE_PROJECT_INFO>();
                BASE_PROJECT_INFO u = Mapper.Map<VIEW_BASE_PROJECT_INFO, BASE_PROJECT_INFO>(data);

                if (string.IsNullOrEmpty(u.SID))
                {
                    var last = bs.Entities<BASE_PROJECT_INFO>().Where(o => o.ITEM_TYPE == data.ITEM_TYPE).OrderByDescending(o => o.LAY_ORDER).FirstOrDefault();
                    if(last != null)
                    {
                        u.LAY_ORDER = last.LAY_ORDER.Value + 1;
                        string head = string.Empty;
                        switch (u.ITEM_TYPE)
                        {
                            case "评审类":
                                head = "PS";
                                break;
                            case "普惠类":
                                head = "PH";
                                break;
                            case "合同类":
                                head = "HT";
                                break;
                        }
                        u.ITEM_CODE = head + u.LAY_ORDER.Value.ToString().PadLeft(2, '0');
                    }
                    u.SID = Guid.NewGuid().ToString();
                    amm = Model_BASE_PROJECT_INFO.Create.Add(u);
                }
                else
                {
                    amm = Model_BASE_PROJECT_INFO.Create.Edit(u);
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
        public AjaxMsgModel Del(VIEW_BASE_PROJECT_INFO data)
        {
            return Model_BASE_PROJECT_INFO.Create.Del(data.SID);
        }
        #endregion
    }
}