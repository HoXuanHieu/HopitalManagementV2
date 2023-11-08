using HospitalManagement.API.DTOs;
using HospitalManagement.API.DTOs.UserDTOs;

namespace HospitalManagement.API.Services.Authenticate
{
    public interface IAuthenticateService
    {
        Task<APIResponse> Login(UserLoginDTO userLoginDto);
        Task<string> Register(UserRegisterDTO registerUserDto);
    }
}
