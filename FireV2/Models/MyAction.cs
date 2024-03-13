namespace FireV2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MyAction
    {
        public int MyActionID { get; set; }

        [StringLength(50)]
        public string ActionName { get; set; }

        public virtual IEnumerable<RoleAction> RoleActions { get; set; }

        public MyAction()
        {
            RoleActions = new List<RoleAction>();

        }
    }
}
