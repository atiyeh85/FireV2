namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BMIS")]
    public partial class BMI
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ABmiid { get; set; }
        public int PeriodId { get; set; }
        [DisplayName(" پیغام سیستم ")]

        public string ABmiText { get; set; }

        [StringLength(50)]
        public string Date { get; set; }
        [DisplayName(" قد ")]

        public double Size { get; set; }
        [DisplayName(" وزن")]

        public double Weight { get; set; }
        [DisplayName(" BMI ")]

        public string BMIRenj { get; set; }

        public int? AthleteId { get; set; }
        public virtual Period Period { get; set; }
        public virtual Athlete Athlete { get; set; }
    }
}
