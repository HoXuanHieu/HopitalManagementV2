using HospitalManagement.API.DTOs;
using HospitalManagement.API.DTOs.DoctorDTOs;

namespace HospitalManagement.API.Services.Doctor
{
    public interface IDoctorService
    {
        Task<APIResponse> GetDoctor(int id);
        Task<APIResponse> GetDoctors(int? page = 0, int? pageSize = int.MaxValue, string? keyword = null, string? sortColumn = "Id");
        Task<APIResponse> CreateDoctor(DoctorCreateDTO doctorCreateDTO);
        Task<APIResponse> UpdateDoctor(int id, DoctorUpdateDTO doctorUpdateDTO);
        Task<APIResponse> DeleteDoctor(int id);
    }
}
