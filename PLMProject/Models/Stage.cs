using System.ComponentModel.DataAnnotations;

namespace PLMApp.Models
{
    public class Stage
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(200)]
        public required string Name { get; set; }
    }
}
