namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MAthleteByPeriod
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MAthByPeriodid { get; set; }

        public int? Medicalid { get; set; }

        public int? MTitleTypeid { get; set; }

        public virtual Medical Medical { get; set; }

        public virtual MedicalTitleType MedicalTitleType { get; set; }
    }
}
