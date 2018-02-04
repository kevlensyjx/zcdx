namespace SolutionWeb.Model.POLICY
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class POLICY_MAIN_INFO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public POLICY_MAIN_INFO()
        {
            POLICY_STATUS_CHANGE = new HashSet<POLICY_STATUS_CHANGE>();
        }

        [Key]
        [StringLength(50)]
        public string SID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CREATE_DT { get; set; }

        [StringLength(50)]
        public string CREATE_BY { get; set; }

        [Column(TypeName = "date")]
        public DateTime? UPDATE_DT { get; set; }

        [StringLength(50)]
        public string UPDATE_BY { get; set; }

        [StringLength(50)]
        public string APPLY_NUMBER { get; set; }

        [StringLength(50)]
        public string STATUS_NAME { get; set; }

        [StringLength(50)]
        public string STATUS_CODE { get; set; }

        [StringLength(50)]
        public string CORPORATION_SID { get; set; }

        [StringLength(100)]
        public string CORP_NAME { get; set; }

        [StringLength(100)]
        public string SOCIAL_CREDIT_CODE { get; set; }

        [StringLength(300)]
        public string REGISTERED_ADDRESS { get; set; }

        [StringLength(100)]
        public string LEGAL_PERSON { get; set; }

        [StringLength(100)]
        public string LEGAL_PERSON_PHONE { get; set; }

        [StringLength(100)]
        public string OPERATOR { get; set; }

        [StringLength(100)]
        public string OPERATOR_PHONE { get; set; }

        [StringLength(100)]
        public string OPERATOR_ID_NO { get; set; }

        [StringLength(100)]
        public string EMIAL { get; set; }

        [StringLength(100)]
        public string REGISTERED_CAPITAL { get; set; }

        [StringLength(50)]
        public string APPLY_ITEM_TYPE { get; set; }

        [StringLength(50)]
        public string APPLY_ITEM_NAME { get; set; }

        [StringLength(50)]
        public string APPLY_MONEY_WORDS { get; set; }

        public decimal? APPLY_MONEY_NUMBER { get; set; }

        [Column(TypeName = "date")]
        public DateTime? APPLY_DT { get; set; }

        [StringLength(50)]
        public string APPLY_STATUS { get; set; }

        [StringLength(50)]
        public string DATA_STATUS { get; set; }

        [StringLength(100)]
        public string BANK_NAME { get; set; }

        [StringLength(100)]
        public string BANK_ACOUNT { get; set; }

        [StringLength(2000)]
        public string CORP_PROMISE { get; set; }

        [StringLength(50)]
        public string VAT_NO { get; set; }

        [StringLength(100)]
        public string COMPANY_NAME { get; set; }

        [StringLength(50)]
        public string POLICITY_STATUS { get; set; }

        [Column(TypeName = "date")]
        public DateTime? POLICITY_BEGIN_DT { get; set; }

        [Column(TypeName = "date")]
        public DateTime? POLICITY_END_DT { get; set; }

        [StringLength(2000)]
        public string POLICITY_STOP_REASON { get; set; }

        [StringLength(50)]
        public string IS_FILE { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FILE_DT { get; set; }

        [StringLength(200)]
        public string COMPANY_ADDRESS { get; set; }

        [StringLength(50)]
        public string COMPANY_PHONE { get; set; }

        [StringLength(200)]
        public string REJECT_REASON { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<POLICY_STATUS_CHANGE> POLICY_STATUS_CHANGE { get; set; }
    }
}
