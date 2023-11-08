using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.API.Models
{
    public class Hospital
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public virtual List<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
