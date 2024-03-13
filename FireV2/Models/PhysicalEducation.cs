namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PhysicalEducation
    {
        public PhysicalEducation()
        {
            Athletes = new HashSet<Athlete>();
        }

        [Key]
        public int PhyEduId { get; set; }

        public int? AthleteId { get; set; }

        public int? PeriodId { get; set; }

        public int? PhyEduItemId { get; set; }

        public virtual ICollection<Athlete> Athletes { get; set; }

        public virtual Period Period { get; set; }

        public virtual PhysicalEducationItem PhysicalEducationItem { get; set; }
    }
}
