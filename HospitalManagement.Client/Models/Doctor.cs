using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Client.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        [Required, ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        public string? Description { get; set; }
        [Required]
        public int HospitalId { get; set; }
        public virtual Hospital? Hospital { get; set; }
        public virtual List<Appointment>? Appointments { get; set; }
    }
}