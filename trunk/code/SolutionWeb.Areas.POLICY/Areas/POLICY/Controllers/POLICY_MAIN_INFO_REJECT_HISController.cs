﻿using AutoMapper;
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
    public class POLICY_MAIN_INFO_REJECT_HISController : BaseController
    {
        // GET: POLICY/POLICY_MAIN_INFO_REJECT_HIS
        #region Identity
        private IBaseService bs = null;
        private IUserLogin ul = null;

        public POLICY_MAIN_INFO_REJECT_HISController(IBaseService baseService, IUserLogin userLogin)
        {
            this.bs = baseService;
            this.ul = userLogin;
        }
        #endregion
        [AjaxRequest]
        [Description("项目信息")]
        [HttpGet]
        public ActionResult Index()
        {
         
            string ControllerUrl = "/api/POLICY/POLICY_MAIN_INFO_REJECT_HIS/";
            var viewModel = new
            {
                Permission = new//权限
                {
                    a_list = this.ul.HasPermission("POLICY", "POLICY_MAIN_INFO_REJECT_HIS", "List", Common.HttpMethod.Post),
                },
                resx = new
                {
                    listTitle = "您没有【查看驳回信息】权限",
                },
                urls = new//请求URL
                {
                    save = ControllerUrl + "Save",
                    list = ControllerUrl + "List",
                    edit = ControllerUrl + "Edit",
                    del = ControllerUrl + "Del",
                    
                    dataGgridDelId = "SID",//多选删除的唯一键名
                    dataGgridName = "data_grid",//列表ID
                    dataGgridType = "datagrid",//列表类型
                    dataAddName = "data_add",//增加窗口

                    dataFormName = "DataForm",//提交表单
                },
                searchForm = new VIEW_POLICY_STATUS_CHANGE()//查询
                {
                     
                },
                addForm = new VIEW_POLICY_NOTICE_INFO()
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
    }

    public class POLICY_MAIN_INFO_REJECT_HISApiController : BaseApiController
    {
        #region Ioc
        private IBaseService bs = null;
        public POLICY_MAIN_INFO_REJECT_HISApiController(IBaseService baseService)
        {
            this.bs = baseService;
        }
        #endregion

        #region 查询
        [HttpPost]//查询
        public ViewModel List(VIEW_POLICY_STATUS_CHANGE data)
        {
            int pageSize = int.Parse(data.rows);
            int pageIndex = int.Parse(data.page);

            //本部门所管辖的被驳回的信息
            IQueryable<POLICY_STATUS_CHANGE> statusEntites =
                bs.Entities<POLICY_STATUS_CHANGE>()
                    .Where(
                        o =>
                            o.DEPT_CODE.Equals(oc.CurrentUser.DEPT_CODE) && o.IS_HANDLE == "1" && o.STATUS_CODE == Constant.POLICY_STATUS.驳回处理);

            IQueryable<POLICY_MAIN_INFO> main_infoEntity = bs.Entities<POLICY_MAIN_INFO>();//.Where(o=>o.STATUS_CODE == data.STATUS_CODE);


            List<string> policysidList = statusEntites.Select(o => o.POLICY_SID).Distinct().ToList();

            //查询条件
            main_infoEntity =
                main_infoEntity.Where(o => policysidList.Contains(o.SID));

            #region 查询条件

            if (data.START_TIME.HasValue)
            {
                main_infoEntity = main_infoEntity.Where(o => o.APPLY_DT >= data.START_TIME);
            }
            if (data.END_TIME.HasValue)
            {
                DateTime dt_end = Convert.ToDateTime(data.END_TIME).AddDays(1);
                main_infoEntity = main_infoEntity.Where(o => o.APPLY_DT <= dt_end);
            }
            #endregion

            List<POLICY_MAIN_INFO> maininfoList = main_infoEntity.ToList();


            List<POLICY_STATUS_CHANGE> statusChangeList = statusEntites.ToList();

            int total = 0;

            total = statusChangeList.Count();
            var listproject = statusChangeList.OrderBy(u => u.ITEM_TYPE).ThenByDescending(u => u.CREATE_DT)
              .Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            List<VIEW_POLICY_STATUS_CHANGE> resultList = new List<VIEW_POLICY_STATUS_CHANGE>();

            foreach (var sModel in listproject)
            {
                POLICY_MAIN_INFO mModel = maininfoList.Where(o => o.SID == sModel.POLICY_SID).FirstOrDefault();
                if (!sModel.HANDLE_DT.HasValue)
                    sModel.HANDLE_DT = DateTime.MinValue;
                if (mModel != null)
                    resultList.Add(new VIEW_POLICY_STATUS_CHANGE
                    {
                        SID = sModel.SID,
                        POLICY_SID = sModel.POLICY_SID,
                        DEPT_CODE = sModel.DEPT_CODE,
                        STATUS_NAME = sModel.STATUS_NAME,
                        HANDLE_DT = sModel.HANDLE_DT,
                        HANDLE_CONTENT = sModel.HANDLE_CONTENT,
                        HANDLE_SID = sModel.HANDLE_SID,
                        APPLY_NUMBER = mModel.APPLY_NUMBER,
                        MAIN_INFO_STATUS_NAME = mModel.STATUS_NAME,
                        MAIN_INFO_STATUS_CODE = mModel.STATUS_CODE,
                        CORPORATION_SID = mModel.CORPORATION_SID,
                        CORP_NAME = mModel.CORP_NAME,
                        SOCIAL_CREDIT_CODE = mModel.SOCIAL_CREDIT_CODE,
                        REGISTERED_ADDRESS = mModel.REGISTERED_ADDRESS,
                        LEGAL_PERSON = mModel.LEGAL_PERSON,
                        LEGAL_PERSON_PHONE = mModel.LEGAL_PERSON_PHONE,
                        OPERATOR = mModel.OPERATOR,
                        OPERATOR_PHONE = mModel.OPERATOR_PHONE,
                        OPERATOR_ID_NO = mModel.OPERATOR_ID_NO,
                        APPLY_DT = mModel.APPLY_DT,
                        STATUS_TIME =
                            Model_BASE_STATUS_DIC.Create.GetCalculateStatusTime((DateTime) sModel.CREATE_DT,
                                (DateTime) sModel.HANDLE_DT),
                        IS_HANDLE = sModel.IS_HANDLE,
                        EMIAL = mModel.EMIAL,
                        REGISTERED_CAPITAL = mModel.REGISTERED_CAPITAL,
                        APPLY_ITEM_TYPE = mModel.APPLY_ITEM_TYPE,
                        APPLY_ITEM_NAME = mModel.APPLY_ITEM_NAME,
                        APPLY_MONEY_WORDS = mModel.APPLY_MONEY_WORDS,
                        APPLY_MONEY_NUMBER = mModel.APPLY_MONEY_NUMBER,
                        TIME_LIMIT = sModel.TIME_LIMIT,
                        POLICITY_STATUS = mModel.POLICITY_STATUS,
                        POLICITY_BEGIN_DT = Convert.ToDateTime(mModel.POLICITY_BEGIN_DT).ToString("yyyy-MM-dd"),
                        POLICITY_END_DT = Convert.ToDateTime(mModel.POLICITY_END_DT).ToString("yyyy-MM-dd"),
                        POLICITY_STOP_REASON = mModel.POLICITY_STOP_REASON,
                        REJECT_REASON = mModel.REJECT_REASON,
                        CREATE_DT = sModel.CREATE_DT
                    });
            }
            return ObjToJson.ViewModelToJson(resultList, total);
        }
        #endregion

       
    }
}