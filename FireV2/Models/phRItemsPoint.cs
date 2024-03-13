namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class phRItemsPoint
    {
        public int phRItemsPointId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("    زمان عملیات آمادگی جسمانی ")]

        public string Time { get; set; }

        [Required]
        [DisplayName("    امتیاز عملیات آمادگی جسمانی ")]

        public double Point { get; set; }
        [DisplayName("   رنج سنی ")]

        public int AgeRengeId { get; set; }

        public int? PhReadinessItemId { get; set; }

        public virtual AgeRenge AgeRenge { get; set; }

        public virtual PhysicalReadinesitem PhysicalReadinesitem { get; set; }
    }
}
