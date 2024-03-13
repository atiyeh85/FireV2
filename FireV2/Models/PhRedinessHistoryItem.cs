namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PhRedinessHistoryItem
    {
        [Key]
        public int PhRHistoryItemId { get; set; }

        public int PhysicalExamHistoryId { get; set; }

        public int PhReadinessItemId { get; set; }
        [DisplayName("    زمان عملیات آمادگی جسمانی ")]

        public string Time { get; set; }
        [DisplayName("    امتیاز عملیات آمادگی جسمانی ")]

        public double Score { get; set; }

        public virtual PhysicalExamHistory PhysicalExamHistory { get; set; }

        public virtual PhysicalReadinesitem PhysicalReadinesitem { get; set; }
    }
}
