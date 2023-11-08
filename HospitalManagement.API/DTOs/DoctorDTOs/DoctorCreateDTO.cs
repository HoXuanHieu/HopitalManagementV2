using HospitalManagement.API.DTOs.UserDTOs;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.API.DTOs.DoctorDTOs
{
    public class DoctorCreateDTO
    {
        [Required]
        public UserCreateDTO User { get; set; }
        public string? Description { get; set; }
        [Required]
        public int HospitalId { get; set; }
    }
}
