using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.API.DTOs.UserDTOs
{
    public class UserLoginDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}