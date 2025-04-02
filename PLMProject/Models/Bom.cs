using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PLMApp.Models
{
    public class Bom
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        public required string Name { get; set; }

        public virtual ICollection<BomMaterial> BomMaterials { get; set; } = new List<BomMaterial>();
    }
}
