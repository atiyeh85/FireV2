namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PhysicalEducationItem
    {
        public PhysicalEducationItem()
        {
            PhysicalEducations = new HashSet<PhysicalEducation>();
        }

        [Key]
        public int PhyEduItemId { get; set; }

        public string PhyEducationTitle { get; set; }

        public virtual ICollection<PhysicalEducation> PhysicalEducations { get; set; }
    }
}
