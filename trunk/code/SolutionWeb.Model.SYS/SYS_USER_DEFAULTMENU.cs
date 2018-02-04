namespace SolutionWeb.Model.SYS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SYS_USER_DEFAULTMENU
    {
        [Key]
        [StringLength(50)]
        public string USER_DEFAULT_ID { get; set; }

        [StringLength(50)]
        public string USER_NAME { get; set; }

        [StringLength(50)]
        public string MENU_ID { get; set; }

        public virtual SYS_MENU SYS_MENU { get; set; }

        public virtual SYS_USER SYS_USER { get; set; }
    }
}
