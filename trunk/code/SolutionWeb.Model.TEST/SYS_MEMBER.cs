namespace SolutionWeb.Model.TEST
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SYS_MEMBER
    {
        [Key]
        [StringLength(50)]
        public string MEMBER_ID { get; set; }

        [StringLength(50)]
        public string NAME { get; set; }

        [StringLength(50)]
        public string MOBILE { get; set; }

        [StringLength(50)]
        public string JOB { get; set; }

        [StringLength(50)]
        public string PHONE { get; set; }

        [StringLength(200)]
        public string NOTE { get; set; }

        public DateTime? UPDATE_DATE { get; set; }

        [StringLength(50)]
        public string DEPT_CODE { get; set; }

        [StringLength(50)]
        public string UPDATE_USER { get; set; }

        [StringLength(1)]
        public string DEL_FLAG { get; set; }

        [StringLength(1)]
        public string LOCATION_FLAG { get; set; }

        [StringLength(50)]
        public string INTELLIGENCE { get; set; }

        [StringLength(50)]
        public string POS_LEVEL { get; set; }

        [StringLength(50)]
        public string MOBILE_STATE { get; set; }
    }
}
