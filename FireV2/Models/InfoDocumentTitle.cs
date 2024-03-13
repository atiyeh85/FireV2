namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InfoDocumentTitle
    {
        public InfoDocumentTitle()
        {
            InfoDocumentTitleTypes = new HashSet<InfoDocumentTitleType>();
        }

        [Key]
        public int InfoDocTitleId { get; set; }

        [Required]
        [StringLength(50)]
        public string InfoDocTitle { get; set; }

        public virtual ICollection<InfoDocumentTitleType> InfoDocumentTitleTypes { get; set; }
    }
}
