namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DocTypeAthelet
    {
        [Key]
        public int DocTypeAtId { get; set; }

        public int? AthleteId { get; set; }

        public int? InfoDocTitleTypeId { get; set; }

        public string Note { get; set; }

        public virtual Athlete Athlete { get; set; }

        public virtual InfoDocumentTitleType InfoDocumentTitleType { get; set; }
    }
}
