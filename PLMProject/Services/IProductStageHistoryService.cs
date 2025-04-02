using PLMApp.Models;

namespace PLMApp.Services
{
    public interface IProductStageHistoryService
    {
        Task<IEnumerable<ProductStageHistory>> GetAllAsync();
        Task<ProductStageHistory?> GetByIdAsync(int productId, int stageId, DateTime startOfStage);
        Task<ProductStageHistory?> AddStageTransitionAsync(ProductStageHistoryDto productStageHistory);
        Task<bool> DeleteAsync(int productId, int stageId, DateTime startOfStage);
    }
}
