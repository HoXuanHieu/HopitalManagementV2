using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.API.DTOs.UserDTOs
{
    public class UserRegisterDTO
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
