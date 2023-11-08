using HospitalManagement.API.DTOs.DoctorDTOs;
using HospitalManagement.API.Services.Doctor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var responseData = await _doctorService.GetDoctor(id);
            return StatusCode(responseData.StatusCode, responseData);
        }     
        [HttpGet]
        public async Task<IActionResult> GetDoctors(int? page = 0, int? pageSize = int.MaxValue, string? keyword = null, string? sortColumn = "Id")
        {
            var responseData = await _doctorService.GetDoctors(page, pageSize, keyword, sortColumn);
            return StatusCode(responseData.StatusCode, responseData);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] DoctorCreateDTO doctorCreateDto)
        {
            var responseData = await _doctorService.CreateDoctor(doctorCreateDto);
            return StatusCode(responseData.StatusCode, responseData);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] DoctorUpdateDTO doctorUpdateDto)
        {
            var responseData = await _doctorService.UpdateDoctor(id, doctorUpdateDto);
            return StatusCode(responseData.StatusCode, responseData);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var responseData = await _doctorService.DeleteDoctor(id);
            return StatusCode(responseData.StatusCode, responseData);
        }
    }
}
