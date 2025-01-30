using System.ComponentModel.DataAnnotations;

namespace reporting.entities
{
    public class Registration : _audit_columns
    {
        [Key]
        [StringLength(32)]
        //public Guid Id { get; set; }
        public string Id { get; set; }
        [Required]
        [StringLength(32)]
        public string RaceClass { get; set; }

        // Foreign Keys
        [Required]
        [StringLength(32)]
        //public Guid DriverId { get; set; }
        public string RacerId { get; set; }
        [Required]
        [StringLength(32)]
        //public Guid RoundId { get; set; }  
        public string RoundId { get; set; }
        
        // Navigation
        public virtual Racer Participant { get; set; }
        public virtual Round Round { get; set; }
    
    }
}
