using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PLMApp.Data;
using PLMApp.Models;

public class DatabaseSeeder
{
    private readonly PLMContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public DatabaseSeeder(PLMContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync()
    {
        await _context.Database.MigrateAsync();

        if (!_context.Roles.Any())
        {
            var adminRole = new Role { RoleName = "admin" };
            var clientRole = new Role { RoleName = "client" };

            await _context.Roles.AddRangeAsync(adminRole, clientRole);
            await _context.SaveChangesAsync();
        }

        var adminRoleEntity = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "admin");

        if (!_context.Users.Any(u => u.Email == "admin@example.com"))
        {
            var adminUser = new User 
            {
                Email = "admin@example.com",
                Name = "Admin User",
                PhoneNumber = "1234567890",
                Password = _passwordHasher.HashPassword(null, "Admin@123")
            };

            _context.Users.Add(adminUser);
            await _context.SaveChangesAsync();

            if (adminRoleEntity != null && !_context.UserRoles.Any(ur => ur.UserId == adminUser.Id && ur.RoleId == adminRoleEntity.Id))
            {
                var userRole = new UserRole
                {
                    UserId = adminUser.Id,
                    RoleId = adminRoleEntity.Id,
                    User = adminUser,
                    Role = adminRoleEntity
                };

                _context.UserRoles.Add(userRole);
                await _context.SaveChangesAsync();
            }
        }

        if (!_context.Stages.Any())
        {
            var stages = new List<Stage>
            {
                new Stage { Name = "Concept", Description = "Initial concept of the product" },
                new Stage { Name = "Feasibility", Description = "Feasibility study for the product" },
                new Stage { Name = "Design", Description = "Product design phase" },
                new Stage { Name = "Production", Description = "Production phase of the product" },
                new Stage { Name = "Release", Description = "Product released" },
                new Stage { Name = "Standby", Description = "Product on standby" },
                new Stage { Name = "Cancel", Description = "Product canceled" },
            };

            await _context.Stages.AddRangeAsync(stages);
            await _context.SaveChangesAsync();
        }
    }
}
