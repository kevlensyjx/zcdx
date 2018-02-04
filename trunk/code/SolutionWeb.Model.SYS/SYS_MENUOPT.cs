namespace SolutionWeb.Model.SYS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SYS_MENUOPT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SYS_MENUOPT()
        {
            SYS_ROLE_MENUOPT_MAP = new HashSet<SYS_ROLE_MENUOPT_MAP>();
        }

        [Key]
        [StringLength(50)]
        public string MENUOPT_ID { get; set; }

        [StringLength(50)]
        public string MENUOPT_NAME { get; set; }

        [StringLength(50)]
        public string MENU_ID { get; set; }

        [StringLength(50)]
        public string C_ICO { get; set; }

        [StringLength(50)]
        public string AREA { get; set; }

        [StringLength(50)]
        public string CONTROLLER { get; set; }

        [StringLength(50)]
        public string ACTION { get; set; }

        [StringLength(50)]
        public string NOTE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SYS_ROLE_MENUOPT_MAP> SYS_ROLE_MENUOPT_MAP { get; set; }
    }
}
