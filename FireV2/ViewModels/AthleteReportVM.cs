using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FireV2.ViewModels
{
    public class AthleteReportVM
    {
        public int AthleteId { get; set; }

        public string Birthdate { get; set; }
        public string Picture { get; set; }

        public string Name { get; set; }

        public double? TotalScore { get; set; }
    }
}