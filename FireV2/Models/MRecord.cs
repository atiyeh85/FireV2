namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MRecord
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MRecordId { get; set; }

        public int? PeriodId { get; set; }

        public int? AthleteId { get; set; }

        public int? MedicalExamId { get; set; }

        public string Note { get; set; }

        public virtual Athlete Athlete { get; set; }

        public virtual MedicalExam MedicalExam { get; set; }
    }
}
