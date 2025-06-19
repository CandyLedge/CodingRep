namespace CodingRep.App_Code
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class branches
    {
        public int id { get; set; }

        public int repoId { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }

        public int? commitId { get; set; }

        public DateTime createdAt { get; set; }

        public virtual commits commits { get; set; }

        public virtual repositories repositories { get; set; }
    }
}
