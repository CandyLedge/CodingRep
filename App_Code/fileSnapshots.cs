namespace CodingRep.App_Code
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class fileSnapshots
    {
        public int id { get; set; }

        public int commitId { get; set; }

        [Required]
        [StringLength(200)]
        public string path { get; set; }

        public byte[] content { get; set; }

        [StringLength(40)]
        public string contentHash { get; set; }

        public int fileMode { get; set; }

        public DateTime createdAt { get; set; }

        public virtual commits commits { get; set; }
    }
}
