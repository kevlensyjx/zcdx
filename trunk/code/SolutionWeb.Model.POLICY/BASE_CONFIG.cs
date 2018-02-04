namespace SolutionWeb.Model.POLICY
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BASE_CONFIG
    {
        [Key]
        [StringLength(50)]
        public string SID { get; set; }

        [StringLength(50)]
        public string CONFIG_TYPE { get; set; }

        [StringLength(50)]
        public string CONFIG_NAME { get; set; }

        [StringLength(50)]
        public string CONFIG_VALUE { get; set; }
       
        [StringLength(100)]
        public string CONFIG_REMARK { get; set; }

     
    }
}
