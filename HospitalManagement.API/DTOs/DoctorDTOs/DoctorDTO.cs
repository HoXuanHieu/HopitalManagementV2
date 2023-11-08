using HospitalManagement.API.DTOs.HospitalDTOs;
using HospitalManagement.API.DTOs.UserDTOs;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.API.DTOs.DoctorDTOs
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
