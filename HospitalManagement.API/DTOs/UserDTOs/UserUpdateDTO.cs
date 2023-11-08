using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.API.DTOs.UserDTOs
{
    public class UserUpdateDTO
    {
        [Required]
        public string FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
        public bool Gender { get; set; }
        [MaxLength(100)]
        public string? Address { get; set; }
    }
}
