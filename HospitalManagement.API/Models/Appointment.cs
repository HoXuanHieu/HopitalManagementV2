using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.API.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string? Symptoms { get; set; }
        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        [Required]
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }

    }
}
