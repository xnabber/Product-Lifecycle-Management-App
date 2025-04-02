using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PLMApp.Data;
using PLMApp.Models;

namespace PLMProject.Services
{
    public class UserService : IUserService
    {
        private readonly PLMContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        public UserService(PLMContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public async Task<bool> AddUserAsync(string email, string name, string password, string? phoneNumber)
        {
            var user = new User
            {
                Email = email,
                Name = name,
                Password = _passwordHasher.HashPassword(null, password),
                PhoneNumber = phoneNumber
            };
            var newUser = _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> Authenticate(string email, string password)
        {
            var user = await _context.Users.Include(x => x.UserRoles).ThenInclude(ur => ur.Role).FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result == PasswordVerificationResult.Success)
            {
                return user;
            }

            return null;
        }

        public async Task<bool> DeleteAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await _context.Users.Include(x => x.UserRoles).ToListAsync();
            return users;
        }
        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users.Include(x => x.UserRoles).FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.Include(x => x.UserRoles).FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<bool> UpdateUserAsync(User user)
        {
            var oldUser = await _context.Users.FindAsync(user.Id);
            if (oldUser == null) return false;
            oldUser.Name = user.Name;
            oldUser.Email = user.Email;
            oldUser.PhoneNumber = user.PhoneNumber;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
