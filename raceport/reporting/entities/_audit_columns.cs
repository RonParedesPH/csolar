using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace reporting.entities
{
    public class _audit_columns
    {
        [Column(TypeName = "datetime2")]
        public DateTime dtcreated { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime dtlastmodified { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime dterased { get; set; }
        [StringLength(64)]
        public string lastmodifiedby { get; set; }
    }
}
