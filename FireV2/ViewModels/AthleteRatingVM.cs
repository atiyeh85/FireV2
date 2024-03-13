using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FireV2.ViewModels
{
    public class AthleteRatingVM
    {
        public int minAge { get; set; }
        public int maxAge { get; set; }

        public double minTotalScore { get; set; }
        public double maxTotalScore { get; set; }

        public string Rate { get; set; }

    }
}