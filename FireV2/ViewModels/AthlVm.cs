using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace FireV2.ViewModels
{
    public class AthlVm
    {
        public int AthleteId { get; set; }

        public string PSazmaniTitle { get; set; }
        public string FatherName { get; set; }
        public int PeriodId { get; set; }

        [DisplayName("   مجموع امتیاز آمادگی جسمانی  ")]

        public double? TotalPhysicalScore { get; set; }
    }
}