namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Degree
    {
        public Degree()
        {
            Athletes = new HashSet<Athlete>();
        }

        public int DegreeId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("    مدرک تحصیلی")]

        public string DegreeName { get; set; }

        public virtual ICollection<Athlete> Athletes { get; set; }
    }
}
