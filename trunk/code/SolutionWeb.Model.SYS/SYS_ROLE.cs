namespace SolutionWeb.Model.SYS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SYS_ROLE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SYS_ROLE()
        {
            SYS_ROLE_MENU_MAP = new HashSet<SYS_ROLE_MENU_MAP>();
            SYS_ROLE_MENUOPT_MAP = new HashSet<SYS_ROLE_MENUOPT_MAP>();
            SYS_USER_ROLE_MAP = new HashSet<SYS_USER_ROLE_MAP>();
        }

        [Key]
        [StringLength(50)]
        public string ROLE_ID { get; set; }

        [StringLength(50)]
        public string NAME { get; set; }

        [StringLength(200)]
        public string NOTE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SYS_ROLE_MENU_MAP> SYS_ROLE_MENU_MAP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SYS_ROLE_MENUOPT_MAP> SYS_ROLE_MENUOPT_MAP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SYS_USER_ROLE_MAP> SYS_USER_ROLE_MAP { get; set; }
    }
}
