using HospitalManagement.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.API.Repositories.Hospital
{
    public class HospitalRepository : IHospitalRepository
    {
        private readonly DatabaseContext _context;

        public HospitalRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Models.Hospital> GetHospital(int id)
            => await _context.Hospitals.Include(h => h.Doctors).FirstOrDefaultAsync(x => x.Id == id);

        public async Task<PaginationDTO<Models.Hospital>> GetHospitals(int? page, int? pageSize, string? keyword, string? sortColumn)
        {
            var query = _context.Hospitals.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(u => u.Name.Contains(keyword));
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

            var pagination = new PaginationDTO<Models.Hospital>();
            var hospitals = new List<Models.Hospital>();
            hospitals = await query.ToListAsync();
            pagination.TotalCount = hospitals.Count;
            if (page == null || pageSize == null)
            {
                pagination.Page = 0;
                pagination.PageSize = (pagination.TotalCount != 0) ? pagination.TotalCount : 10;
            }
            else
            {
                hospitals = await query.Skip(page!.Value * pageSize!.Value).Take(pageSize.Value).ToListAsync();
                pagination.PageSize = pageSize.Value;
                pagination.Page = page.Value;
            }
            pagination.Items = hospitals;
            return pagination;
        }

        public async Task<bool> CreateHospital(Models.Hospital hospital)
        {
            await _context.Hospitals.AddAsync(hospital);
            return await IsSaveChanges();
        }

        public async Task<bool> UpdateHospital(Models.Hospital hospital)
        {
            _context.Entry(hospital).State = EntityState.Modified;
            return await IsSaveChanges();
        }

        public async Task<bool> DeleteHospital(Models.Hospital hospital)
        {
            _context.Remove(hospital);
            return await IsSaveChanges();
        }

        public async Task<bool> IsSaveChanges() => await _context.SaveChangesAsync() > 0;
    }
}
