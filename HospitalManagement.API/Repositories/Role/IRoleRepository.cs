namespace HospitalManagement.API.Repositories.Role
{
    public interface IRoleRepository
    {
        Task<List<Models.Role>> GetRoles();
        Task<Models.Role> GetRoleByName(string name);
    }
}
