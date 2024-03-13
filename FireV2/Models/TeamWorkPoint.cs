namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TeamWorkPoint
    {
        [Key]
        public int TWorkPointsId { get; set; }

        [Required]
        [StringLength(10)]
        [DisplayName("   زمان عملیات تیمی   ")]

        public string Time { get; set; }

        [Required]
        [DisplayName("   امتیاز عملیات تیمی   ")]
        public int PackTeamid { get; set; }

        public double Points { get; set; }

    }
}
