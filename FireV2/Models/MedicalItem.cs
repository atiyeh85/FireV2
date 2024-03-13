namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MedicalItem
    {
        public MedicalItem()
        {
            MedicalTitleTypes = new HashSet<MedicalTitleType>();
        }

        public int MedicalItemid { get; set; }

        [StringLength(50)]
        public string MItem { get; set; }
        public int? TypeOfTestId { get; set; }

        public virtual TypeOfTest TypeOfTest { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MedicalTitleType> MedicalTitleTypes { get; set; }
    }
}
