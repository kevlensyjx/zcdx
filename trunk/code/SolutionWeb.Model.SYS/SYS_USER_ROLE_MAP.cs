namespace SolutionWeb.Model.SYS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SYS_USER_ROLE_MAP
    {
        [Key]
        [StringLength(50)]
        public string USER_ROLE_ID { get; set; }

        [StringLength(50)]
        public string USER_NAME { get; set; }

        [StringLength(50)]
        public string ROLE_ID { get; set; }

        public virtual SYS_ROLE SYS_ROLE { get; set; }

        public virtual SYS_USER SYS_USER { get; set; }
    }
}
