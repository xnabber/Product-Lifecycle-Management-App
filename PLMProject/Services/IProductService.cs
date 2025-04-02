using PLMApp.Models;

namespace PLMProject.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int productId);
        Task<Product?> AddProductAsync(ProductRequestDto product);
        Task<Product?> UpdateProductAsync(ProductRequestDto product);
        Task<IDictionary<string, int>> GetProductsGroupedByCurrentStageAsync();
        Task<List<StageDurationDto>> GetProductStageDurationsAsync(int productId);
        Task<bool> DeleteAsync(int productId);
        Task<List<ProjectStatistics>> GetDetailedProductStatisticsAsync();
        Task<ProjectStatistics?> GetDetailedProductStatisticsByProjectIdAsync(int projectId);
    }
}
