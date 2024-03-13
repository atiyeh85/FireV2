namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SportScoreByPeriod
    {
        [Key]
        public int SScoreBypriodId { get; set; }

        public int SScoreId { get; set; }

        public int PeriodId { get; set; }
        public int AthleteId { get; set; }
        public virtual Period Period { get; set; }
        public virtual Athlete Athlete { get; set; }

        public virtual SportsScoreTitle SportsScoreTitle { get; set; }
    }
}
