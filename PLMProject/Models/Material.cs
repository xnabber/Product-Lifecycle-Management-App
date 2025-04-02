using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PLMApp.Models
{
    public class Material
    {
        [Key]
        public required string MaterialNumber { get; set; }

        public decimal Height { get; set; }

        [MaxLength(500)]
        public string? MaterialDescription { get; set; }

        public decimal Weight { get; set; }
        public decimal Width { get; set; }
    }

    public class MaterialDto
    {
        public string MaterialNumber { get; set; }
        [Required]
        public decimal Height { get; set; }
        public string? MaterialDescription { get; set; }
        [Required]
        public decimal Weight { get; set; }
        [Required]
        public decimal Width { get; set; }

    }
}
