namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TeamByPeriod
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TeamByPeriodId { get; set; }

        public int PeriodId { get; set; }

        public int AthleteId { get; set; }

        public virtual Period Period { get; set; }
    }
}
