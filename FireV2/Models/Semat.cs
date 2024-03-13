namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Semat
    {
        public Semat()
        {
            Athletes = new HashSet<Athlete>();
        }

        public int SematId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("    ”„   ")]

        public string SematName { get; set; }

        public virtual ICollection<Athlete> Athletes { get; set; }
    }
}
