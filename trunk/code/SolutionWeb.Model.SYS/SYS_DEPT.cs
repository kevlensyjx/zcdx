namespace SolutionWeb.Model.SYS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SYS_DEPT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SYS_DEPT()
        {
            SYS_MEMBER = new HashSet<SYS_MEMBER>();
            SYS_USER = new HashSet<SYS_USER>();
        }

        [Key]
        [StringLength(50)]
        public string DEPT_CODE { get; set; }

        [StringLength(50)]
        public string DEPT_NAME { get; set; }

        [Required]
        [StringLength(50)]
        public string PARENT_CODE { get; set; }

        [StringLength(50)]
        public string C_ICO { get; set; }

        [StringLength(1)]
        public string STATUS_FLAG { get; set; }

        [StringLength(1)]
        public string DEL_FLAG { get; set; }

        [StringLength(200)]
        public string NOTE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DEPT_ORDER { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DEPT_FLAG { get; set; }

        [StringLength(50)]
        public string ROLE_ID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SYS_MEMBER> SYS_MEMBER { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SYS_USER> SYS_USER { get; set; }
    }
}
