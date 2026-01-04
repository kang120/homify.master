using AutoMapper;
using MasterDatabase;
using MasterDatabase.Models;
using MasterLib.Common;
using MasterLib.Helpers;
using MasterService.Dtos.Master.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasterService.Services
{
    public class UserService
    {
        private readonly MasterDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserService(MasterDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await _dbContext.Users.ToListAsync();
            return users;
        }

        public async Task<User> GetUser(Guid userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (user == null)
            {
                throw new BadRequestException("User not found");
            }

            return user;
        }

        public async Task<User> CreateUser(UserCreateDto obj)
        {
            var existing = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == obj.Username);
            if (existing != null)
            {
                throw new BadRequestException("Username already exists");
            }
            existing = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == obj.Email);
            if (existing != null)
            {
                throw new BadRequestException("Email already exists");
            }

            var user = _mapper.Map<User>(obj);
            user.UserId = Guid.NewGuid();

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> UpdateUser(UserUpdateDto obj, long id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                throw new BadRequestException("User not found");
            }

            _mapper.Map(obj, user);

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> UpdateUserPassword(UserPasswordUpdateDto obj, long id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                throw new BadRequestException("User not found");
            }

            user.Password = PasswordHelper.HashPassword(new PasswordHashSalt { Username = user.Username, Email = user.Email }, obj.Password);

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<bool> DeleteUser(long id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                throw new BadRequestException("User not found");
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
