using AutoMapper;
using HospitalManagement.API.DTOs;
using HospitalManagement.API.DTOs.UserDTOs;
using HospitalManagement.API.Repositories.Doctor;
using HospitalManagement.API.Repositories.Role;
using HospitalManagement.API.Repositories.User;

namespace HospitalManagement.API.Services.User
{
    public class UserService : IUserService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository,
            IMapper mapper,
            IRoleRepository roleRepository,
            IDoctorRepository doctorRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _roleRepository = roleRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<APIResponse> GetUser(int? id)
        {
            try
            {
                if (id == null) return new APIResponse { StatusCode = 400, Message = "Empty ID" };
                var result = await _userRepository.GetUser(id.Value);
                if (result == null) return new APIResponse { StatusCode = 404, Message = $"User doesn't exist" };
                var user = _mapper.Map<UserDTO>(result);
                if (user.RoleName == "Doctor")
                {
                    var doctor = await _doctorRepository.GetDoctorByUserId(user.Id);
                    return new APIResponse
                    {
                        StatusCode = 200,
                        Message = "Success",
                        Data = new
                        {
                            Id = user.Id,
                            Email = user.Email,
                            FullName = user.FullName,
                            PhoneNumber = user.PhoneNumber,
                            Birthday = user.Birthday,
                            Gender = user.Gender,
                            Address = user.Address,
                            RoleId = user.RoleId,
                            RoleName = user.RoleName,
                            IsEmailVerified = user.IsEmailVerified,
                            Doctor = doctor
                        }
                    };
                }
                return new APIResponse { StatusCode = 200, Message = "Success", Data = user };
            }
            catch (Exception ex)
            {
                return new APIResponse { StatusCode = 500, Message = ex.Message };
            }

        }

        public async Task<APIResponse> GetUsers(int? page, int? pageSize, string? name, string? sortColumn, string? roleName)
        {
            try
            {
                var pagination = await _userRepository.GetUsers(page, pageSize, name, sortColumn, roleName);
                var paginationUserDTO = new PaginationDTO<UserDTO>();
                paginationUserDTO.PageSize = pagination.PageSize;
                paginationUserDTO.TotalCount = pagination.TotalCount;
                paginationUserDTO.Page = pagination.Page;
                if (pagination.Items != null)
                {
                    paginationUserDTO.Items = pagination.Items.Select(_mapper.Map<Models.User, UserDTO>).ToList();
                }
                return new APIResponse { StatusCode = 200, Message = "Success", Data = paginationUserDTO };
            }
            catch (Exception ex)
            {
                return new APIResponse { StatusCode = 500, Message = ex.Message };
            }

        }

        public async Task<APIResponse> CreateUser(UserCreateDTO userDTO)
        {
            try
            {
                var check = _userRepository.IsEmailAlreadyExists(userDTO.Email);
                var roleUser = _roleRepository.GetRoleByName(userDTO.RoleName);
                await Task.WhenAll(check, roleUser);
                if (check.Result) return new APIResponse { StatusCode = 400, Message = "Email doesn't exist" };
                if (roleUser.Result == null) return new APIResponse { StatusCode = 404, Message = "Roles doesn't exist" };
                var newUser = _mapper.Map<UserCreateDTO, Models.User>(userDTO);
                newUser.RoleId = roleUser.Result.Id;
                newUser.IsEmailVerified = true;
                await _userRepository.CreateUser(newUser);
                return new APIResponse { StatusCode = 200, Message = "Success" };
            }
            catch (Exception ex)
            {
                return new APIResponse { StatusCode = 500, Message = ex.Message };
            }

        }

        public async Task<APIResponse> UpdateUser(int userId, UserUpdateDTO userUpdateDTO)
        {
            try
            {
                var userCurrent = await _userRepository.GetUser(userId);
                if (userCurrent == null) return new APIResponse { StatusCode = 404, Message = "User doesn't exist" };
                userCurrent.FullName = userUpdateDTO.FullName;
                userCurrent.PhoneNumber = userUpdateDTO.PhoneNumber;
                userCurrent.Birthday = userUpdateDTO.Birthday;
                userCurrent.Address = userUpdateDTO.Address;
                userCurrent.Gender = userUpdateDTO.Gender;
                await _userRepository.UpdateUser(userCurrent);
                return new APIResponse { StatusCode = 200, Message = "Success" };
            }
            catch (Exception ex)
            {
                return new APIResponse { StatusCode = 500, Message = ex.Message };
            }

        }

        public async Task<APIResponse> DeleteUser(int userId)
        {
            var user = await _userRepository.GetUser(userId);
            if (user == null) return new APIResponse { StatusCode = 404, Message = "User doesn't exist" };
            await _userRepository.DeleteUser(user);
            return new APIResponse { StatusCode = 200, Message = "Success" };
        }
    }
}
