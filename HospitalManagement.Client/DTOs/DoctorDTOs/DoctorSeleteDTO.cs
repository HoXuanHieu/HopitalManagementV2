using HospitalManagement.Client.DTOs.HospitalDTOs;
using HospitalManagement.Client.DTOs.UserDTOs;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Client.DTOs.DoctorDTOs
{
    public class DoctorSeleteDTO
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string? Description { get; set; }
    }
}
