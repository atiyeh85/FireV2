namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeType")]
    public partial class EmployeType
    {
        public EmployeType()
        {
            Athletes = new HashSet<Athlete>();
        }

        [Key]
        public int EmTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string EmTypeTitle { get; set; }

        public virtual ICollection<Athlete> Athletes { get; set; }
    }
}
