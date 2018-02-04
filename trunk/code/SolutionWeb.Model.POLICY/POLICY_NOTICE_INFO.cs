namespace SolutionWeb.Model.POLICY
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class POLICY_NOTICE_INFO
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

        [StringLength(200)]
        public string NOTICE_TITLE { get; set; }

        [StringLength(2000)]
        public string NOTICE_CONTENT { get; set; }

        [StringLength(50)]
        public string IS_SHOW { get; set; }

        [StringLength(50)]
        public string NOTICE_TYPE { get; set; }
    }
}
