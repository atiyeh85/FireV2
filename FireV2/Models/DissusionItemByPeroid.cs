namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DissusionItemByPeroid
    {
        [Key]
        public int DissByPeriodId { get; set; }

        public int DissPerId { get; set; }

        public int DissTitleId { get; set; }

        public virtual DissuasionsTitle DissuasionsTitle { get; set; }

        public virtual Dissusion Dissusion { get; set; }
    }
}
