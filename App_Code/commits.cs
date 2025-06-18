namespace CodingRep.App_Code
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class commits
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public commits()
        {
            branches = new HashSet<branches>();
            commits1 = new HashSet<commits>();
            fileSnapshots = new HashSet<fileSnapshots>();
        }

        public int id { get; set; }

        public int repoId { get; set; }

        public int userId { get; set; }

        [StringLength(256)]
        public string message { get; set; }

        public int? parentId { get; set; }

        public DateTime timestamp { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<branches> branches { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<commits> commits1 { get; set; }

        public virtual commits commits2 { get; set; }

        public virtual repositories repositories { get; set; }

        public virtual users users { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fileSnapshots> fileSnapshots { get; set; }
    }
}
