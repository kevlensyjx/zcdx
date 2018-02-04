namespace SolutionWeb.Model.POLICY
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BASE_STATUS_DIC
    {
        [Key]
        [StringLength(50)]
        public string SID { get; set; }

        [StringLength(50)]
        public string S_CODE { get; set; }

        [StringLength(50)]
        public string S_NAME { get; set; }

        [StringLength(500)]
        public string NOTE { get; set; }

        [StringLength(50)]
        public string S_TYPE { get; set; }

        public int? S_SORT { get; set; }

        [StringLength(50)]
        public string S_SHOW { get; set; }
    }
}
