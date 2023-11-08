using AutoMapper;
using HospitalManagement.API.DTOs;
using HospitalManagement.API.DTOs.DoctorDTOs;
using HospitalManagement.API.DTOs.HospitalDTOs;
using HospitalManagement.API.Models;
using HospitalManagement.API.Repositories.Hospital;

namespace HospitalManagement.API.Services.Hospital
{
    public class HospitalService : IHospitalService
    {
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IMapper _mapper;

        public HospitalService(IHospitalRepository hospitalRepository, IMapper mapper)
        {
            _hospitalRepository = hospitalRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse> GetHospital(int id)
        {
            try
            {
                var result = await _hospitalRepository.GetHospital(id);
                if (result == null)
                    return new APIResponse { StatusCode = 404, Message = "Hospital doesn't exist" };

                var hospital = new HospitalDetails()
                {
                    Id = result.Id,
                    Name = result.Name,
                    Address = result.Address,
                    Doctors = result.Doctors.Select(_mapper.Map<Models.Doctor, DoctorDTO>).ToList()
                };

                return new APIResponse { StatusCode = 200, Message = "Success", Data = hospital };
            }
            catch (Exception ex)
            {
                return new APIResponse { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<APIResponse> GetHospitals(int? page = 0, int? pageSize = int.MaxValue, string? keyword = null, string? sortColumn = "Id")
        {
            try
            {
                var hospitals = await _hospitalRepository.GetHospitals(page, pageSize, keyword, sortColumn);
                var result = _mapper.Map<PaginationDTO<Models.Hospital>, PaginationDTO<HospitalDTO>>(hospitals);
                return new APIResponse { StatusCode = 200, Message = "Success", Data = result };
            }
            catch (Exception ex)
            {
                return new APIResponse { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<APIResponse> CreateHospital(HospitalDTO hospitalDTO)
        {
            try
            {
                var hospital = _mapper.Map<HospitalDTO, Models.Hospital>(hospitalDTO);
                var result = await _hospitalRepository.CreateHospital(hospital);
                return new APIResponse { StatusCode = 200, Message = "Success" };
            }
            catch (Exception ex)
            {
                return new APIResponse { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<APIResponse> UpdateHospital(int id, HospitalDTO hospitalDTO)
        {
            try
            {
                var hospitalCurrent = await _hospitalRepository.GetHospital(id);
                if (hospitalCurrent == null) return new APIResponse { StatusCode = 404, Message = "Hospital doesn't exist" };
                hospitalCurrent.Name = hospitalDTO.Name;
                hospitalCurrent.Address = hospitalDTO.Address;
                await _hospitalRepository.UpdateHospital(hospitalCurrent);
                return new APIResponse { StatusCode = 200, Message = "Success" };
            }
            catch (Exception ex)
            {
                return new APIResponse { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<APIResponse> DeleteHospital(int id)
        {
            try
            {
                var hospitalCurrent = await _hospitalRepository.GetHospital(id);
                if (hospitalCurrent == null) return new APIResponse { StatusCode = 404, Message = "Hospital doesn't exist" };
                await _hospitalRepository.DeleteHospital(hospitalCurrent);
                return new APIResponse { StatusCode = 200, Message = "Success" };
            }
            catch (Exception ex)
            {
                return new APIResponse { StatusCode = 500, Message = ex.Message };
            }
        }
    }
}
