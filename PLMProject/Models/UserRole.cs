﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLMApp.Models
{
    public class UserRole
    {
        [Key, Column(Order = 0)]
        public int RoleId { get; set; }

        [Key, Column(Order = 1)]
        public int UserId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role? Role { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
