namespace CodingRep.App_Code
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class repositories
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public repositories()
        {
            branches = new HashSet<branches>();
            commits = new HashSet<commits>();
        }

        public int id { get; set; }

        public int userId { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }

        [StringLength(500)]
        public string description { get; set; }

        public DateTime createdAt { get; set; }

        public bool isPrivate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<branches> branches { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<commits> commits { get; set; }

        public virtual users users { get; set; }
    }
}
