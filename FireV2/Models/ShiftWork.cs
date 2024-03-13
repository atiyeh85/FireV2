namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ShiftWork
    {
        public ShiftWork()
        {
            Athletes = new HashSet<Athlete>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ShiftWorkId { get; set; }

        [Required]
        [StringLength(50)]

        public string ShWorkTitle { get; set; }

        public int? StationId { get; set; }

        public virtual ICollection<Athlete> Athletes { get; set; }

        public virtual Station Station { get; set; }
    }
}
