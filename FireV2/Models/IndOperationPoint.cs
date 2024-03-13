namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class IndOperationPoint
    {

        public int IndOperationPointId { get; set; }

        [Required]
        [DisplayName("   زمان عملیات انفرادی")]

        [StringLength(50)]
        public string Time { get; set; }
        [DisplayName("  امتیاز عملیات انفرادی")]
        public int PackIndId { get; set; }

        public double Points { get; set; }

    }
}
