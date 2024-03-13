namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Archive
    {
        [Key]
        public int IsArchiveid { get; set; }

        [StringLength(50)]
        public string ArchiveNote { get; set; }

        [StringLength(50)]
        public string Date { get; set; }
        public string UsetArchive { get; set; }
        public string Time { get; set; }
        public bool IsArchive { get; set; }
        public int AthleteId { get; set; }
        public virtual Athlete Athlete { get; set; }
    }
}
