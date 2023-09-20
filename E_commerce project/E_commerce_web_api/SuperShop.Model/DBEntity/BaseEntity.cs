using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SuperShop.Model.DBEntity
{
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        public Int64? CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreatedDate { get; set; }

        [ScaffoldColumn(false)]
        public Int64? UpdatedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? UpdatedDate { get; set; }

        [ScaffoldColumn(false)]
        public bool IsRemoved { get; set; }

        [ScaffoldColumn(false)]
        public bool IsActive { get; set; }
    }
}
