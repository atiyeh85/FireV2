namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PostSazmani
    {
        public PostSazmani()
        {
            Athletes = new HashSet<Athlete>();
        }

        public int PostSazmaniId { get; set; }

        [Required]
        [DisplayName("   پست سازمانی   ")]

        public string PSazmaniTitle { get; set; }

        public virtual ICollection<Athlete> Athletes { get; set; }
    }
}
