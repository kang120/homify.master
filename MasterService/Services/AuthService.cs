
using AutoMapper;
using MasterDatabase;
using MasterDatabase.Models;
using MasterLib.Dtos.Master.User;
using MasterLib.Helpers;
using MasterService.Dtos.Master.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MasterService.Services
{
    public class AuthService
    {
        private readonly MasterDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(MasterDbContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto obj)
        {
            var user = await _dbContext.Users.Include(u => u.UserTokens).FirstOrDefaultAsync(u => u.Username == obj.Username && !string.IsNullOrEmpty(u.Password));

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            var isPasswordValid = PasswordHelper.VerifyPassword(new PasswordHashSalt { Username = user.Username, Email = user.Email }, user.Password, obj.Password);
            if (isPasswordValid == false)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            var dto = _mapper.Map<UserDto>(user);
            var accessToken = TokenHelper.GenerateJwtToken(dto, _configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], _configuration["Jwt:Key"]);
            var refreshToken = TokenHelper.GenerateRefreshToken();

            user.UserTokens.Add(new UserToken
            {
                RefreshToken = refreshToken,
                ExpiredAt = DateTime.UtcNow.AddDays(7)
            });

            await _dbContext.SaveChangesAsync();

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(string oldRefreshToken)
        {
            var token = await _dbContext.UserTokens.Include(rt => rt.User).FirstOrDefaultAsync(rt => rt.RefreshToken == oldRefreshToken);

            if (token == null || token.ExpiredAt < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid refresh token");

            var dto = _mapper.Map<UserDto>(token.User);
            var accessToken = TokenHelper.GenerateJwtToken(dto, _configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], _configuration["Jwt:Key"]);
            var newRefreshToken = TokenHelper.GenerateRefreshToken();

            _dbContext.UserTokens.Add(new UserToken
            {
                RefreshToken = newRefreshToken,
                UserId = token.UserId,
                ExpiredAt = DateTime.UtcNow.AddDays(7)
            });
            _dbContext.UserTokens.Remove(token);

            await _dbContext.SaveChangesAsync();

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var token = await _dbContext.UserTokens.FirstOrDefaultAsync(rt => rt.RefreshToken == refreshToken);
            if (token != null)
            {
                _dbContext.UserTokens.Remove(token);
                await _dbContext.SaveChangesAsync();
            }
        }
    }

}
