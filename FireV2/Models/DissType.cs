namespace FireV2.Models
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DissType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DissType()
        {
            DissuasionsTitles = new HashSet<DissuasionsTitle>();
        }

        public int DissTypeid { get; set; }

        [StringLength(50)]
        public string DissTypeTitle { get; set; }
        public virtual ICollection<DissuasionsTitle> DissuasionsTitles { get; set; }
    }
}
