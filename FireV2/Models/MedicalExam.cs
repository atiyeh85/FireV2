namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MedicalExam
    {
        public MedicalExam()
        {
            MRecords = new HashSet<MRecord>();
        }

        public int MedicalExamId { get; set; }

        [Required]
        [DisplayName("   عنوات تست ")]

        public string MExamTitle { get; set; }

        [Required]
        [DisplayName("   توضیحات  ")]

        public string MExamDescription { get; set; }

        public virtual ICollection<MRecord> MRecords { get; set; }
    }
}
