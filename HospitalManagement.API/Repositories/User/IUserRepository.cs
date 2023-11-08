using HospitalManagement.API.DTOs;

namespace HospitalManagement.API.Repositories.User
{
    public interface IUserRepository
    {
        Task<Models.User> GetUser(int id);
        Task<Models.User> GetUserByEmail(string email);
        Task<PaginationDTO<Models.User>> GetUsers(int? page, int? pageSize, string? name, string sortColumn, string? roleName);
        Task<bool> CreateUser(Models.User user);
        Task<bool> UpdateUser(Models.User user);
        Task<bool> DeleteUser(Models.User user);
        Task<bool> IsEmailAlreadyExists(string email);
        Task<bool> VerifiedEmail(string token);
        Task<bool> IsSaveChanges();
    }
}
