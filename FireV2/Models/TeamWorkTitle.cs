namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TeamWorkTitle
    {
        public TeamWorkTitle()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TWorkId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("   نام تیم   ")]

        public string TworkTitle { get; set; }

    }
}
