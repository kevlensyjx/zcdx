namespace SolutionWeb.Model.SYS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SYS_USER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SYS_USER()
        {
            SYS_USER_ROLE_MAP = new HashSet<SYS_USER_ROLE_MAP>();
            SYS_USER_DEFAULTMENU = new HashSet<SYS_USER_DEFAULTMENU>();
        }

        [Key]
        [StringLength(50)]
        public string USER_NAME { get; set; }

        [StringLength(50)]
        public string PASSWORD { get; set; }

        [StringLength(50)]
        public string DEPT_CODE { get; set; }

        [Column(TypeName = "date")]
        public DateTime? UPDATE_DATE { get; set; }

        [StringLength(50)]
        public string UPDATE_USER { get; set; }

        [StringLength(255)]
        public string NOTE { get; set; }

        [StringLength(50)]
        public string MANAGE_DEPT_CODE { get; set; }

        [StringLength(50)]
        public string ZSNAME { get; set; }

        public virtual SYS_DEPT SYS_DEPT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SYS_USER_ROLE_MAP> SYS_USER_ROLE_MAP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SYS_USER_DEFAULTMENU> SYS_USER_DEFAULTMENU { get; set; }
    }
}
