namespace SolutionWeb.Model.POLICY
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class POLICY_STATUS_CHANGE
    {
        [Key]
        [StringLength(50)]
        public string SID { get; set; }

        [StringLength(50)]
        public string POLICY_SID { get; set; }

        [StringLength(50)]
        public string DEPT_CODE { get; set; }

        [StringLength(50)]
        public string ITEM_TYPE { get; set; }

        [StringLength(50)]
        public string STATUS_NAME { get; set; }

        [StringLength(50)]
        public string STATUS_CODE { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CREATE_DT { get; set; }

        [StringLength(50)]
        public string HANDLE_SID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? HANDLE_DT { get; set; }

        [StringLength(50)]
        public string HANDLE_RESULT { get; set; }

        [StringLength(2000)]
        public string HANDLE_CONTENT { get; set; }

        [StringLength(50)]
        public string IS_HANDLE { get; set; }

        [StringLength(50)]
        public string IS_CALCULATE { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CALCULATE_DT { get; set; }

        public int? TIME_LIMIT { get; set; }

        [StringLength(50)]
        public string HANDLE_NAME { get; set; }

        [StringLength(50)]
        public string DEPT_NAME { get; set; }

        [StringLength(50)]
        public string IS_VOTE { get; set; }
        [StringLength(50)]
        public string SEAL_SID { get; set; }
        public virtual POLICY_MAIN_INFO POLICY_MAIN_INFO { get; set; }
    }
}
