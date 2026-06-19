using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CamZone.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public int Age { get; set; }

        // Thêm 2 cái này
        public string? NgheNghiep { get; set; }
        public string? GioiTinh { get; set; }
    }
}
