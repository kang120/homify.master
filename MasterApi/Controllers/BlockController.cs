using AutoMapper;
using MasterDatabase;
using MasterDatabase.Models;
using MasterLib.Extensions;
using MasterLib.Helpers;
using MasterService.Dtos.Master.Block;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auth.Controllers
{
    [ApiController]
    public class BlockController : ControllerBase
    {
        private readonly MasterDbContext _dbContext;
        private readonly IMapper _mapper;

        public BlockController(MasterDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet("blocks")]
        public async Task<IActionResult> GetBlocks()
        {
            var blocks = await _dbContext.Blocks.ToListAsync();
            return Ok(blocks);
        }

        [HttpGet("block/{id}")]
        public async Task<IActionResult> GetBlock(long id)
        {
            var block = await _dbContext.Blocks.FirstOrDefaultAsync(x => x.Id == id);
            if (block == null)
            {
                return BadRequest("Block not found");
            }

            return Ok(block);
        }

        [HttpPost("block")]
        public async Task<IActionResult> CreateBlock(BlockCreateDto obj)
        {
            var block = _mapper.Map<Block>(obj);

            _dbContext.Blocks.Add(block);
            await _dbContext.SaveChangesAsync();

            return Ok("Successfully create new block");
        }

        [HttpPut("block/{id}")]
        public async Task<IActionResult> UpdateBlock(BlockUpdateDto obj, long id)
        {
            var block = await _dbContext.Blocks.FirstOrDefaultAsync(x => x.Id == id);
            if(block == null)
            {
                return BadRequest("Block not found");
            }

            _mapper.Map(obj, block);

            _dbContext.Blocks.Update(block);
            await _dbContext.SaveChangesAsync();

            return Ok("Successfully update block");
        }

        [HttpDelete("block/{id}")]
        public async Task<IActionResult> DeleteBlock(long id)
        {
            var block = await _dbContext.Blocks.FirstOrDefaultAsync(x => x.Id == id);
            if (block == null)
            {
                return BadRequest("Block not found");
            }

            block.DeletedOn = DateTime.UtcNow;
            block.DeletedBy = Request.GetUserId();

            _dbContext.Blocks.Update(block);
            await _dbContext.SaveChangesWithoutAuditAsync();

            return Ok("Successfully delete block");
        }
    }
}
