using AutoMapper;
using SolutionWeb.Common;
using SolutionWeb.Interface;
using SolutionWeb.Model.POLICY;
using SolutionWeb.Models;
using SolutionWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SolutionWeb.Areas.POLICY.Controllers
{
    public class POLICY_COMPANY_APPROVE_HISController : BaseController
    {
        #region Identity
        private IBaseService bs = null;
        private IUserLogin ul = null;

        public POLICY_COMPANY_APPROVE_HISController(IBaseService baseService, IUserLogin userLogin)
        {
            this.bs = baseService;
            this.ul = userLogin;
        }
        #endregion
        // GET: POLICY/POLICY_COMPANY_APPROVE_HIS
        public ActionResult Index()
        {
            string ControllerUrl = "/api/POLICY/POLICY_COMPANY_APPROVE_HIS/";
            var viewModel = new
            {
                Permission = new//权限
                {
                    a_list = this.ul.HasPermission("POLICY", "POLICY_COMPANY_APPROVE_HIS", "List", Common.HttpMethod.Post),
                },
                resx = new
                {
                    listTitle = "您没有【查看注册信息】权限",
                    addTitle = "您没有【新增注册信息】权限",
                    editTitle = "您没有【编辑注册信息】权限！",
                    deleteTitle = "您没有【删除注册信息】权限！"
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
                searchForm = new VIEW_CORPORATION_BASE_INFO()//查询
                {
                },
                addForm = new VIEW_CORPORATION_BASE_INFO()
                { //添加修改
                },
                extForm = new//扩展类
                {
                }
            };
            return View(viewModel);
        }
    }

    public class POLICY_COMPANY_APPROVE_HISApiController : BaseApiController
    {
        #region Ioc
        private IBaseService bs = null;
        public POLICY_COMPANY_APPROVE_HISApiController(IBaseService baseService)
        {
            this.bs = baseService;
        }
        #endregion

        #region 查询
        [HttpPost]//查询
        public ViewModel List(VIEW_CORPORATION_BASE_INFO data)
        {
            int pageSize = int.Parse(data.rows);
            int pageIndex = int.Parse(data.page);
            string sort = data.sort;
            string order = data.order;

            //查询条件
            IQueryable<CORPORATION_BASE_INFO> CORPORATION_BASE_INFOEntity = bs.Entities<CORPORATION_BASE_INFO>().Where(o => !string.IsNullOrEmpty(o.APPLY_RESULT));

            int total = 0;

            total = CORPORATION_BASE_INFOEntity.Count();
            var listproject = CORPORATION_BASE_INFOEntity.OrderByDescending(o=>o.CREATE_DT)
                .Skip(pageSize * (pageIndex - 1)).Take(pageSize).Select(o => new
                {
                    SID = o.SID,
                    CREATE_DT = o.CREATE_DT,
                    CREATE_BY = o.CREATE_BY,
                    UPDATE_DT = o.UPDATE_DT,
                    UPDATE_BY = o.UPDATE_BY,
                    CORP_NAME = o.CORP_NAME,
                    CORP_STATUS = o.CORP_STATUS,
                    SOCIAL_CREDIT_CODE = o.SOCIAL_CREDIT_CODE,
                    REGISTERED_ADDRESS = o.REGISTERED_ADDRESS,
                    LEGAL_PERSON = o.LEGAL_PERSON,
                    LEGAL_PERSON_PHONE = o.LEGAL_PERSON_PHONE,
                    OPERATOR = o.OPERATOR,
                    OPERATOR_PHONE = o.OPERATOR_PHONE,
                    OPERATOR_ID_NO = o.OPERATOR_ID_NO,
                    EMIAL = o.EMIAL,
                    REGISTERED_CAPITAL = o.REGISTERED_CAPITAL,
                    USER_NAME = o.USER_NAME,
                    PASSWORD = o.PASSWORD,
                    APPLY_RESULT= o.APPLY_RESULT
                }).ToList();


            return ObjToJson.ViewModelToJson(listproject, total);
        }
        #endregion
        

        #region 审批
        [HttpPost]
        public AjaxMsgModel Apply(VIEW_CORPORATION_BASE_INFO data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {
                var info = bs.Entities<CORPORATION_BASE_INFO>().Where(o => o.SID == data.SID).FirstOrDefault();
                if(info != null)
                {
                    info.APPLY_RESULT = data.APPLY_RESULT;
                    info.CORP_STATUS = null;
                    amm = Model_CORPORATION_BASE_INFO.Create.Edit(info);
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

        #region 获取企业注册详细信息
        [System.Web.Http.HttpPost]
        public AjaxMsgModel GetCorpInfo(VIEW_CORPORATION_BASE_INFO data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {
                CORPORATION_BASE_INFO info = bs.Entities<CORPORATION_BASE_INFO>().Where(o => o.SID == data.SID).FirstOrDefault();
                if(info != null)
                {
                    Mapper.CreateMap<CORPORATION_BASE_INFO, VIEW_CORPORATION_BASE_INFO>();
                    VIEW_CORPORATION_BASE_INFO u = Mapper.Map<CORPORATION_BASE_INFO, VIEW_CORPORATION_BASE_INFO>(info);
                    var zzjg = bs.Entities<POLICY_APPLY_FILE>().Where(o => o.MAIN_SID == u.SID && o.FILE_NAME == "组织机构证").FirstOrDefault();
                    var yyzz = bs.Entities<POLICY_APPLY_FILE>().Where(o => o.MAIN_SID == u.SID && o.FILE_NAME == "营业执照").FirstOrDefault();
                    if (zzjg != null) u.zzjg = zzjg.FILE_PATH;
                    if (yyzz != null) u.yyzz = yyzz.FILE_PATH;
                    amm.Statu = AjaxStatu.ok;
                    amm.Data = u;
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
    }

}