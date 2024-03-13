namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Occupationalfield
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OccupationalfieldId { get; set; }

        [Required]
        public string OccupationalfieldTitle { get; set; }
    }
}
