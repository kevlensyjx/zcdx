using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolutionWeb.ViewModels
{
    public partial class VIEW_POLICY_STATUS_CHANGE
    {

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
        public string POLICITY_STATUS { get; set; }
        public string POLICITY_BEGIN_DT { get; set; }
        public string POLICITY_END_DT { get; set; }
        public string POLICITY_STOP_REASON { get; set; }
        public string COMPANY_ADDRESS { get; set; }


        public string REJECT_REASON { get; set; }
        public string COMPANY_PHONE { get; set; }
        public string VAT_NO { get; set; }
        public string COMPANY_NAME { get; set; }
        public string BANK_NAME { get; set; }
        public string BANK_ACOUNT { get; set; }

        public string STATUS_TIME { get; set; }
        public Nullable<System.DateTime> START_TIME { get; set; }
        public Nullable<System.DateTime> END_TIME { get; set; }

    }
}