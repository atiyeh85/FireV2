namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Medical
    {
        public Medical()
        {
            MAthleteByPeriods = new HashSet<MAthleteByPeriod>();
            FileUploads = new HashSet<FileUpload>();
            AthTypeMedicals = new HashSet<AthTypeMedical>();
        }

        public int Medicalid { get; set; }

        public string DateInsert { get; set; }
        public string UserInsert { get; set; }
        public string LastUserE { get; set; }
        public string LastDateE { get; set; }
        public bool Flag { get; set; }
        public int AthleteId { get; set; }
        [Required(ErrorMessage = "دوره را وارد کنید")]

        public int PeriodId { get; set; }

        [StringLength(500)]
        public string Note { get; set; }
        [Required(ErrorMessage = "نوع تست را وارد کنید")]

        public int TypeOfTestId { get; set; }

        public virtual TypeOfTest TypeOfTest { get; set; }
        public virtual Athlete Athlete { get; set; }
        public virtual ICollection<MAthleteByPeriod> MAthleteByPeriods { get; set; }
        public virtual ICollection<FileUpload> FileUploads { get; set; }
        public virtual ICollection<AthTypeMedical> AthTypeMedicals { get; set; }
        public virtual Period Period { get; set; }

    }
}
