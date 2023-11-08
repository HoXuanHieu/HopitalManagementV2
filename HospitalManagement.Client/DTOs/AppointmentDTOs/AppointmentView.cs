using HospitalManagement.Client.DTOs.UserDTOs;

namespace HospitalManagement.Client.DTOs.AppointmentDTOs
{
    public class AppointmentView
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string Symptoms { get; set; }
        public UserDTO User { get; set; }
    }
}
