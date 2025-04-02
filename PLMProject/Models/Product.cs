using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLMApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("BomId")]
        public required Bom Bom { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public decimal EstimatedHeight { get; set; }
        public decimal EstimatedWeight { get; set; }
        public decimal EstimatedWidth { get; set; }

        [MaxLength(200)]
        public required string Name { get; set; }

        public virtual ICollection<ProductStageHistory> ProductStageHistory { get; set; } = new List<ProductStageHistory>();
    }

    public class ProductRequestDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        public decimal EstimatedHeight { get; set; }

        [Required]
        public decimal EstimatedWeight { get; set; }

        [Required]
        public decimal EstimatedWidth { get; set; }

        [Required]
        public int BomId { get; set; }
    }
}

public class StageDurationDto
{
    public required string StageName { get; set; }
    public int DaysInStage { get; set; }
}

public class ProjectStatistics
{
    // Basic Product Information
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductDescription { get; set; }
    public decimal EstimatedHeight { get; set; }
    public decimal EstimatedWeight { get; set; }
    public decimal EstimatedWidth { get; set; }
    public string? BOMName { get; set; }

    // BOM and Material Information
    public List<BOMMaterialStatistics> BOMMaterials { get; set; } = new List<BOMMaterialStatistics>();

    // Stage History Information
    public List<StageHistoryStatistics> StageHistory { get; set; } = new List<StageHistoryStatistics>();

    // Current Stage Information
    public string? CurrentStage { get; set; }
    public int TotalMaterialQuantity { get; set; }
}

public class BOMMaterialStatistics
{
    public string? MaterialNumber { get; set; }
    public string? MaterialDescription { get; set; }
    public decimal Qty { get; set; }
    public string? UnitMeasureCode { get; set; }
}

public class StageHistoryStatistics
{
    public string? StageName { get; set; }
    public DateTime StartOfStage { get; set; }
    public string? UserName { get; set; }
    public int Duration { get; set; }  // Duration in days
}
