namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class IndividualOperationItem
    {
        public IndividualOperationItem()
        {
            IndivisualOperationHistoryItems = new HashSet<IndivisualOperationHistoryItem>();
            IndOperationItemsByPeriods = new HashSet<IndOperationItemsByPeriod>();
        }

        [Key]
        public int IndOperationItemId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName(" عنوان عملیات انفرادی ")]

        public string IndOperationTitle { get; set; }

        public virtual ICollection<IndivisualOperationHistoryItem> IndivisualOperationHistoryItems { get; set; }

        public virtual ICollection<IndOperationItemsByPeriod> IndOperationItemsByPeriods { get; set; }

    }
}
