namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Season
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SeasonId { get; set; }

        [Required]
        [StringLength(50)]
        public string SeasonTitle { get; set; }


    }
}
