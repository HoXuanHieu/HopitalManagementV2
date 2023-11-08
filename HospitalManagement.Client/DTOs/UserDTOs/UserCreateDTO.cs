using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Client.DTOs.UserDTOs
{
    public class UserCreateDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
        public bool Gender { get; set; }
        [MaxLength(100)]
        public string? Address { get; set; }
        public string RoleName { get; set; }
    }
}
