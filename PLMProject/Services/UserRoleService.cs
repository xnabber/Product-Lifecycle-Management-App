using Microsoft.EntityFrameworkCore;
using PLMApp.Data;
using PLMApp.Models;

namespace PLMProject.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly PLMContext _context;
        public UserRoleService(PLMContext context)
        {
            _context = context;
        }
        public async Task<bool> AddUserRoleAsync(UserRole userRoles)
        {
            var user = await _context.Users.FindAsync(userRoles.UserId);
            if (user == null) return false;
            var role = await _context.Roles.FindAsync(userRoles.RoleId);
            if (role == null) return false;
            _context.UserRoles.Add(userRoles);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int userId, int roleId)
        {
            var userRoles = await _context.UserRoles.FindAsync(roleId,userId);
            if (userRoles == null) return false;
            _context.UserRoles.Remove(userRoles);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserRole>> GetAllAsync()
        {
            var userRoles = await _context.UserRoles.ToListAsync();
            return userRoles;
        }
        public async Task<UserRole?> GetByIdAsync(int userId, int roleId)
        {
            return await _context.UserRoles.FindAsync(roleId, userId);
        }
        public async Task<bool> UpdateUserRoleAsync(UserRole userRoles)
        {
            var user = await _context.Users.FindAsync(userRoles.UserId);
            if (user == null) return false;
            var role = await _context.Roles.FindAsync(userRoles.RoleId);
            if (role == null) return false;
            var oldUserRole = await _context.UserRoles.FindAsync(userRoles.RoleId, userRoles.UserId);
            if (oldUserRole == null) return false;
            oldUserRole.UserId = userRoles.UserId;
            oldUserRole.RoleId = userRoles.RoleId;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
