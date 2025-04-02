using PLMApp.Models;

namespace PLMProject.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int userId);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> AddUserAsync(string email, string name, string password, string? phoneNumber);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteAsync(int userId);
        Task<User?> Authenticate(string username, string password);
        Task<bool> UserExistsAsync(string email);
    }
}
