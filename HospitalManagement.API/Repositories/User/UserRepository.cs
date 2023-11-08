using HospitalManagement.API.DTOs;
using HospitalManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.API.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Models.User> GetUser(int id)
            => await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(user => user.Id == id);

        public async Task<Models.User> GetUserByEmail(string email)
            => await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);

        public async Task<PaginationDTO<Models.User>> GetUsers(int? page, int? pageSize, string? name, string? sortColumn, string? roleName)
        {
            var query = _context.Users.Include(u => u.Role).AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(u => u.FullName.Contains(name));
            }

            if (!string.IsNullOrEmpty(roleName))
            {
                query = query.Where(u => u.Role.Name.Equals(roleName));
            }

            switch (sortColumn)
            {
                case "Id":
                    query = query.OrderBy(x => x.Id);
                    break;
                default:
                    query = query.OrderBy(x => x.Id);
                    break;
            }

            var pagination = new PaginationDTO<Models.User>();
            var users = new List<Models.User>();
            users = await query.ToListAsync();
            pagination.TotalCount = users.Count;
            if (page == null || pageSize == null)
            {
                pagination.Page = 0;
                pagination.PageSize = users.Count;
            }
            else
            {
                users = await query.Skip(page.Value * pageSize.Value).Take(pageSize.Value).ToListAsync();
                pagination.PageSize = pageSize.Value;
                pagination.Page = page.Value;
            }
            pagination.Items = users;
            return pagination;
        }

        public async Task<bool> CreateUser(Models.User user)
        {
            await _context.Users.AddAsync(user);
            return await IsSaveChanges();
        }

        public async Task<bool> UpdateUser(Models.User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            return await IsSaveChanges();
        }

        public async Task<bool> DeleteUser(Models.User user)
        {
            _context.Users.Remove(user);
            return await IsSaveChanges();
        }

        public async Task<bool> IsEmailAlreadyExists(string email)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(email));
            return currentUser == null ? false : true;
        }

        public async Task<bool> VerifiedEmail(string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Token == token);
            if (user == null) return false;
            user.IsEmailVerified = true;
            _context.Entry(user).State = EntityState.Modified;
            return true;
        }

        public async Task<bool> IsSaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
