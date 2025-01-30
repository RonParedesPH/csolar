using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace reporting.entities
{
    public class Round : _audit_columns
    {
        [Key]
        //public Guid Id { get; set; }
        [StringLength(32)]
        public string Id { get; set; }
        [Required]
        [StringLength(128)]
        public string Name { get; set; } = string.Empty;
        [StringLength(420)] 
        public string Description { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime EventDate { get; set; }
        [StringLength(128)]
        public string Venue { get; set; }

        // Foreign Keys
        //public Guid SeasonId { get; set; }
        [StringLength(32)]
        public string SeasonId { get; set; }

        // Navigation
        public virtual Season Season { get; set; }
        public virtual List<Registration> Participants { get; set; }
    }
}
