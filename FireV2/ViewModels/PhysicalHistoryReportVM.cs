using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FireV2.ViewModels
{
    public class PhysicalHistoryReportVM
    {
        public PhysicalHistoryReportVM()
        {
            Titles = new List<string>();
        }
        [Key]
        public int AthleteId { get; set; }

        public string PSazmaniTitle { get; set; }
        public string FatherName { get; set; }
        public int PhysicalExamHistoryId { get; set; }
        public bool Flag { get; set; }
        public int Reng { get; set; }
        public string Fnam { get; set; }
        public string StatusScore { get; set; }
        public string Lname { get; set; }
        public string Age { get; set; }
        public int PeriodId { get; set; }
        public string Note { get; set; }
        public int DissPerId { get; set; }
        public int DissTitleId { get; set; }

        [StringLength(50)]
        public string DissTitles { get; set; }
        [DisplayName("   مجموع امتیاز آمادگی جسمانی  ")]

        public double? TotalPhysicalScore { get; set; }

        [DisplayName("    مجموع زمان کل عملیات تیمی  ")]
        public string TotalTimeWorkTime { get; set; }

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

        [DisplayName("زمان عملیات انفرادی ")]

        public string IndOperTotalFinalOper { get; set; }

        [DisplayName("   امتیاز نهایی   ")]

        public string TitleAge { get; set; }
        public List<string> Titles { set; get; }
        public double? TotalScore { get; set; }

        public string AthleteName { get; set; }

        public string PeriodTitle { get; set; }

        public string Birthdate { get; set; }

        public string Picture { get; set; }
    }
}