using AutoMapper;
using HospitalManagement.API.DTOs;
using HospitalManagement.API.DTOs.DoctorDTOs;
using HospitalManagement.API.Repositories.Doctor;
using HospitalManagement.API.Repositories.Hospital;
using HospitalManagement.API.Repositories.User;
using HospitalManagement.API.Services.User;

namespace HospitalManagement.API.Services.Doctor
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public DoctorService(IDoctorRepository doctorRepository,
            IHospitalRepository hospitalRepository,
            IUserService userService,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _hospitalRepository = hospitalRepository;
            _userService = userService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse> GetDoctor(int id)
        {
            try
            {
                var result = await _doctorRepository.GetDoctor(id);
                if (result == null) return new APIResponse { StatusCode = 404, Message = "Doctor doesn't exist" };
                var resultDTO = _mapper.Map<Models.Doctor, DoctorDTO>(result);
                return new APIResponse { StatusCode = 200, Message = "Success", Data = resultDTO };
            }
            catch (Exception ex)
            {
                return new APIResponse { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<APIResponse> GetDoctors(int? page = 0, int? pageSize = int.MaxValue, string? keyword = null, string? sortColumn = "Id")
        {
            try
            {
                var result = await _doctorRepository.GetDoctors(page, pageSize, keyword, sortColumn);
                var resultDTO = _mapper.Map<PaginationDTO<Models.Doctor>, PaginationDTO<DoctorDTO>>(result);
                return new APIResponse { StatusCode = 200, Message = "Sucsess", Data = resultDTO };
            }
            catch (Exception ex)
            {
                return new APIResponse { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<APIResponse> CreateDoctor(DoctorCreateDTO doctorCreateDTO)
        {
            try
            {
                var IsHospitalIdValid = _hospitalRepository.GetHospital(doctorCreateDTO.HospitalId);
                if (IsHospitalIdValid.Result == null) return new APIResponse { StatusCode = 404, Message = "Hospital doesn't exist" };
                doctorCreateDTO.User.RoleName = "Doctor";
                var resultCreateUser = await _userService.CreateUser(doctorCreateDTO.User);
                if (resultCreateUser.StatusCode != 200) return resultCreateUser;
                var user = await _userRepository.GetUserByEmail(doctorCreateDTO.User.Email);
                var doctor = new Models.Doctor();
                doctor.Description = doctorCreateDTO.Description;
                doctor.HospitalId = doctorCreateDTO.HospitalId;
                doctor.UserId = user.Id;
                var resultData = await _doctorRepository.CreateDoctor(doctor);
                return new APIResponse { StatusCode = 200, Message = "Sucsess" };
            }
            catch (Exception ex)
            {
                return new APIResponse { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<APIResponse> UpdateDoctor(int id, DoctorUpdateDTO doctorUpdateDTO)
        {
            try
            {
                var doctor = await _doctorRepository.GetDoctor(id);
                if (doctor == null) return new APIResponse { StatusCode = 404, Message = "Doctor doesn't exist" };
                var IsHospitalIdValid = _hospitalRepository.GetHospital(doctorUpdateDTO.HospitalId);
                if (IsHospitalIdValid.Result == null) return new APIResponse { StatusCode = 404, Message = "Hospital doesn't exist" };
                var resultUpdateUser = await _userService.UpdateUser(doctor.User.Id, doctorUpdateDTO.User);
                if (resultUpdateUser.StatusCode != 200) return resultUpdateUser;
                doctor.Description = doctorUpdateDTO.Description;
                doctor.HospitalId = doctorUpdateDTO.HospitalId;
                var resultData = await _doctorRepository.UpdateDoctor(doctor);
                return new APIResponse { StatusCode = 200, Message = "Sucsess", };
            }
            catch (Exception ex)
            {
                return new APIResponse { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<APIResponse> DeleteDoctor(int id)
        {
            try
            {
                var doctor = await _doctorRepository.GetDoctor(id);
                if (doctor == null) return new APIResponse { StatusCode = 404, Message = "Doctor doesn't exist" };
                await _userRepository.DeleteUser(doctor.User);
                return new APIResponse { StatusCode = 200, Message = "Sucsess" };
            }
            catch (Exception ex)
            {
                return new APIResponse { StatusCode = 500, Message = ex.Message };
            }
        }
    }
}
