using HospitalManagement.API.DTOs.UserDTOs;
using HospitalManagement.API.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HospitalManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO user)
        {

            var responseData = await _userService.CreateUser(user);
            return StatusCode(responseData.StatusCode, responseData);
        }
        [Authorize(Roles = "Admin, Doctor")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(int? page = null, int? pageSize = null, string? name = null, string? roleName = null, string? sortColumn = "Id")
        {

            var responseData = await _userService.GetUsers(page, pageSize, name, sortColumn, roleName);
            return Ok(responseData);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var responseData = await _userService.GetUser(id);
            return StatusCode(responseData.StatusCode, responseData);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser(int id, [FromBody] UserUpdateDTO userUpdateDTO)
        {
            var responseData = await _userService.UpdateUser(id, userUpdateDTO);
            return StatusCode(responseData.StatusCode, responseData);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var responseData = await _userService.DeleteUser(id);
            return StatusCode(responseData.StatusCode, responseData);
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = Convert.ToInt32(User.FindFirstValue("UserId"));
            var responseData = await _userService.GetUser(userId);
            return StatusCode(responseData.StatusCode, responseData);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDTO userUpdateDTO)
        {
            var userId = Convert.ToInt32(User.FindFirstValue("UserId"));
            var responseData = await _userService.UpdateUser(userId, userUpdateDTO);
            return StatusCode(responseData.StatusCode, responseData);
        }
    }
}
