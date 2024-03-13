using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FireV2.ViewModels
{
    public class MedicalVm
    {
        public MedicalVm()
        {
            Titles = new List<string>();

        }
        public int Medicalid { get; set; }
        public List<string> Titles { set; get; }
        public string MItem { get; set; }
        public string DateInsert { get; set; }
        public string UserInsert { get; set; }
        public int AthleteId { get; set; }
        
        public string Type { get; set; }

        public int PeriodId { get; set; }
        public string SeasonTitle { get; set; }
        [StringLength(500)]
        public string Note { get; set; }
        public int? TypeOfTestId { get; set; }
        public int count { get; set; }
    }
}