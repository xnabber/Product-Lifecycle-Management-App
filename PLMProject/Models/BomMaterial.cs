using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PLMApp.Models
{
    public class BomMaterial
    {
        [Key, Column(Order = 0)]
        public int BomId { get; set; }

        [Key, Column(Order = 1)]
        public required string MaterialNumber { get; set; }

        public decimal Qty { get; set; }

        [MaxLength(50)]
        public string UnitMeasureCode { get; set; } = null!;

        [ForeignKey(nameof(BomId))]
        public required Bom Bom { get; set; }

        [ForeignKey(nameof(MaterialNumber))]
        public required Material Material { get; set; }
    }

    public class BomMaterialDto
    {
        public int BomId { get; set; }
        public required string MaterialNumber { get; set; }
        public decimal Qty { get; set; }
        public required string UnitMeasureCode{ get; set; }
    }
}
