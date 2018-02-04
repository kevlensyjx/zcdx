namespace SolutionWeb.Model.POLICY
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CORPORATION_BASE_INFO
    {
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
        public string USER_NAME { get; set; }

        [StringLength(50)]
        public string PASSWORD { get; set; }

        [StringLength(50)]
        public string CORP_STATUS { get; set; }

        [StringLength(50)]
        public string APPLY_RESULT { get; set; }
    }
}
