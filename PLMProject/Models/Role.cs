using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PLMApp.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public required string RoleName { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    }
}
