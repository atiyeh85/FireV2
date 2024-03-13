namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Unjustified
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UnJustifiedId { get; set; }

        [Required]
        public string UnJustifiedTitle { get; set; }
    }
}
