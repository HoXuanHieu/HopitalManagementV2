using HospitalManagement.API.DTOs.UserDTOs;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.API.DTOs.DoctorDTOs
{
    public class DoctorUpdateDTO
    {
        public UserUpdateDTO User { get; set; }
        public string? Description { get; set; }
        [Required]
        public int HospitalId { get; set; }
    }
}
