using Microsoft.EntityFrameworkCore;
using PLMApp.Data;
using PLMApp.Models;

namespace PLMProject.Services
{
    public class RoleService : IRoleService
    {
        private readonly PLMContext _context;
        public RoleService(PLMContext context)
        {
            _context = context;
        }
        public async Task<bool> AddRoleAsync(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null) return false;
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            var roles = await _context.Roles.ToListAsync();
            return roles;
        }
        public async Task<Role?> GetByIdAsync(int roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }
        public async Task<bool> UpdateRoleAsync(Role role)
        {
            var oldRole = await _context.Roles.FindAsync(role.Id);
            if (oldRole == null) return false;
            oldRole.RoleName = role.RoleName;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
