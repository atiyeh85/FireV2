namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EmployeeStatu
    {
        public EmployeeStatu()
        {
            Athletes = new HashSet<Athlete>();
        }

        [Key]
        public int EmployeeStatusId { get; set; }

        [Required]
        [DisplayName("    نوع  استخدام")]

        public string EmStatusTitle { get; set; }

        public virtual ICollection<Athlete> Athletes { get; set; }
    }
}
