using HospitalManagement.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.API.Repositories.Doctor
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly DatabaseContext _context;

        public DoctorRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Models.Doctor> GetDoctor(int id)
            => await _context.Doctors.Where(x => x.Id == id).Include(x => x.Hospital).Include(x => x.User).Include(d => d.User.Role).FirstOrDefaultAsync();

        public async Task<Models.Doctor> GetDoctorByUserId(int userId)
            => await _context.Doctors.Where(x => x.UserId == userId).FirstOrDefaultAsync();

        public async Task<PaginationDTO<Models.Doctor>> GetDoctors(int? page = 0, int? pageSize = int.MaxValue, string? keyword = null, string? sortColumn = "Id")
        {
            var query = _context.Doctors.Include(d => d.Hospital).Include(d => d.User).Include(d => d.User.Role).AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(u => u.User.FullName.Contains(keyword));
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

            var pagination = new PaginationDTO<Models.Doctor>();
            var doctors = new List<Models.Doctor>();
            doctors = await query.ToListAsync();
            pagination.TotalCount = doctors.Count;
            if (page == null || pageSize == null)
            {
                pagination.Page = 0;
                pagination.PageSize = doctors.Count;
            }
            else
            {
                doctors = await query.Skip(page!.Value * pageSize!.Value).Take(pageSize.Value).ToListAsync();
                pagination.PageSize = pageSize.Value;
                pagination.Page = page.Value;
            }
            pagination.Items = doctors;
            return pagination;
        }

        public async Task<bool> CreateDoctor(Models.Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
            return await IsSaveChanges();
        }

        public async Task<bool> UpdateDoctor(Models.Doctor doctor)
        {
            _context.Entry(doctor).State = EntityState.Modified;
            return await IsSaveChanges();
        }

        public async Task<bool> DeleteDoctor(Models.Doctor doctor)
        {
            _context.Doctors.Remove(doctor);
            return await IsSaveChanges();
        }

        public async Task<bool> IsSaveChanges() => await _context.SaveChangesAsync() > 0;
    }
}
