using HospitalManagement.API.DTOs;
using HospitalManagement.API.DTOs.AppointmentDTOs;
using HospitalManagement.API.Services.Appointment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HospitalManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAppointments(int? page = null, int? pageSize = null, DateTime? date = null, int? patientId = null, int? doctorId = null, string? sortBy = "Date")
        {
            var role = User.FindFirstValue("Role");
            var res = new APIResponse();
            if (role == "Admin")
            {
                res = await _appointmentService.GetAppointments(page, pageSize, date, patientId, doctorId, sortBy);
            }
            else if (role == "User")
            {
                patientId = Convert.ToInt32(User.FindFirstValue("UserId"));
                res = await _appointmentService.GetAppointments(page, pageSize, date, patientId, doctorId, sortBy);
            }
            else if (role == "Doctor")
            {
                doctorId = Convert.ToInt32(User.FindFirstValue("DoctorId"));
                res = await _appointmentService.GetAppointments(page, pageSize, date, patientId, doctorId, sortBy);
            }


            return StatusCode(res.StatusCode, res);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var res = await _appointmentService.GetAppointmentById(id);
            return StatusCode(res.StatusCode, res);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("admin-create")]
        public async Task<IActionResult> AdminCreateAppointment([FromBody] AppointmentCreateDTO appointmentCreate)
        {
            var res = await _appointmentService.CreateAppointment(appointmentCreate);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost("patient-create")]
        public async Task<IActionResult> PatientCreateAppointment([FromBody] AppointmentCreateDTO appointmentCreate)
        {
            var patientId = User.FindFirstValue("UserId");
            appointmentCreate.UserId = Convert.ToInt32(patientId);
            var res = await _appointmentService.CreateAppointment(appointmentCreate);
            return StatusCode(res.StatusCode, res);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var role = User.FindFirstValue("Role");
            var userId = Convert.ToInt32(User.FindFirstValue("UserId"));
            var res = await _appointmentService.DeleteAppointment(id, role, userId);
            return StatusCode(res.StatusCode, res);
        }
    }
}
