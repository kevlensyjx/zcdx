namespace SolutionWeb.Model.POLICY
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BASE_WORKFLOW_INFO
    {
        [Key]
        [StringLength(50)]
        public string SID { get; set; }

        [StringLength(50)]
        public string ITEM_TYPE { get; set; }

        [StringLength(50)]
        public string STATUS_NAME { get; set; }

        [StringLength(50)]
        public string STATUS_CODE { get; set; }

        [StringLength(50)]
        public string HANDLE_RESULT { get; set; }

        [StringLength(50)]
        public string NEXT_STATUS_NAME { get; set; }

        [StringLength(50)]
        public string NEXT_STATUS_CODE { get; set; }

        public int? TIME_LIMIT { get; set; }

        [StringLength(50)]
        public string BUTTONS_VALUE { get; set; }

        [StringLength(2000)]
        public string BUTTONS { get; set; }

        [StringLength(4000)]
        public string BACK_STEP { get; set; }

        [StringLength(4000)]
        public string PREVIOUS_STEP { get; set; }

        [StringLength(4000)]
        public string NEXT_STEP { get; set; }

        [StringLength(50)]
        public string IS_SEAL { get; set; }

        [StringLength(50)]
        public string IS_HAS_FILE { get; set; }

        [StringLength(4000)]
        public string DEFAULT_HANDLER { get; set; }

        [StringLength(50)]
        public string BACK_CLASS { get; set; }

        [StringLength(50)]
        public string COUNTERSIGN { get; set; }
    }
}
