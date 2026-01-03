using AutoMapper;
using MasterDatabase;
using MasterDatabase.Models;
using MasterLib.Common;
using MasterService.Dtos.Master.Unit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasterService.Services
{
    public class UnitService
    {
        private readonly MasterDbContext _dbContext;
        private readonly IMapper _mapper;

        public UnitService(MasterDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<Unit>> GetUnits()
        {
            var units = await _dbContext.Units.ToListAsync();
            return units;
        }

        public async Task<Unit> GetUnit(long id)
        {
            var unit = await _dbContext.Units.FirstOrDefaultAsync(x => x.Id == id);
            if (unit == null)
            {
                throw new BadRequestException("Unit not found");
            }

            return unit;
        }

        public async Task<Unit> CreateUnit(UnitCreateDto obj)
        {
            var existing = await _dbContext.Units.FirstOrDefaultAsync(x => x.Code == obj.Code);
            if (existing != null)
            {
                throw new BadRequestException("Unit with same code already exists");
            }
            existing = await _dbContext.Units.Include(x => x.Block).FirstOrDefaultAsync(x => x.Name == obj.Name && obj.BlockId != null && x.BlockId == obj.BlockId);
            if (existing != null)
            {
                throw new BadRequestException("Unit with same name already exists in " + existing.Block.Name);
            }

            var unit = _mapper.Map<Unit>(obj);

            _dbContext.Units.Add(unit);
            await _dbContext.SaveChangesAsync();

            return unit;
        }

        public async Task<Unit> UpdateUnit(UnitUpdateDto obj, long id)
        {
            var existing = await _dbContext.Units.Include(x => x.Block).FirstOrDefaultAsync(x => x.Name == obj.Name && obj.BlockId != null && x.BlockId == obj.BlockId);
            if (existing != null && existing.Id != id)
            {
                throw new BadRequestException("Unit with same name already exists in " + existing.Block.Name);
            }

            var unit = await _dbContext.Units.FirstOrDefaultAsync(x => x.Id == id);
            if (unit == null)
            {
                throw new BadRequestException("Unit not found");
            }

            _mapper.Map(obj, unit);

            _dbContext.Units.Update(unit);
            await _dbContext.SaveChangesAsync();

            return unit;
        }

        public async Task<bool> DeleteUnit(long id)
        {
            var unit = await _dbContext.Units.FirstOrDefaultAsync(x => x.Id == id);
            if (unit == null)
            {
                throw new BadRequestException("Unit not found");
            }

            _dbContext.Units.Remove(unit);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
