namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Share
    {
        public Share()
        {
            Athletes = new HashSet<Athlete>();
        }

        public int ShareId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("   سهمیه   ")]

        public string IsShare { get; set; }

        public virtual ICollection<Athlete> Athletes { get; set; }
    }
}
