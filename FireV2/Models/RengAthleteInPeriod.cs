namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RengAthleteInPeriod
    {
        [Key]
        public int RangePeriodId { get; set; }

        public string AgeReng { get; set; }
        public string UserDate { get; set; }
        public string InsertDate { get; set; }
        public int Age { get; set; }
        public int? AthleteId { get; set; }

        public int? YearOfP { get; set; }
        public int AgeRengeId { get; set; }

        //public virtual Period Period { get; set; }
        //public virtual AgeRenge AgeRenge { get; set; }

    }
}
