namespace CodingRep.App_Code
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public users()
        {
            commits = new HashSet<commits>();
            repositories = new HashSet<repositories>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string userName { get; set; }

        [Required]
        [StringLength(256)]
        public string passwordHash { get; set; }

        [Required]
        [StringLength(50)]
        public string salt { get; set; }

        [Required]
        [StringLength(100)]
        public string email { get; set; }

        [StringLength(256)]
        public string avatarUrl { get; set; }

        public DateTime createdAt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<commits> commits { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<repositories> repositories { get; set; }
    }
}
