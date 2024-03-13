namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class IndivisualOperationHistoryItem
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IndHistoryId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IndOperationItemId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("     زمان عملیات انفرادی ")]

        public string IndOperationTime { get; set; }

        public virtual IndividualHistory IndividualHistory { get; set; }

        public virtual IndividualOperationItem IndividualOperationItem { get; set; }
    }
}
