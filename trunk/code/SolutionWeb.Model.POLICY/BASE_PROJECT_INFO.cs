namespace SolutionWeb.Model.POLICY
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BASE_PROJECT_INFO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BASE_PROJECT_INFO()
        {
            BASE_PROJECT_CONFIG = new HashSet<BASE_PROJECT_CONFIG>();
        }

        [Key]
        [StringLength(50)]
        public string SID { get; set; }

        [StringLength(50)]
        public string ITEM_TYPE { get; set; }

        [StringLength(200)]
        public string ITEM_NAME { get; set; }

        [StringLength(50)]
        public string CASHING_WAY { get; set; }

        public int? LAY_ORDER { get; set; }

        [StringLength(50)]
        public string ITEM_CODE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BASE_PROJECT_CONFIG> BASE_PROJECT_CONFIG { get; set; }
    }
}
