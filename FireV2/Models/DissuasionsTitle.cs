namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DissuasionsTitle
    {
        public DissuasionsTitle()
        {
            Dissusions = new HashSet<Dissusion>();
        }
        [Key]
        public int DissTitleId { get; set; }

        [StringLength(50)]
        public string DissTitles { get; set; }
        public int? DissTypeid { get; set; }

        public virtual DissType DissType { get; set; }
        public virtual ICollection<Dissusion> Dissusions { get; set; }

    }
}
