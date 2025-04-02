using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLMApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        public required string Email { get; set; }

        [MaxLength(200)]
        public required string Name { get; set; }

        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        public required string Password { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
