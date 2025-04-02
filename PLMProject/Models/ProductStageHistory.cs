using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLMApp.Models
{
    public class ProductStageHistory
    {
        [Key]
        [Column(Order = 0)]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public required Product Product { get; set; }

        [Key]
        [Column(Order = 1)]
        public int StageId { get; set; }

        [ForeignKey("StageId")]
        public required Stage Stage { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime StartOfStage { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }
    }

    public class ProductStageHistoryDto
    {
        public int ProductId { get; set; }
        public int StageId { get; set; }
        public required string StartOfStage { get; set; }
        public int UserId { get; set; }
    }
}
