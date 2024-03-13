namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserAction
    {
        [Key]
        [Column(Order = 0)]
        public string UserName { get; set; }

        [Key]
        [Column(Order = 1)]
        public int MyActionID { get; set; }
        public virtual MyAction MyAction { get; set; }
        //user relation
        //public virtual Users AspNetRole { get; set; }
    }
}
