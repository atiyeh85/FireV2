namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AgeRenge
    {
        public AgeRenge()
        {
            phRItemsPoints = new HashSet<phRItemsPoint>();
            //RengAthleteInPeriods = new HashSet<RengAthleteInPeriod>();
        }

        public int AgeRengeId { get; set; }
        [DisplayName("  کمترین  رنج")]
        public int MinRenge { get; set; }
        [DisplayName(" بیشترین رنج")]
        public int MaxReng { get; set; }
        public string TitleAge { get; set; }

        //public virtual ICollection<RengAthleteInPeriod> RengAthleteInPeriods { get; set; }
        public virtual ICollection<phRItemsPoint> phRItemsPoints { get; set; }
    }
}
