namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Station
    {
        public Station()
        {
            ShiftWorks = new HashSet<ShiftWork>();
        }

        public int StationId { get; set; }

        [Required]
        [StringLength(50)]
        public string StationTitle { get; set; }

        public virtual ICollection<ShiftWork> ShiftWorks { get; set; }
    }
}
