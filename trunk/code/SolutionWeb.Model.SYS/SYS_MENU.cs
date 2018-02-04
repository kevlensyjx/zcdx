namespace SolutionWeb.Model.SYS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SYS_MENU
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SYS_MENU()
        {
            SYS_ROLE_MENU_MAP = new HashSet<SYS_ROLE_MENU_MAP>();
            SYS_USER_DEFAULTMENU = new HashSet<SYS_USER_DEFAULTMENU>();
        }

        [Key]
        [StringLength(50)]
        public string MENU_ID { get; set; }

        [StringLength(50)]
        public string MENU_NAME { get; set; }

        [StringLength(50)]
        public string C_ICO { get; set; }

        [StringLength(50)]
        public string AREA { get; set; }

        [StringLength(50)]
        public string CONTROLLER { get; set; }

        [StringLength(50)]
        public string ACTION { get; set; }

        [Required]
        [StringLength(50)]
        public string PARENT_ID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal MENU_LEVEL { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MENU_ORDER { get; set; }

        [StringLength(1)]
        public string GIS_ORDER { get; set; }

        [StringLength(1)]
        public string ISSHOW_FLAG { get; set; }

        [StringLength(50)]
        public string DEFMENU_ID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SYS_ROLE_MENU_MAP> SYS_ROLE_MENU_MAP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SYS_USER_DEFAULTMENU> SYS_USER_DEFAULTMENU { get; set; }
    }
}
