using HospitalManagement.API.DTOs;

namespace HospitalManagement.API.Repositories.Hospital
{
    public interface IHospitalRepository
    {
        Task<Models.Hospital> GetHospital(int id);
        Task<PaginationDTO<Models.Hospital>> GetHospitals(int? page = 0, int? pageSize = int.MaxValue, string? keyword = null, string? sortColumn = "Id");
        Task<bool> CreateHospital(Models.Hospital hospital);
        Task<bool> UpdateHospital(Models.Hospital hospital);
        Task<bool> DeleteHospital(Models.Hospital hospital);
        Task<bool> IsSaveChanges();
    }
}