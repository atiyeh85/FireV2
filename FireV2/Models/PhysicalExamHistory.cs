namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PhysicalExamHistory
    {
        public PhysicalExamHistory()
        {

            PhRedinessHistoryItems = new HashSet<PhRedinessHistoryItem>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PhysicalExamHistoryId { get; set; }

        public int PeriodId { get; set; }
        public string UserInsert { get; set; }
        public string TimeInsert { get; set; }
        public int? TeamByPeriodId { get; set; }
        public int AgeReng { get; set; }
        public int AthleteId { get; set; }
        [DisplayName("   مجموع امتیاز آمادگی جسمانی  ")]

        public double? TotalPhysicalScore { get; set; }
        public int Reng { get; set; }
        [StringLength(50)]
        [DisplayName("    مجموع زمان کل عملیات تیمی  ")]
        public string TotalTimeWorkTime { get; set; }
        public string StatusScore { get; set; }

        [StringLength(50)]
        [DisplayName("   زمان عملیات تیمی  ")]
        public string TeamWorkTime { get; set; }

        [DisplayName("   زمان  خطاعملیات تیمی  ")]

        public string TeamWorkFaltTime { get; set; }

        [DisplayName("   امتیاز عملیات تیمی  ")]

        public double? TeamWorkScore { get; set; }
        [DisplayName("   توضیحات  ")]

        public string PhysicalExamHistoryNote { get; set; }
        [DisplayName("   امتیاز ورزشی   ")]

        public double? SportsScore { get; set; }

        [DisplayName("  امتیاز عملیات انفرادی  ")]

        public double? IndTotalScore { get; set; }

        [StringLength(50)]
        [DisplayName("زمان عملیات انفرادی ")]

        public string IndOperTotalFinalOper { get; set; }
        public string DateInsert { get; set; }

        [DisplayName("   امتیاز نهایی   ")]

        public double? TotalScore { get; set; }

        public virtual Athlete Athlete { get; set; }
        public virtual Period Period { get; set; }

        public virtual ICollection<PhRedinessHistoryItem> PhRedinessHistoryItems { get; set; }
    }
}
