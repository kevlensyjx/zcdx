
using SolutionWeb.Common;
using SolutionWeb.Model.POLICY;
using SolutionWeb.Model.SYS;
using SolutionWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

namespace SolutionWeb.Models
{
    public partial class Model_POLICY_STATUS_CHANGE
    {
        public  AjaxMsgModel Edit(VIEW_POLICY_STATUS_CHANGE model)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            amm.Statu = AjaxStatu.err;
            amm.Msg = "录入失败！";
            try
            {
                POLICY_STATUS_CHANGE currentstatusModel =
                    bs.Entities<POLICY_STATUS_CHANGE>().FirstOrDefault(o => o.SID == model.SID && o.DEPT_CODE== oc.CurrentUser.DEPT_CODE);

                if (currentstatusModel == null)
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = "非法数据";
                    return amm;
                }

                #region 当前状态信息修改
                currentstatusModel.HANDLE_RESULT = model.HANDLE_RESULT;
                currentstatusModel.HANDLE_CONTENT = model.HANDLE_CONTENT;
                currentstatusModel.HANDLE_SID = oc.CurrentUser.USER_NAME;
                currentstatusModel.HANDLE_NAME = oc.CurrentUser.ZSNAME;
                currentstatusModel.IS_HANDLE = "1";
                currentstatusModel.HANDLE_DT = DateTime.Now;
                currentstatusModel.SEAL_SID = model.SEAL_SID;
                #endregion

                #region 申请信息表中状态修改

                POLICY_MAIN_INFO mainInfoModel =
                    bs.Entities<POLICY_MAIN_INFO>().FirstOrDefault(o => o.SID == currentstatusModel.POLICY_SID);
                #endregion

                //短信推送接口暂时没有添加
                //状态变化列表
                List<POLICY_STATUS_CHANGE> statuschangeList = new List<POLICY_STATUS_CHANGE>();
                List<POLICY_STATUS_CHANGE> statuschangeupdateList = new List<POLICY_STATUS_CHANGE>();

                POLICY_STATUS_CHANGE nextstatusModel = new POLICY_STATUS_CHANGE();


                var nextstatusdic =
                    bs.Entities<BASE_WORKFLOW_INFO>()
                        .FirstOrDefault(
                            o =>
                                o.ITEM_TYPE == model.APPLY_ITEM_TYPE && o.STATUS_CODE == model.STATUS_CODE);

                if (nextstatusdic != null)
                {

                    SYS_DEPT deptModel = new SYS_DEPT();
                    //如果是业务核定环境或者部门会审环节，需要查找配置好的部门信息
                    bool hs_flag = false;
                    #region 部门会审 特殊处理
                    if (currentstatusModel.STATUS_CODE == Constant.POLICY_STATUS.部门会审)
                    {
                        #region 评审类
                        if (currentstatusModel.ITEM_TYPE == "评审类")
                        {
                            //需要计算会审成员是否会审完毕，所有成员都填写过会审意见，才能走下一步
                            List<POLICY_STATUS_CHANGE> hsstatusList = bs.Entities<POLICY_STATUS_CHANGE>()
                                .Where(o =>
                                    o.POLICY_SID == currentstatusModel.POLICY_SID &&
                                    o.STATUS_CODE == Constant.POLICY_STATUS.部门会审 &&
                                    o.IS_HANDLE == "0"
                                ).ToList();
                            //未处理的记录数是1的时候，说明是最后一个会审审批人
                            if (hsstatusList.Count == 1)
                            {
                                hs_flag = false;
                            }
                            else
                                hs_flag = true;
                        }
                        #endregion
                        #region 普惠类
                        /*
                         * 一票否决直接驳回到受理窗口，其他不管是否通过，都往下走。
                         * 其中国税包含（高新区和经济区，这两个肯定有一个否决，两个都否决才算否决）。
                         * 一票否决部门包括：国税、地税、工商、安监、环保
                         * 
                         */
                        if (currentstatusModel.ITEM_TYPE == "普惠类")
                        {
                            List<POLICY_STATUS_CHANGE> hsstatusList = bs.Entities<POLICY_STATUS_CHANGE>()
                               .Where(o =>
                                   o.POLICY_SID == currentstatusModel.POLICY_SID &&
                                   o.STATUS_CODE == Constant.POLICY_STATUS.部门会审 
                               ).ToList();
                            //未处理的记录数是1的时候，说明是最后一个会审审批人
                            if (hsstatusList.Where(o => o.IS_HANDLE == "0").ToList().Count == 1 && currentstatusModel.IS_VOTE == "0")
                            {
                                hs_flag = false;
                            }
                            else
                            {
                                hs_flag = true;
                                if (currentstatusModel.HANDLE_RESULT == "0" && currentstatusModel.IS_VOTE == "1")
                                {
                                    var hs_gxqgs_model = hsstatusList.FirstOrDefault(o => o.DEPT_CODE == "011601");//高新区国税局
                                    var hs_kfqgs_model = hsstatusList.FirstOrDefault(o => o.DEPT_CODE == "011602");//开发区国税局
                                    if (hs_gxqgs_model != null && hs_kfqgs_model != null)
                                    {
                                        //如果是高新区国税局，则需判断开发区国税局
                                        if (currentstatusModel.DEPT_CODE == "011601")
                                        {
                                            if (hs_kfqgs_model.IS_HANDLE == "1" && hs_kfqgs_model.HANDLE_RESULT == "0")
                                            {
                                                hs_flag = false;
                                                nextstatusdic.NEXT_STATUS_CODE = Constant.POLICY_STATUS.驳回处理;
                                            }
                                        }
                                        else if (currentstatusModel.DEPT_CODE == "011602")
                                        {
                                            if (hs_gxqgs_model.IS_HANDLE == "1" && hs_gxqgs_model.HANDLE_RESULT == "0")
                                            {
                                                hs_flag = false;
                                                nextstatusdic.NEXT_STATUS_CODE = Constant.POLICY_STATUS.驳回处理;
                                                nextstatusdic.NEXT_STATUS_NAME = "驳回处理";
                                            }
                                        }
                                        else
                                        {
                                            hs_flag = false;
                                            nextstatusdic.NEXT_STATUS_CODE = Constant.POLICY_STATUS.驳回处理;
                                            nextstatusdic.NEXT_STATUS_NAME = "驳回处理";
                                        }
                                    }
                                    else
                                    {
                                        {
                                            hs_flag = false;
                                            nextstatusdic.NEXT_STATUS_CODE = Constant.POLICY_STATUS.驳回处理;
                                            nextstatusdic.NEXT_STATUS_NAME = "驳回处理";
                                        }
                                    }
                                    //满足一票否决，则处理未处理的状态记录为，已处理
                                    if (!hs_flag)
                                    {
                                        var nohandleList = hsstatusList.Where(o => o.IS_HANDLE == "0" && o.SID != currentstatusModel.SID).ToList();
                                        foreach (var item in nohandleList)
                                        {
                                            item.IS_HANDLE = "1";
                                            item.HANDLE_CONTENT = "已被:" + currentstatusModel.DEPT_NAME + " 一票否决，您无需审批";
                                            item.HANDLE_DT = DateTime.Now;
                                            statuschangeupdateList.Add(item);
                                        }
                                    }
                                }
                                else if (currentstatusModel.HANDLE_RESULT == "1" && hsstatusList.Where(o => o.IS_HANDLE == "0").ToList().Count == 1)
                                {
                                    hs_flag = false;
                                }
                            }
                        }

                        #endregion
                    }
                    #endregion

                    if (!hs_flag)
                    {
                        mainInfoModel.STATUS_CODE = nextstatusdic.NEXT_STATUS_CODE;
                        mainInfoModel.STATUS_NAME = nextstatusdic.NEXT_STATUS_NAME;

                        if (model.HANDLE_RESULT == "1")
                        { mainInfoModel.APPLY_STATUS = "通过"; }
                        else
                        { mainInfoModel.APPLY_STATUS = "驳回"; }

                        #region 如果是窗口驳回的，才往主表里面存放驳回原因
                        if (currentstatusModel.STATUS_CODE == Constant.POLICY_STATUS.窗口受理)
                        {
                            if (model.HANDLE_RESULT == "0")
                                mainInfoModel.REJECT_REASON = (mainInfoModel.REJECT_REASON == null)
                                    ? ""
                                    : mainInfoModel.REJECT_REASON + ";" + model.HANDLE_CONTENT;
                            else
                                mainInfoModel.REJECT_REASON = mainInfoModel.REJECT_REASON;
                        }

                        #endregion


                        if (nextstatusdic.NEXT_STATUS_CODE == Constant.POLICY_STATUS.业务核定)
                        {
                            #region 业务核定

                            BASE_PROJECT_INFO projectinfoModel =
                                bs.Entities<BASE_PROJECT_INFO>()
                                    .FirstOrDefault(
                                        o => o.ITEM_NAME == model.APPLY_ITEM_NAME && o.ITEM_TYPE == model.APPLY_ITEM_TYPE);
                            if (projectinfoModel != null)
                            {
                                BASE_PROJECT_CONFIG configinfoModel =
                                    projectinfoModel.BASE_PROJECT_CONFIG.FirstOrDefault(o => o.DEPT_TYPE == "牵头部门");
                                if (configinfoModel != null)
                                {
                                    nextstatusModel = new POLICY_STATUS_CHANGE();
                                    nextstatusModel.SID = Guid.NewGuid().ToString();
                                    nextstatusModel.POLICY_SID = currentstatusModel.POLICY_SID;
                                    nextstatusModel.ITEM_TYPE = currentstatusModel.ITEM_TYPE;
                                    nextstatusModel.STATUS_CODE = nextstatusdic.NEXT_STATUS_CODE;
                                    nextstatusModel.STATUS_NAME = nextstatusdic.NEXT_STATUS_NAME;
                                    nextstatusModel.CREATE_DT = DateTime.Now;
                                    nextstatusModel.IS_HANDLE = "0";
                                    nextstatusModel.IS_CALCULATE = "0";
                                    nextstatusModel.TIME_LIMIT = nextstatusdic.TIME_LIMIT;
                                    nextstatusModel.DEPT_CODE = configinfoModel.DEPT_CODE;
                                    nextstatusModel.DEPT_NAME = configinfoModel.DEPT_NAME;
                                    statuschangeList.Add(nextstatusModel);
                                }
                                else
                                {
                                    amm.Statu = AjaxStatu.err;
                                    amm.Msg = "审批失败！无下一流程配置信息";
                                    return amm;
                                }
                            }
                            else
                            {
                                amm.Statu = AjaxStatu.err;
                                amm.Msg = "审批失败！无下一流程配置信息";
                                return amm;
                            }

                            #endregion
                        }
                        else if (nextstatusdic.NEXT_STATUS_CODE == Constant.POLICY_STATUS.部门会审)
                        {
                            #region 部门会审

                            BASE_PROJECT_INFO projectinfoModel =
                                bs.Entities<BASE_PROJECT_INFO>()
                                    .FirstOrDefault(
                                        o => o.ITEM_NAME == model.APPLY_ITEM_NAME && o.ITEM_TYPE == model.APPLY_ITEM_TYPE);
                            if (projectinfoModel != null)
                            {
                                List<BASE_PROJECT_CONFIG> configinfoList =
                                    projectinfoModel.BASE_PROJECT_CONFIG.Where(o => o.DEPT_TYPE == "会审部门").ToList();
                                foreach (var configinfoModel in configinfoList)
                                {
                                    nextstatusModel = new POLICY_STATUS_CHANGE();
                                    nextstatusModel.SID = Guid.NewGuid().ToString();
                                    nextstatusModel.POLICY_SID = currentstatusModel.POLICY_SID;
                                    nextstatusModel.ITEM_TYPE = currentstatusModel.ITEM_TYPE;
                                    nextstatusModel.STATUS_CODE = nextstatusdic.NEXT_STATUS_CODE;
                                    nextstatusModel.STATUS_NAME = nextstatusdic.NEXT_STATUS_NAME;
                                    nextstatusModel.CREATE_DT = DateTime.Now;
                                    nextstatusModel.IS_HANDLE = "0";
                                    nextstatusModel.IS_CALCULATE = "0";
                                    nextstatusModel.TIME_LIMIT = nextstatusdic.TIME_LIMIT;
                                    nextstatusModel.DEPT_CODE = configinfoModel.DEPT_CODE;
                                    nextstatusModel.DEPT_NAME = configinfoModel.DEPT_NAME;
                                    nextstatusModel.IS_VOTE = configinfoModel.IS_VOTE;
                                    statuschangeList.Add(nextstatusModel);
                                }
                            }
                            else
                            {
                                amm.Statu = AjaxStatu.err;
                                amm.Msg = "审批失败！无下一流程配置信息";
                                return amm;
                            }

                            #endregion
                        }
                        else if (nextstatusdic.NEXT_STATUS_CODE == Constant.POLICY_STATUS.分管领导审批)
                        {
                            //TODO:分管领导审批 处理对象是 分管领导 011502
                            #region 分管领导审批
                            nextstatusModel = new POLICY_STATUS_CHANGE();
                            nextstatusModel.SID = Guid.NewGuid().ToString();
                            nextstatusModel.POLICY_SID = currentstatusModel.POLICY_SID;
                            nextstatusModel.ITEM_TYPE = currentstatusModel.ITEM_TYPE;
                            nextstatusModel.STATUS_CODE = nextstatusdic.NEXT_STATUS_CODE;
                            nextstatusModel.STATUS_NAME = nextstatusdic.NEXT_STATUS_NAME;
                            nextstatusModel.CREATE_DT = DateTime.Now;
                            nextstatusModel.IS_HANDLE = "0";
                            nextstatusModel.IS_CALCULATE = "0";
                            nextstatusModel.TIME_LIMIT = nextstatusdic.TIME_LIMIT;
                            nextstatusModel.DEPT_CODE = "011502";
                            nextstatusModel.DEPT_NAME = "分管领导";
                            statuschangeList.Add(nextstatusModel);
                            #endregion
                        }
                        else if (nextstatusdic.NEXT_STATUS_CODE == Constant.POLICY_STATUS.事项公示 ||
                            nextstatusdic.NEXT_STATUS_CODE == Constant.POLICY_STATUS.窗口受理 ||
                            nextstatusdic.NEXT_STATUS_CODE == Constant.POLICY_STATUS.项目评价 ||
                            nextstatusdic.NEXT_STATUS_CODE == Constant.POLICY_STATUS.驳回处理)
                        {
                            //TODO:事项公示 处理对象是 受理窗口 011502
                            #region 事项公示
                            nextstatusModel = new POLICY_STATUS_CHANGE();
                            nextstatusModel.SID = Guid.NewGuid().ToString();
                            nextstatusModel.POLICY_SID = currentstatusModel.POLICY_SID;
                            nextstatusModel.ITEM_TYPE = currentstatusModel.ITEM_TYPE;
                            nextstatusModel.STATUS_CODE = nextstatusdic.NEXT_STATUS_CODE;
                            nextstatusModel.STATUS_NAME = nextstatusdic.NEXT_STATUS_NAME;
                            nextstatusModel.CREATE_DT = DateTime.Now;
                            nextstatusModel.IS_HANDLE = "0";
                            nextstatusModel.IS_CALCULATE = "0";
                            nextstatusModel.TIME_LIMIT = nextstatusdic.TIME_LIMIT;
                            nextstatusModel.DEPT_CODE = "011501";
                            nextstatusModel.DEPT_NAME = "受理窗口";
                            statuschangeList.Add(nextstatusModel);
                            #endregion
                        }
                        else if (nextstatusdic.NEXT_STATUS_CODE == Constant.POLICY_STATUS.企业请拨)
                        {
                            //TODO:企业请拨 处理对象是 企业经办人处理 0000000
                            #region 事项公示
                            //nextstatusModel = new POLICY_STATUS_CHANGE();
                            //nextstatusModel.SID = Guid.NewGuid().ToString();
                            //nextstatusModel.POLICY_SID = currentstatusModel.POLICY_SID;
                            //nextstatusModel.ITEM_TYPE = currentstatusModel.ITEM_TYPE;
                            //nextstatusModel.STATUS_CODE = nextstatusdic.NEXT_STATUS_CODE;
                            //nextstatusModel.STATUS_NAME = nextstatusdic.NEXT_STATUS_NAME;
                            //nextstatusModel.CREATE_DT = DateTime.Now;
                            //nextstatusModel.IS_HANDLE = "0";
                            //nextstatusModel.IS_CALCULATE = "0";
                            //nextstatusModel.TIME_LIMIT = nextstatusdic.TIME_LIMIT;
                            //nextstatusModel.DEPT_CODE = "0000-0000";
                            //nextstatusModel.DEPT_NAME = "企业经办人处理";
                            //statuschangeList.Add(nextstatusModel);
                            #endregion
                        }
                        else if (nextstatusdic.NEXT_STATUS_CODE == Constant.POLICY_STATUS.企业确认)
                        {
                            //TODO:企业请拨 处理对象是 企业经办人处理 0000000
                            #region 企业确认
                            nextstatusModel = new POLICY_STATUS_CHANGE();
                            nextstatusModel.SID = Guid.NewGuid().ToString();
                            nextstatusModel.POLICY_SID = currentstatusModel.POLICY_SID;
                            nextstatusModel.ITEM_TYPE = currentstatusModel.ITEM_TYPE;
                            nextstatusModel.STATUS_CODE = nextstatusdic.NEXT_STATUS_CODE;
                            nextstatusModel.STATUS_NAME = nextstatusdic.NEXT_STATUS_NAME;
                            nextstatusModel.CREATE_DT = DateTime.Now;
                            nextstatusModel.IS_HANDLE = "0";
                            nextstatusModel.IS_CALCULATE = "0";
                            nextstatusModel.TIME_LIMIT = nextstatusdic.TIME_LIMIT;
                            nextstatusModel.DEPT_CODE = "0000-0000";
                            nextstatusModel.DEPT_NAME = "企业经办人处理";
                            statuschangeList.Add(nextstatusModel);
                            #endregion
                        }
                        else if (nextstatusdic.NEXT_STATUS_CODE == Constant.POLICY_STATUS.企业申请)
                        {
                            #region 企业申请
                            mainInfoModel.APPLY_STATUS = "驳回";
                            #endregion
                        }
                    }
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = "审批失败！无下一流程配置信息";
                    return amm;
                }

                #region 数据入库

                int index = 0;
                using (TransactionScope ts = new TransactionScope())
                {

                    if (statuschangeList.Count > 0)
                    {
                        index += bs.AddListEntity(statuschangeList);
                    }
                    index += bs.UpdateEntity(currentstatusModel, new string[] { "HANDLE_RESULT", "HANDLE_CONTENT", "HANDLE_SID", "HANDLE_NAME", "IS_HANDLE", "HANDLE_DT","SEAL_SID" });
                    index += bs.UpdateEntity(mainInfoModel, new string[] { "STATUS_NAME", "STATUS_CODE", "APPLY_STATUS", "REJECT_REASON" });
                    foreach (var item in statuschangeupdateList)
                    {
                        index += bs.UpdateEntity(item, new string[] { "IS_HANDLE", "HANDLE_CONTENT", "HANDLE_DT" });
                    }
                    ts.Complete();
                }

                if (index >= 2)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = "审批成功！";
                }
                else
                {
                    amm.Statu = AjaxStatu.err;//这一行要加，因为验证时已更改
                    amm.Msg = "审批失败！";
                }
                #endregion
                return amm;
            }
            catch (Exception ex)
            {
                RecordLog.RecordError(ex.ToString());
                return amm;
            }
        }
    }
}