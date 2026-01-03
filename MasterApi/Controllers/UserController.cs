using MasterDatabase;
using MasterDatabase.Models;
using MasterLib.Helpers;
using MasterService.Dtos.Master.User;
using MasterService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auth.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsers();
            return Ok(users);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUser(long id)
        {
            var user = await _userService.GetUser(id);
            return Ok(user);
        }

        [HttpPost("user")]
        public async Task<IActionResult> CreateUser(UserCreateDto obj)
        {
            var user = await _userService.CreateUser(obj);
            return Ok(user);
        }

        [HttpPut("user/{id}")]
        public async Task<IActionResult> UpdateUser(UserUpdateDto obj, long id)
        {
            var user = await _userService.UpdateUser(obj, id);
            return Ok(user);
        }

        [HttpPut("user/{id}/updatepassword")]
        public async Task<IActionResult> UpdateUserPassword(UserPasswordUpdateDto obj, long id)
        {
            var user = await _userService.UpdateUserPassword(obj, id);
            return Ok(user);
        }

        [HttpDelete("user/{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            await _userService.DeleteUser(id);
            return Ok("Successfully delete user");
        }
    }
}
