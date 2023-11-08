using HospitalManagement.Client.DTOs.UserDTOs;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Client.DTOs.DoctorDTOs
{
    public class DoctorUpdateDTO
    {
        public UserUpdateDTO User { get; set; }
        public string? Description { get; set; }
        [Required]
        public int HospitalId { get; set; }
    }
}
