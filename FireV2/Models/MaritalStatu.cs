namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MaritalStatu
    {
        public MaritalStatu()
        {
            Athletes = new HashSet<Athlete>();
        }

        [Key]
        public int MaritalStatusId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("    وضعیتت تاهل ")]

        public string MaritalStatusTitle { get; set; }

        public virtual ICollection<Athlete> Athletes { get; set; }
    }
}
