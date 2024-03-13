namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ostan
    {
        public Ostan()
        {
            Cities = new HashSet<City>();
        }

        public int OstanId { get; set; }

        [StringLength(50)]
        public string OstanName { get; set; }


        public virtual ICollection<City> Cities { get; set; }
    }
}
