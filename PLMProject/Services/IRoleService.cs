using PLMApp.Models;

namespace PLMProject.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role?> GetByIdAsync(int roleId);
        Task<bool> AddRoleAsync(Role role);
        Task<bool> UpdateRoleAsync(Role role);
        Task<bool> DeleteAsync(int roleId);
    }
}
