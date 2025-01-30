using System.ComponentModel.DataAnnotations;

namespace reporting.entities
{
    public class Team : _audit_columns
    {
        [Key]
        [StringLength(32)]
        //public Guid Id { get; set; }
        public string Id { get; set; }
        [Required]
        [StringLength(128)]
        public string Name { get; set; }
        [StringLength(420)]
        public string Description { get; set; }

        // Navigation Property
        //public virtual List<racepository> Members { get; set; }
    }
}
