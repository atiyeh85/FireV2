namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public partial class Period
    {
        public Period()
        {
            BMIs = new HashSet<BMI>();
            Dissusions = new HashSet<Dissusion>();
            IndividualHistorys = new HashSet<IndividualHistory>();
            Medicals = new HashSet<Medical>();
            PhysicalEducations = new HashSet<PhysicalEducation>();
            PhysicalExamHistories = new HashSet<PhysicalExamHistory>();
            //RengAthleteInPeriods = new HashSet<RengAthleteInPeriod>();
            SportScoreByPeriods = new HashSet<SportScoreByPeriod>();
            TeamByPeriods = new HashSet<TeamByPeriod>();
        }
        public int PeriodId { get; set; }
        [Required]
        [DisplayName("   سال  ")]
        public int Year { get; set; }
        [StringLength(250)]
        [DisplayName("   عناوین دوره ")]
        public string SeasonTitle { get; set; }
        public string FromDate { get; set; }
        public string Todate { get; set; }
        [DisplayName("   دوره  ")]

        public int Season { get; set; }
        public string UserInsert { get; set; }
        public string DateInsert { get; set; }
        public int? PackIndid { get; set; }

        public int? PackTeamid { get; set; }
        public virtual PackPackInd PackPackInd { get; set; }

        public virtual PackPointTeam PackPointTeam { get; set; }
        public virtual ICollection<Dissusion> Dissusions { get; set; }
        public virtual ICollection<BMI> BMIs { get; set; }
        //public virtual ICollection<RengAthleteInPeriod> RengAthleteInPeriods { get; set; }

        public virtual ICollection<IndividualHistory> IndividualHistorys { get; set; }

        public virtual ICollection<TeamByPeriod> TeamByPeriods { get; set; }

        public virtual ICollection<PhysicalExamHistory> PhysicalExamHistories { get; set; }
        public virtual ICollection<PhysicalEducation> PhysicalEducations { get; set; }
        public virtual ICollection<Medical> Medicals { get; set; }
        public virtual ICollection<SportScoreByPeriod> SportScoreByPeriods { get; set; }

    }
}
