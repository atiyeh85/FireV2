namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EditTable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EditTableid { get; set; }

        [StringLength(50)]
        public string UserEdit { get; set; }

        [StringLength(50)]
        public string DateEdit { get; set; }

        [StringLength(50)]
        public string TimeEdit { get; set; }

        [StringLength(50)]
        public string FormEdit { get; set; }

        public int? AthleteId { get; set; }
    }
}
