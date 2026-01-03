using AutoMapper;
using MasterDatabase;
using MasterDatabase.Models;
using MasterLib.Extensions;
using MasterLib.Helpers;
using MasterService.Dtos.Master.Block;
using MasterService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auth.Controllers
{
    [ApiController]
    public class BlockController : ControllerBase
    {
        private readonly BlockService _blockService;

        public BlockController(BlockService blockService)
        {
            _blockService = blockService;
        }

        [HttpGet("blocks")]
        public async Task<IActionResult> GetBlocks()
        {
            var blocks = await _blockService.GetBlocks();
            return Ok(blocks);
        }

        [HttpGet("block/{id}")]
        public async Task<IActionResult> GetBlock(long id)
        {
            var block = await _blockService.GetBlock(id);
            return Ok(block);
        }

        [HttpPost("block")]
        public async Task<IActionResult> CreateBlock(BlockCreateDto obj)
        {
            var block = await _blockService.CreateBlock(obj);
            return Ok(block);
        }

        [HttpPut("block/{id}")]
        public async Task<IActionResult> UpdateBlock(BlockUpdateDto obj, long id)
        {
            var block = await _blockService.UpdateBlock(obj, id);
            return Ok(block);
        }

        [HttpDelete("block/{id}")]
        public async Task<IActionResult> DeleteBlock(long id)
        {
            await _blockService.DeleteBlock(id);
            return Ok("Successfully delete block");
        }
    }
}
