using HospitalManagement.API.DTOs;
using HospitalManagement.API.DTOs.AppointmentDTOs;

namespace HospitalManagement.API.Services.Appointment
{
    public interface IAppointmentService
    {
        Task<APIResponse> GetAppointments(int? page = null, int? pageSize = null, DateTime? date = null, int? patientId = null, int? doctorId = null, string? sortBy = "Date");
        Task<APIResponse> CreateAppointment(AppointmentCreateDTO appointmentCreate);
        Task<APIResponse> GetAppointmentById(int id);
        Task<APIResponse> DeleteAppointment(int id, string role, int userId);
    }
}
