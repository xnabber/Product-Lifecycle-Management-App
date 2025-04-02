using PLMApp.Models;

namespace PLMProject.Services
{
    public interface IUserRoleService
    {
        Task<IEnumerable<UserRole>> GetAllAsync();
        Task<UserRole?> GetByIdAsync(int userId, int roleId);
        Task<bool> AddUserRoleAsync(UserRole userRoles);
        Task<bool> UpdateUserRoleAsync(UserRole userRoles);
        Task<bool> DeleteAsync(int userId, int roleId);
    }
}
