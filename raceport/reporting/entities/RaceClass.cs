using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace reporting.entities
{
    [Table("Class")]
    public class RaceClass : _audit_columns
    {
        [Key]
        [StringLength(32)]
        //public Guid Id { get; set; }
        public string Id { get; set; } =  string.Empty;
        [Required]
        [StringLength(32)]
        public string Code { get; set; } = string.Empty;
        [StringLength(420)]
        public string Description { get; set; } = string.Empty;

        // Navigation Property
        //public virtual List<RaceDriver> Members { get; set; }
    }
}
