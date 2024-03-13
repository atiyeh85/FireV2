namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PhysicalReadinesitem
    {
        public PhysicalReadinesitem()
        {
            PhReadinessItemByPeriods = new HashSet<PhReadinessItemByPeriod>();
            PhRedinessHistoryItems = new HashSet<PhRedinessHistoryItem>();
            phRItemsPoints = new HashSet<phRItemsPoint>();
        }

        [Key]
        public int PhReadinessItemId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("   عناوین آمادگی جسمانی   ")]

        public string PhReadinessItemTitle { get; set; }

        public virtual ICollection<PhReadinessItemByPeriod> PhReadinessItemByPeriods { get; set; }

        public virtual ICollection<PhRedinessHistoryItem> PhRedinessHistoryItems { get; set; }

        public virtual ICollection<phRItemsPoint> phRItemsPoints { get; set; }
    }
}
