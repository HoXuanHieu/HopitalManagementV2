using HospitalManagement.API.DTOs;

namespace HospitalManagement.API.Repositories.Appointment
{
    public interface IAppointmentRepository
    {
        Task<PaginationDTO<Models.Appointment>> GetAppointments(int? page = null, int? pageSize = null, DateTime? date = null, int? patientId = null, int? doctorId = null, string? sortBy = "Date");
        Task<bool> CreateAppointmentAsync(Models.Appointment appointment);
        Task<bool> IsSaveChange();
        Task<Models.Appointment> GetAppointmentById(int id);
        Task<bool> UpdateAppointment(Models.Appointment appointment);
        Task<bool> DeleteAppointment(Models.Appointment appointment);
    }
}
