using HospitalManagement.API.DTOs.HospitalDTOs;
using HospitalManagement.API.Services.Hospital;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalsController : ControllerBase
    {
        private readonly IHospitalService _hospitalService;

        public HospitalsController(IHospitalService hospitalService)
        {
            _hospitalService = hospitalService;
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHospitalById(int id)
        {
            var responseData = await _hospitalService.GetHospital(id);
            return StatusCode(responseData.StatusCode, responseData);
        }

        [HttpGet]
        public async Task<IActionResult> GetHospitals(int? page = 0, int? pageSize = int.MaxValue, string? keyword = null, string? sortColumn = "Id")
        {
            var responseData = await _hospitalService.GetHospitals(page, pageSize, keyword, sortColumn);
            return StatusCode(responseData.StatusCode, responseData.Data);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateHospital([FromBody] HospitalDTO hospitalDTO)
        {
            var responseData = await _hospitalService.CreateHospital(hospitalDTO);
            return StatusCode(responseData.StatusCode, responseData);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHospital(int id, [FromBody] HospitalDTO hospitalDTO)
        {
            var responseData = await _hospitalService.UpdateHospital(id, hospitalDTO);
            return StatusCode(responseData.StatusCode, responseData);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHospital(int id)
        {
            var responseData = await _hospitalService.DeleteHospital(id);
            return StatusCode(responseData.StatusCode, responseData);
        }
    }
}
