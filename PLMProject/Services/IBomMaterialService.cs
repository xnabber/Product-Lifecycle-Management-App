using PLMApp.Models;

namespace PLMProject.Services
{
    public interface IBomMaterialService
    {
        Task<IEnumerable<BomMaterial>> GetAllAsync();
        Task<BomMaterial?> GetByIdAsync(int bomId, string materialId);
        Task<BomMaterial?> AddBomMaterialAsync(BomMaterialDto bomMaterial);
        Task<BomMaterial?> UpdateBomMaterialAsync(BomMaterialDto bomMaterial);
        Task<IEnumerable<BomMaterial>> GetByBom(int bomId);
        Task<bool> DeleteAsync(int bomId, string materialId);
    }
}
