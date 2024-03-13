namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class City
    {
        public int CityId { get; set; }

        [StringLength(50)]
        public string CitiyName { get; set; }

        public int? OstanId { get; set; }

        public virtual Ostan Ostan { get; set; }
    }
}
