using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace reporting.entities
{
    public class Racer : _audit_columns
    {
        [Key]
        [Required]
        //public Guid Id { get; set; }
        [StringLength(32)]
        public string Id { get; set; }
        [Required]
        [StringLength(256)] 
        public string Name { get; set; }
        public int Number {  get; set; }
        [StringLength(256)]
        public string CarDetails { get; set; }
        [StringLength(32)]
        public string RaceClass {  get; set; }
        [StringLength(420)]
        public string Notes { get; set; }

        // Foreign Key
        //public Guid? TeamIdOptional { get; set; }
        [StringLength(32)]
        public string TeamIdOptional { get; set; }
        // Navigation Prop
        public virtual List<Registration> Participations { get; set; }
    }
}
