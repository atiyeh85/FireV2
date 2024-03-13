namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FileUpload
    {
        [Key]
        public int FileUploadid { get; set; }

        public int Medicalid { get; set; }

        [StringLength(500)]
        public string AddressFile { get; set; }

        [StringLength(50)]
        public string DateUpload { get; set; }

        [StringLength(50)]
        public string UserUpload { get; set; }

        public virtual Medical Medical { get; set; }
    }
}
