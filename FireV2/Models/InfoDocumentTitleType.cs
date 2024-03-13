namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InfoDocumentTitleType
    {
        public InfoDocumentTitleType()
        {
            DocTypeAthelets = new HashSet<DocTypeAthelet>();
        }

        [Key]
        public int InfoDocTitleTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string InfoDocTitleType { get; set; }

        public int InfoDocTitleId { get; set; }

        public virtual ICollection<DocTypeAthelet> DocTypeAthelets { get; set; }

        public virtual InfoDocumentTitle InfoDocumentTitle { get; set; }
    }
}
