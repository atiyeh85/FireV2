namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class IsAbsent
    {
        [Key]
        public int AbsId { get; set; }

        [StringLength(50)]
        [DisplayName(" حضور/ غیاب ")]

        public string IsAbsentTitle { get; set; }
    }
}
