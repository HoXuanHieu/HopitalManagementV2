using HospitalManagement.Client.DTOs.HospitalDTOs;
using HospitalManagement.Client.DTOs.UserDTOs;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Client.DTOs.DoctorDTOs
{
    public class DoctorDTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public virtual UserDTO User { get; set; }
        public string? Description { get; set; }
        [Required]
        public int HospitalId { get; set; }
        public virtual HospitalDTO Hospital { get; set; }
    }
}
