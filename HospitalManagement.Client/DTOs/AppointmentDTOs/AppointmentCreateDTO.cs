using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Client.DTOs.AppointmentDTOs
{
    public class AppointmentCreateDTO
    {
        public int? UserId { get; set; }
        public int? DoctorId { get; set; }
        [Required]
        public string Symptoms { get; set; }
    }
}
