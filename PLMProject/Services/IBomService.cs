using PLMApp.Models;

namespace PLMProject.Services
{
    public interface IBomService
    {
        Task<IEnumerable<Bom>> GetAllAsync();
        Task<Bom?> GetByIdAsync(int bomId);
        Task<Bom> AddBomAsync(string name);
        Task<bool> UpdateBomAsync(Bom bom);
        Task<bool> DeleteAsync(int bomId);
    }
}
