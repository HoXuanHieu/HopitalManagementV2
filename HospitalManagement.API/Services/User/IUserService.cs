using HospitalManagement.API.DTOs;
using HospitalManagement.API.DTOs.UserDTOs;

namespace HospitalManagement.API.Services.User
{
    public interface IUserService
    {
        Task<APIResponse> GetUser(int? id);
        Task<APIResponse> GetUsers(int? page, int? pageSize, string? name, string sortColumn, string roleName);
        Task<APIResponse> CreateUser(UserCreateDTO user);
        Task<APIResponse> UpdateUser(int userId, UserUpdateDTO userUpdateDTO);
        Task<APIResponse> DeleteUser(int userId);
    }
}
