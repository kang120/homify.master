using MasterDatabase;
using MasterDatabase.Models;
using MasterLib.Helpers;
using MasterService.Dtos.Master.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auth.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private MasterDbContext _dbContext;

        public UserController(MasterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _dbContext.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUser(long id)
        {
            var model = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
            {
                return BadRequest("User not found");
            }

            return Ok(model);
        }

        [HttpPost("user")]
        public async Task<IActionResult> CreateUser(UserCreateDto obj)
        {
            var model = new User
            {
                UserId = Guid.NewGuid(),
                Username = obj.Username,
                Email = obj.Email,
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                PhoneNo = obj.PhoneNo,
                IsActive = true,
            };

            _dbContext.Users.Add(model);
            await _dbContext.SaveChangesAsync();

            // send email

            return Ok("Successfully create new user");
        }

        [HttpPut("user/{id}")]
        public async Task<IActionResult> UpdateUser(UserUpdateDto obj, long id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if(user == null)
            {
                return BadRequest("User not found");
            }

            user.FirstName = obj.FirstName;
            user.LastName = obj.LastName;
            user.PhoneNo = obj.PhoneNo;
            user.IsActive = obj.IsActive;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return Ok("Successfully update user");
        }

        [HttpPut("user/{id}/updatepassword")]
        public async Task<IActionResult> UpdateUserPassword(UserPasswordUpdateDto obj, long id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            user.Password = PasswordHelper.HashPassword(new PasswordHashSalt { Username = user.Username, Email = user.Email }, obj.Password);

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return Ok("Successfully update user password");
        }
    }
}
