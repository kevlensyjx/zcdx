using SolutionWeb.Model.POLICY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolutionWeb.ViewModels
{
    public class VIEW_POLICY_MAIN_INFO_APPROVE
    {
        #region 企业基本信息

        public string SID { get; set; }
        public string APPLY_NUMBER { get; set; }
        public string MAIN_INFO_STATUS_NAME { get; set; }
        public string MAIN_INFO_STATUS_CODE { get; set; }
        public string CORPORATION_SID { get; set; }
        public string CORP_NAME { get; set; }
        public string SOCIAL_CREDIT_CODE { get; set; }
        public string REGISTERED_ADDRESS { get; set; }
        public string LEGAL_PERSON { get; set; }
        public string LEGAL_PERSON_PHONE { get; set; }
        public string OPERATOR { get; set; }
        public string OPERATOR_PHONE { get; set; }
        public string OPERATOR_ID_NO { get; set; }
        public string EMIAL { get; set; }
        public string REGISTERED_CAPITAL { get; set; }
        public string APPLY_ITEM_TYPE { get; set; }
        public string APPLY_ITEM_NAME { get; set; }
        public string APPLY_MONEY_WORDS { get; set; }
        public Nullable<decimal> APPLY_MONEY_NUMBER { get; set; }
        public Nullable<System.DateTime> APPLY_DT { get; set; }
        public string APPLY_STATUS { get; set; }

        public string STATUS_TIME { get; set; }

        #endregion

        #region 流程节点信息 A02 窗口受理
        public string IS_SHOW_1 { get; set; }
        public string HANDLE_CONTENT_1 { get; set; }
        
        #endregion

        #region 流程节点信息 B01 业务核定
        public string IS_SHOW_2 { get; set; }
        public string HANDLE_CONTENT_2 { get; set; }
        #endregion

        #region 流程节点信息 B02 项目评价
        public string IS_SHOW_3 { get; set; }
        public string HANDLE_CONTENT_3 { get; set; }
       
        #endregion

        #region 流程节点信息 B03 部门会审
        public string IS_SHOW_4 { get; set; }
        public string HANDLE_CONTENT_4 { get; set; }
       
        #endregion

        #region 流程节点信息 C01 分管领导审批
        public string HANDLE_CONTENT_5 { get; set; }
        
        #endregion
        #region 流程节点信息 C02 事项公示
        public string HANDLE_CONTENT_6 { get; set; }
        #endregion
        #region 流程节点信息 D02 财务拨款
        public string HANDLE_CONTENT_7 { get; set; }

        #endregion
        #region 是否启用电子签章
        public string SEAL_FLAG { get; set; }
        #endregion
    }

    public partial class VIEW_POLICY_MAIN_INFO
    {
        public List<POLICY_APPLY_FILE> fileList { set; get; }
    }
}