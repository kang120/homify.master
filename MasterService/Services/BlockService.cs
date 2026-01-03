using AutoMapper;
using MasterDatabase;
using MasterDatabase.Models;
using MasterLib.Common;
using MasterService.Dtos.Master.Block;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasterService.Services
{
    public class BlockService
    {
        private readonly MasterDbContext _dbContext;
        private readonly IMapper _mapper;

        public BlockService(MasterDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<Block>> GetBlocks()
        {
            var blocks = await _dbContext.Blocks.ToListAsync();
            return blocks;
        }

        public async Task<Block> GetBlock(long id)
        {
            var block = await _dbContext.Blocks.FirstOrDefaultAsync(x => x.Id == id);
            if (block == null)
            {
                throw new BadRequestException("Block not found");
            }

            return block;
        }

        public async Task<Block> CreateBlock(BlockCreateDto obj)
        {
            var existing = await _dbContext.Blocks.FirstOrDefaultAsync(x => x.Code == obj.Code);
            if (existing != null)
            {
                throw new BadRequestException("Block with same code already exists");
            }
            existing = await _dbContext.Blocks.FirstOrDefaultAsync(x => x.Name == obj.Name);
            if (existing != null)
            {
                throw new BadRequestException("Block with same name already exists");
            }

            var block = _mapper.Map<Block>(obj);

            _dbContext.Blocks.Add(block);
            await _dbContext.SaveChangesAsync();

            return block;
        }

        public async Task<Block> UpdateBlock(BlockUpdateDto obj, long id)
        {
            var existing = await _dbContext.Blocks.FirstOrDefaultAsync(x => x.Name == obj.Name);
            if (existing != null && existing.Id != id)
            {
                throw new BadRequestException("Block with same name already exists");
            }

            var block = await _dbContext.Blocks.FirstOrDefaultAsync(x => x.Id == id);
            if (block == null)
            {
                throw new BadRequestException("Block not found");
            }

            _mapper.Map(obj, block);

            _dbContext.Blocks.Update(block);
            await _dbContext.SaveChangesAsync();

            return block;
        }

        public async Task<bool> DeleteBlock(long id)
        {
            var block = await _dbContext.Blocks.FirstOrDefaultAsync(x => x.Id == id);
            if (block == null)
            {
                throw new BadRequestException("Block not found");
            }

            _dbContext.Blocks.Remove(block);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
