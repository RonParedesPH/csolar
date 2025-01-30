using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace reporting.entities
{
    public class Season : _audit_columns
    {
        [Key]
        [StringLength(32)]
        //public Guid Id { get; set; }
        public string Id { get; set; }
        [Required]
        [StringLength(128)]
        public string Title { get; set; }
        [StringLength(420)]
        public string Description { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime Start { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime Finish { get; set; }

        // Navigation
        public virtual List<Round> Rounds { get; set; }
    }
}
