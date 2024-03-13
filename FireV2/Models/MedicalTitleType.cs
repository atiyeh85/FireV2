namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MedicalTitleType
    {
        public MedicalTitleType()
        {
            MAthleteByPeriods = new HashSet<MAthleteByPeriod>();
            AthTypeMedicals = new HashSet<AthTypeMedical>();
        }

        [Key]
        public int MTitleTypeid { get; set; }

        public string MTType { get; set; }

        public int MedicalItemid { get; set; }

        public virtual ICollection<MAthleteByPeriod> MAthleteByPeriods { get; set; }
        public virtual ICollection<AthTypeMedical> AthTypeMedicals { get; set; }
        public virtual MedicalItem MedicalItem { get; set; }
    }
}
