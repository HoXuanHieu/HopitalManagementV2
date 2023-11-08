using HospitalManagement.API.DTOs.DoctorDTOs;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.API.DTOs.HospitalDTOs
{
    public class HospitalDetails
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public virtual List<DoctorDTO> Doctors { get; set; } = new List<DoctorDTO>();
    }
}
