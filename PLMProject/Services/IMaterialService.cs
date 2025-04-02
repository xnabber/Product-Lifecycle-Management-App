using PLMApp.Models;

namespace PLMProject.Services
{
    public interface IMaterialService
    {
        Task<IEnumerable<Material>> GetAllAsync();
        Task<Material?> GetByIdAsync(int materialId);
        Task<Material> AddMaterialAsync(MaterialDto material);
        Task<Material?> UpdateMaterialAsync(MaterialDto material);
        Task<bool> DeleteAsync(int materialId);
    }
}
