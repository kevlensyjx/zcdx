using SolutionWeb.Common;
using SolutionWeb.Model.POLICY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using System.Web;

namespace SolutionWeb.User.Models
{
    public partial class Model_POLICY_MAIN_INFO
    {
        public AjaxMsgModel Add(POLICY_MAIN_INFO data, List<POLICY_APPLY_FILE> listFiles)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {
                int returnValue = 0;
                #region 申请状态处理  cwb 添加
               
                POLICY_STATUS_CHANGE nextstatusModel = null;
                if (data.DATA_STATUS == "提交")
                {
                    nextstatusModel = new POLICY_STATUS_CHANGE();
                    nextstatusModel.SID = Guid.NewGuid().ToString();
                    nextstatusModel.POLICY_SID = data.SID;
                    nextstatusModel.ITEM_TYPE = data.APPLY_ITEM_TYPE;
                    nextstatusModel.STATUS_CODE = Constant.POLICY_STATUS.窗口受理;
                    nextstatusModel.STATUS_NAME = "窗口受理";
                    nextstatusModel.CREATE_DT = DateTime.Now;
                    nextstatusModel.IS_HANDLE = "0";
                    nextstatusModel.IS_CALCULATE = "0";
                    nextstatusModel.TIME_LIMIT = 0;
                    nextstatusModel.DEPT_CODE = "011501";
                    nextstatusModel.DEPT_NAME = "受理窗口";
                }
                data.STATUS_CODE = Constant.POLICY_STATUS.企业申请;
                data.STATUS_NAME = "企业申请";
                data.APPLY_DT = DateTime.Now;
                #endregion
                using (TransactionScope ts = new TransactionScope())
                {
                    returnValue = bs.AddEntity(data);
                    returnValue += bs.AddListEntity(listFiles);
                    #region 申请状态处理  cwb 添加
                    if (nextstatusModel != null)
                    {
                        returnValue += bs.AddEntity(nextstatusModel);
                    }
                    #endregion
                    ts.Complete();
                }
                if (returnValue >1)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = "申请提交成功，请等待管理员审核，审核通过后会以短信方式通知您！";
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = "申请失败";
                }
                
                return amm;
            }
            catch (Exception ex)
            {
                RecordLog.RecordError(ex.ToString());
                return amm;
            }
        }

        public AjaxMsgModel Edit(POLICY_MAIN_INFO data, List<POLICY_APPLY_FILE> listFiles)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {
                int returnValue = 0;

                #region 申请状态处理  cwb 添加

                POLICY_STATUS_CHANGE nextstatusModel = null;
                if (data.DATA_STATUS == "提交")
                {
                    nextstatusModel = new POLICY_STATUS_CHANGE();
                    nextstatusModel.SID = Guid.NewGuid().ToString();
                    nextstatusModel.POLICY_SID = data.SID;
                    nextstatusModel.ITEM_TYPE = data.APPLY_ITEM_TYPE;
                    nextstatusModel.STATUS_CODE = Constant.POLICY_STATUS.窗口受理;
                    nextstatusModel.STATUS_NAME = "窗口受理";
                    nextstatusModel.CREATE_DT = DateTime.Now;
                    nextstatusModel.IS_HANDLE = "0";
                    nextstatusModel.IS_CALCULATE = "0";
                    nextstatusModel.TIME_LIMIT = 0;
                    nextstatusModel.DEPT_CODE = "011501";
                    nextstatusModel.DEPT_NAME = "受理窗口";
                }
                data.APPLY_DT = DateTime.Now;
                #endregion

                using (TransactionScope ts = new TransactionScope())
                {
                    Expression<Func<POLICY_MAIN_INFO, object>>[] ignoreProperties =
                        new Expression<Func<POLICY_MAIN_INFO, object>>[]
                        {
                            p => p.POLICY_STATUS_CHANGE
                            , p => p.CREATE_DT
                            , p => p.CREATE_BY
                            , p => p.CORPORATION_SID
                            , p => p.REJECT_REASON
                        };
                    
                    returnValue = bs.UpdateEntity(data, ignoreProperties);
                    returnValue += bs.DelByWhere<POLICY_APPLY_FILE>(u=>u.MAIN_SID==data.SID);
                    returnValue += bs.AddListEntity(listFiles);
                    #region 申请状态处理  cwb 添加
                    if (nextstatusModel != null)
                    {
                        returnValue += bs.AddEntity(nextstatusModel);
                    }
                    #endregion
                    ts.Complete();
                }
                if (returnValue > 1)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = "申请提交成功，请等待管理员审核，审核通过后会以短信方式通知您！";
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = "申请失败";
                }

                return amm;
            }
            catch (Exception ex)
            {
                RecordLog.RecordError(ex.ToString());
                return amm;
            }
        }

        
        public AjaxMsgModel Del(string SID)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            int returnValue = 0;
            using (TransactionScope ts = new TransactionScope())
            {
                returnValue += bs.DelByWhere<POLICY_STATUS_CHANGE>(u => u.POLICY_SID == SID);
                returnValue += bs.DelByWhere<POLICY_APPLY_FILE>(u => u.MAIN_SID == SID);
                returnValue = bs.DelByWhere<POLICY_MAIN_INFO>(u => u.SID == SID);
                ts.Complete();
            }
            if (returnValue > 0)
            {
                amm.Statu = AjaxStatu.ok;
                amm.Msg = string.Format(Message.OptSussess, "申请", Message.DelOpt);
            }
            else
            {
                amm.Statu = AjaxStatu.err;
                amm.Msg = string.Format(Message.OptFail, "申请", Message.DelOpt);
            }
            return amm;
        }

        #region 企业请拨
        /// <summary>
        /// 企业请拨
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public AjaxMsgModel CompanyApplyBank(POLICY_MAIN_INFO data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {
                int returnValue = 0;

                #region 企业请拨处理  cwb 添加
                //需提前判断申请企业是否已进行过企业请拨
                POLICY_MAIN_INFO model = bs.Entities<POLICY_MAIN_INFO>().FirstOrDefault(o => o.SID.Equals(data.SID));
                if (model != null)
                {
                    if (model.STATUS_CODE != Constant.POLICY_STATUS.企业请拨)
                    {
                        amm.Statu = AjaxStatu.err;
                        amm.Msg = "您已进行过请求拨款，请勿重复操作！";
                        return amm;
                    }
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = "非法操作";
                    return amm;
                }
                BASE_WORKFLOW_INFO workflow =
                    bs.Entities<BASE_WORKFLOW_INFO>()
                        .FirstOrDefault(o => o.STATUS_CODE == Constant.POLICY_STATUS.企业请拨 && o.HANDLE_RESULT == "1" && o.ITEM_TYPE == data.APPLY_ITEM_TYPE);

                POLICY_STATUS_CHANGE nextstatusModel = null;
                if (workflow != null)
                {
                    nextstatusModel = new POLICY_STATUS_CHANGE();
                    nextstatusModel.SID = Guid.NewGuid().ToString();
                    nextstatusModel.POLICY_SID = data.SID;
                    nextstatusModel.ITEM_TYPE = data.APPLY_ITEM_TYPE;
                    nextstatusModel.STATUS_CODE = workflow.NEXT_STATUS_CODE;
                    nextstatusModel.STATUS_NAME = workflow.NEXT_STATUS_NAME;
                    nextstatusModel.CREATE_DT = DateTime.Now;
                    nextstatusModel.IS_HANDLE = "0";
                    nextstatusModel.IS_CALCULATE = "0";
                    nextstatusModel.TIME_LIMIT = workflow.TIME_LIMIT;
                    nextstatusModel.DEPT_CODE = "0113";
                    nextstatusModel.DEPT_NAME = "财务部";

                    data.STATUS_CODE = workflow.NEXT_STATUS_CODE;
                    data.STATUS_NAME = workflow.NEXT_STATUS_NAME;
                }

                #endregion

                using (TransactionScope ts = new TransactionScope())
                {

                    #region 企业请拨处理  cwb 添加
                    if (nextstatusModel != null)
                    {
                        returnValue += bs.AddEntity(nextstatusModel);
                        returnValue += bs.UpdateEntity(data,
                            new string[]
                            {"STATUS_CODE", "STATUS_NAME", "BANK_NAME", "BANK_ACOUNT", "VAT_NO", "COMPANY_NAME", "COMPANY_ADDRESS", "COMPANY_PHONE"});
                    }
                    #endregion

                    ts.Complete();
                }
                if (returnValue > 1)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = "提交成功";
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = "提交失败";
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


        public AjaxMsgModel Over(string SID)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            POLICY_MAIN_INFO model = bs.Entities<POLICY_MAIN_INFO>().FirstOrDefault(o => o.SID.Equals(SID));
            if (model != null)
            {
                if (model.STATUS_CODE != Constant.POLICY_STATUS.企业确认)
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = "您已进行过企业确认，请勿重复操作！";
                    return amm;
                }
            }
            else
            {
                amm.Statu = AjaxStatu.err;
                amm.Msg = "非法操作";
                return amm;
            }

            POLICY_MAIN_INFO data = new POLICY_MAIN_INFO()
            {
                SID = SID,
                STATUS_CODE = Constant.POLICY_STATUS.备案归档,
                STATUS_NAME = "备案归档"
            };
            POLICY_STATUS_CHANGE nextstatusModel = null;

            nextstatusModel = new POLICY_STATUS_CHANGE();
            nextstatusModel.SID = Guid.NewGuid().ToString();
            nextstatusModel.POLICY_SID = data.SID;
            nextstatusModel.ITEM_TYPE = data.APPLY_ITEM_TYPE;
            nextstatusModel.STATUS_CODE = Constant.POLICY_STATUS.备案归档;
            nextstatusModel.STATUS_NAME = "备案归档";
            nextstatusModel.CREATE_DT = DateTime.Now;
            nextstatusModel.IS_HANDLE = "0";
            nextstatusModel.IS_CALCULATE = "0";
            nextstatusModel.TIME_LIMIT = 0;
            nextstatusModel.DEPT_CODE = "011501";
            nextstatusModel.DEPT_NAME = "受理窗口";

            int returnValue = 0;
            using (TransactionScope ts = new TransactionScope())
            {
                returnValue += bs.UpdateEntity(data, new string[] { "STATUS_CODE", "STATUS_NAME" });
                returnValue += bs.AddEntity(nextstatusModel);
                ts.Complete();
            }
            if (returnValue > 0)
            {
                amm.Statu = AjaxStatu.ok;
                amm.Msg = "收款确认成功！";
            }
            else
            {
                amm.Statu = AjaxStatu.err;
                amm.Msg = "收款确认失败！";
            }
            return amm;
        }
    }
}