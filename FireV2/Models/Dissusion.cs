namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Dissusion
    {
        public Dissusion()
        {
        }

        [Key]
        public int DissPerId { get; set; }

        public int PeriodId { get; set; }

        public int AthleteId { get; set; }

        public string Note { get; set; }
        public string InsetDate { get; set; }
        public string InsetTIme { get; set; }
        public string InsertUser { get; set; }
        public bool Isactive { get; set; }
        public int? DissTitleId { get; set; }

        public bool? Diss { get; set; }
        public virtual Athlete Athlete { get; set; }
        public virtual DissuasionsTitle DissuasionsTitle { get; set; }
        public virtual Period Period { get; set; }
    }
}
