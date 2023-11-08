using HospitalManagement.API.DTOs;

namespace HospitalManagement.API.Repositories.Doctor
{
    public interface IDoctorRepository
    {
        Task<Models.Doctor> GetDoctor(int id);
        Task<Models.Doctor> GetDoctorByUserId(int userId);
        Task<PaginationDTO<Models.Doctor>> GetDoctors(int? page = 0, int? pageSize = int.MaxValue, string? keyword = null, string? sortColumn = "Id");
        Task<bool> CreateDoctor(Models.Doctor doctor);
        Task<bool> UpdateDoctor(Models.Doctor doctor);
        Task<bool> DeleteDoctor(Models.Doctor doctor);
        Task<bool> IsSaveChanges();
    }
}
