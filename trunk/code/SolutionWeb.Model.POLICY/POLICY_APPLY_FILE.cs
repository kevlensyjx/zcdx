namespace SolutionWeb.Model.POLICY
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class POLICY_APPLY_FILE
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

        [StringLength(50)]
        public string FILE_CLASS { get; set; }

        [StringLength(50)]
        public string MAIN_SID { get; set; }

        [StringLength(500)]
        public string FILE_NAME { get; set; }

        [StringLength(50)]
        public string FILE_TYPE { get; set; }

        [StringLength(500)]
        public string FILE_PATH { get; set; }

        [StringLength(100)]
        public string DOCUMENT_NAME { get; set; }

        [StringLength(50)]
        public string PATENT_NUMBER { get; set; }
    }
}
