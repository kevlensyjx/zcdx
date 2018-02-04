namespace SolutionWeb.Model.SYS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SYS_ROLE_MENUOPT_MAP
    {
        [Key]
        [StringLength(50)]
        public string ROLE_MENUOPT_ID { get; set; }

        [StringLength(50)]
        public string ROLE_ID { get; set; }

        [StringLength(50)]
        public string MENUOPT_ID { get; set; }

        public virtual SYS_MENUOPT SYS_MENUOPT { get; set; }

        public virtual SYS_ROLE SYS_ROLE { get; set; }
    }
}
