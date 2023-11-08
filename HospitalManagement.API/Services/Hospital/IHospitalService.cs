using HospitalManagement.API.DTOs;
using HospitalManagement.API.DTOs.HospitalDTOs;

namespace HospitalManagement.API.Services.Hospital
{
    public interface IHospitalService
    {
        Task<APIResponse> GetHospital(int id);
        Task<APIResponse> GetHospitals(int? page = 0, int? pageSize = int.MaxValue, string? keyword = null, string? sortColumn = "Id");
        Task<APIResponse> UpdateHospital(int id, HospitalDTO hospitalDto);
        Task<APIResponse> CreateHospital(HospitalDTO hospitalDto);
        Task<APIResponse> DeleteHospital(int id);
    }
}
