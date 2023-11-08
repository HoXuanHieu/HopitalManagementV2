using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.API.Repositories.Role
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DatabaseContext _context;

        public RoleRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Models.Role>> GetRoles()
            => await _context.Roles.ToListAsync();

        public async Task<Models.Role> GetRoleByName(string name)
            => _context.Roles.FirstOrDefault(x => x.Name == name);
    }
}
