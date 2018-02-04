namespace SolutionWeb.Model.POLICY
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BASE_PROJECT_CONFIG
    {
        [Key]
        [StringLength(50)]
        public string SID { get; set; }

        [StringLength(50)]
        public string PROJECT_SID { get; set; }

        [StringLength(50)]
        public string DEPT_TYPE { get; set; }

        [StringLength(50)]
        public string DEPT_CODE { get; set; }

        [StringLength(100)]
        public string DEPT_NAME { get; set; }

        [StringLength(50)]
        public string IS_VOTE { get; set; }

        public virtual BASE_PROJECT_INFO BASE_PROJECT_INFO { get; set; }
    }
}
