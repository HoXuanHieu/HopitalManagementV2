using HospitalManagement.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.API.Repositories.Appointment
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly DatabaseContext _context;

        public AppointmentRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAppointmentAsync(Models.Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            return true;
        }

        public async Task<bool> DeleteAppointment(Models.Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
            return true;
        }

        public async Task<Models.Appointment> GetAppointmentById(int id)
        {
            return await _context.Appointments.Include(a => a.User).Include(a => a.Doctor).FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<PaginationDTO<Models.Appointment>> GetAppointments(int? page = null, int? pageSize = null, DateTime? date = null, int? patientId = null, int? doctorId = null, string? sortBy = "Date")
        {
            var query = _context.Appointments.Include(a => a.Doctor).Include(a => a.User).AsQueryable();

            if (date != null)
            {
                query = query.Where(u => u.Date == date.Value.Date);
            }
            if (patientId != null)
            {
                query = query.Where(u => u.UserId == patientId.Value);
            }
            if (doctorId != null)
            {
                query = query.Where(u => u.DoctorId == doctorId.Value);
            }

            switch (sortBy)
            {
                case "Id":
                    query = query.OrderBy(u => u.Id);
                    break;
                case "Date":
                    query = query.OrderByDescending(u => u.Date).ThenByDescending(u => u.Id);
                    break;
                default:
                    query = query.OrderByDescending(u => u.Date);
                    break;
            }
            var pagination = new PaginationDTO<Models.Appointment>();
            var appointments = new List<Models.Appointment>();

            appointments = await query.ToListAsync();
            pagination.TotalCount = appointments.Count;
            if (page == null || pageSize == null)
            {
                pagination.Page = 0;
                pagination.PageSize = (pagination.TotalCount != 0) ? pagination.TotalCount : 10;
            }
            else
            {
                appointments = await query.Skip(page!.Value * pageSize!.Value).Take(pageSize.Value).ToListAsync();
                pagination.PageSize = pageSize.Value;
                pagination.Page = page.Value;
            }
            pagination.Items = appointments;
            return pagination;
        }

        public async Task<bool> IsSaveChange()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAppointment(Models.Appointment appointment)
        {
            _context.Entry(appointment).State = EntityState.Modified;
            return true;
        }
    }
}
