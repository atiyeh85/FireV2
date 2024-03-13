namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TypeOfTest
    {
        public TypeOfTest()
        {
            Medicals = new HashSet<Medical>();
            MedicalItems = new HashSet<MedicalItem>();

        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TypeOfTestId { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }
        public virtual ICollection<Medical> Medicals { get; set; }
        public virtual ICollection<MedicalItem> MedicalItems { get; set; }
    }
}
