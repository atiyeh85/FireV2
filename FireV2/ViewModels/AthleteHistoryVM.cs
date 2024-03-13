using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FireV2.ViewModels
{
    public class AthleteHistoryVM
    {

        
        public bool IndividualOperations { get; set; }
        public bool PhysicalReadiness { get; set; }
        public bool SportsScore { get; set; }
        public bool Flag { get; set; }

        public bool DissUsionPeriod { get; set; }
        public bool Teamwork { get; set; }
        public bool IndDis { get; set; }
        public bool PhDis { get; set; }
        public bool TeamDis { get; set; }
        public bool FullDis { get; set; }
        public int AthleteId { get; set; }
        public int PeriodId { get; set; }
        public string Picture { get; set; }

        
    }
}