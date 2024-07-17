using System;
using System.Collections.Generic;

namespace WPF_Project.Models
{
    public partial class staff
    {
        public int StaffId { get; set; }
        public string? Address { get; set; }
        public long? Phone { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; } = null!;
        public string Username { get; set; } = null!;
    }
}
