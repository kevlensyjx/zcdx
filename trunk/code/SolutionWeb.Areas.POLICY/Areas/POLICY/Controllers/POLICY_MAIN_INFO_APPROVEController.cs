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
    public class POLICY_MAIN_INFO_APPROVEController : BaseController
    {
        // GET: POLICY/POLICY_MAIN_INFO_APPROVE

        #region Identity
        private IBaseService bs = null;
        private IUserLogin ul = null;

        public POLICY_MAIN_INFO_APPROVEController(IBaseService baseService, IUserLogin userLogin)
        {
            this.bs = baseService;
            this.ul = userLogin;
        }
        #endregion
        [AjaxRequest]
        [Description("项目信息")]
        [HttpGet]
        public ActionResult Index(string id)
        {
            BASE_STATUS_DIC statusModel = Model_BASE_STATUS_DIC.Create.GetStatusInfoByCode(id);
            string ControllerUrl = "/api/POLICY/POLICY_MAIN_INFO_APPROVE/";
            var viewModel = new
            {
                Permission = new//权限
                {
                    a_list = this.ul.HasPermission("POLICY", "POLICY_MAIN_INFO_APPROVE", "List", Common.HttpMethod.Post),
                    //a_add = this.ul.HasPermission("POLICY", "POLICY_MAIN_INFO_APPROVE", "Add", Common.HttpMethod.Post),
                    //a_edit = this.ul.HasPermission("POLICY", "POLICY_MAIN_INFO_APPROVE", "Edit", Common.HttpMethod.Get),
                    //a_del = this.ul.HasPermission("POLICY", "POLICY_MAIN_INFO_APPROVE", "Del", Common.HttpMethod.Get),
                },
                resx = new
                {
                    listTitle = "您没有【查看" + statusModel.S_NAME + "信息】权限",
                    //addTitle = "您没有【新增" + statusModel.S_NAME + "信息】权限",
                    //editTitle = "您没有【编辑" + statusModel.S_NAME + "信息】权限！",
                    //deleteTitle = "您没有【删除" + statusModel.S_NAME + "信息】权限！"
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
                searchForm = new VIEW_POLICY_STATUS_CHANGE()//查询
                {
                    STATUS_CODE = id
                },
                addForm = new VIEW_POLICY_STATUS_CHANGE()
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

        [AjaxRequest]
        [HttpGet]
        [Description("查看申请状态")]
        [Skip]
        public ActionResult Apply(string id)
        {
            VIEW_POLICY_MAIN_INFO_APPROVE resultData = Model_POLICY_MAIN_INFO.Create.GetApprovelInfo(id);
            return ObjToJson.GetToJson(resultData, true);
        }

        [AjaxRequest]
        [HttpGet]
        [Description("查看企业申请状态")]
        [Skip]
        public ActionResult ApplyCorpInfo(string id)
        {
            AjaxMsgModel resultData = Model_POLICY_MAIN_INFO.Create.GetApplyCorpInfo(id);
            return ObjToJson.GetToJson(resultData, true);
        }

        [AjaxRequest]
        [Skip]
        public ActionResult DownLoadImage(string id)
        {
            string fileName = bs.Entities<POLICY_APPLY_FILE>().FirstOrDefault(m => m.SID == id).FILE_PATH;
            var path = Server.MapPath(fileName);
            return File(path, "iamge/jpeg", fileName.Substring(fileName.LastIndexOf('/') + 1));
        }


    }
    public class POLICY_MAIN_INFO_APPROVEApiController : BaseApiController
    {
        #region Ioc
        private IBaseService bs = null;
        public POLICY_MAIN_INFO_APPROVEApiController(IBaseService baseService)
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

            IQueryable<POLICY_STATUS_CHANGE> statusEntites =
                bs.Entities<POLICY_STATUS_CHANGE>()
                    .Where(
                        o =>
                            o.DEPT_CODE.Equals(oc.CurrentUser.DEPT_CODE) && o.IS_HANDLE == "0");

            IQueryable<POLICY_MAIN_INFO> main_infoEntity = bs.Entities<POLICY_MAIN_INFO>();//.Where(o=>o.STATUS_CODE == data.STATUS_CODE);

            //if (!string.IsNullOrEmpty(data.IS_HANDLE))
            //{
            //    statusEntites = statusEntites.Where(o => o.IS_HANDLE == data.IS_HANDLE);
            //}
            if (!string.IsNullOrEmpty(data.STATUS_CODE))
            {
                statusEntites = statusEntites.Where(o => o.STATUS_CODE == data.STATUS_CODE);
            }

            var statusLookup = statusEntites.ToLookup(o => o.POLICY_SID);

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

            //List<EasyUIComBoBoxNode> allstatusList = Model_BASE_STATUS_DIC.Create.GetAllStatusList();

            List<POLICY_STATUS_CHANGE> statusChangeList = new List<POLICY_STATUS_CHANGE>();
            foreach (var item in statusLookup)
            {
                POLICY_STATUS_CHANGE sModel = item.OrderByDescending(o => o.CREATE_DT).FirstOrDefault();
                statusChangeList.Add(sModel);
            }


            int total = 0;

            total = statusChangeList.Count();
            var listproject = statusChangeList.OrderByDescending(u => u.CREATE_DT).ThenByDescending(u => u.ITEM_TYPE)
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
                        STATUS_CODE = sModel.STATUS_CODE,
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
                            Model_BASE_STATUS_DIC.Create.GetCalculateStatusTime((DateTime)sModel.CREATE_DT,
                                (DateTime)sModel.HANDLE_DT),
                        IS_HANDLE = sModel.IS_HANDLE,
                        EMIAL = mModel.EMIAL,
                        REGISTERED_CAPITAL = mModel.REGISTERED_CAPITAL,
                        APPLY_ITEM_TYPE = mModel.APPLY_ITEM_TYPE,
                        APPLY_ITEM_NAME = mModel.APPLY_ITEM_NAME,
                        APPLY_MONEY_WORDS = mModel.APPLY_MONEY_WORDS,
                        APPLY_MONEY_NUMBER = mModel.APPLY_MONEY_NUMBER,
                        TIME_LIMIT = sModel.TIME_LIMIT,
                        CREATE_DT = sModel.CREATE_DT,

                        COMPANY_NAME = mModel.COMPANY_NAME,
                        COMPANY_ADDRESS =mModel.COMPANY_ADDRESS,
                        COMPANY_PHONE = mModel.COMPANY_PHONE,
                        VAT_NO=mModel.VAT_NO,
                        BANK_NAME=mModel.BANK_NAME,
                        BANK_ACOUNT = mModel.BANK_ACOUNT,

                        REJECT_REASON = mModel.REJECT_REASON,
                    });
            }
            return ObjToJson.ViewModelToJson(resultList, total);
        }
        #endregion

        #region 保存
        [HttpPost]
        public AjaxMsgModel Save(VIEW_POLICY_STATUS_CHANGE data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {
                if (string.IsNullOrEmpty(data.SID)||string.IsNullOrEmpty(data.STATUS_CODE))
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = "非法操作";
                    return amm;
                }
                if (string.IsNullOrEmpty(data.HANDLE_CONTENT) || string.IsNullOrEmpty(data.HANDLE_RESULT) ||data.HANDLE_RESULT == "undefined")
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = "审批意见或审批结果不能为空";
                    return amm;
                }
               

                 
                amm = Model_POLICY_STATUS_CHANGE.Create.Edit(data);
                
                return amm;
            }
            catch (Exception ex)
            {
                RecordLog.RecordError(ex.ToString());
                return amm;
            }
        }
        #endregion

        #region 保存
        [System.Web.Http.HttpPost]
        public AjaxMsgModel SaveInfo(VIEW_POLICY_STATUS_CHANGE data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {
                if (string.IsNullOrEmpty(data.SID) || string.IsNullOrEmpty(data.STATUS_CODE))
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = "非法操作";
                    return amm;
                }
                if (string.IsNullOrEmpty(data.HANDLE_CONTENT) || string.IsNullOrEmpty(data.HANDLE_RESULT) || data.HANDLE_RESULT == "undefined")
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = "审批意见或审批结果不能为空";
                    return amm;
                }



                amm = Model_POLICY_STATUS_CHANGE.Create.Edit(data);

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