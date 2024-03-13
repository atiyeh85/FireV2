using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace FireV2.ViewModels
{
    public class ListAtheletevm
    {

        public int AthleteId { get; set; }
        public string Picture { get; set; }
        public bool Flag { get; set; }

        public string FullName { get; set; }
        public string Age { get; set; }
        [DisplayName("    مدرک تحصیلی")]
        public string DegreeName { get; set; } 
        [DisplayName("   پست سازمانی   ")]
        public string PSazmaniTitle { get; set; }
        public string GoorohKhoni { get; set; }
        public string PersonalCode { get; set; }
        public string Expertise { get; set; }
        public string ShWorkTitle { get; set; }
        public string ss { get; set; }
        public string StationTitle { get; set; }

    }
}