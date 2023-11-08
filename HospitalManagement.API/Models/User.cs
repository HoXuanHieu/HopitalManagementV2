using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.API.Models
{
    [Table("User")]
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
        public bool Gender { get; set; }
        [MaxLength(100)]
        public string? Address { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public string? Token { get; set; }
        public bool IsEmailVerified { get; set; }
        public virtual List<Appointment>? Appointments { get; set; }
    }
}
