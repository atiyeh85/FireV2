namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SportsScoreTitle
    {
        public SportsScoreTitle()
        {
            SportScoreByPeriods = new HashSet<SportScoreByPeriod>();
        }

        [Key]
        public int SScoreId { get; set; }

        [StringLength(50)]
        [DisplayName("    عناوین قهرمانی  ")]

        public string SScoreTitle { get; set; }

        public virtual ICollection<SportScoreByPeriod> SportScoreByPeriods { get; set; }
    }
}
