namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("IndividualHistorys")]
    public partial class IndividualHistory
    {
        public IndividualHistory()
        {
            IndivisualOperationHistoryItems = new HashSet<IndivisualOperationHistoryItem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IndHistoryId { get; set; }

        public int AthleteId { get; set; }

        [StringLength(50)]
        [DisplayName("    مجموع زمان عملیات انفرادی ")]
        public string IndOperTotalTime { get; set; }

        [StringLength(50)]
        [DisplayName("    زمان خطا")]
        public string IndOperTotalFaults { get; set; }

        [StringLength(50)]
        [DisplayName("    مجموع زمان کل")]
        public string IndOperTotalFinalOper { get; set; }
        public string DateInsert { get; set; }
        public string TimeInsert { get; set; }
        public string UserInsert { get; set; }

        [DisplayName("    امتیاز  عملیات انفرادی ")]
        public double? IndOperScore { get; set; }

        public int PeriodId { get; set; }

        public virtual Athlete Athlete { get; set; }

        public virtual Period Period { get; set; }

        public virtual ICollection<IndivisualOperationHistoryItem> IndivisualOperationHistoryItems { get; set; }
    }
}
