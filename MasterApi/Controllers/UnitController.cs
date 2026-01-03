using AutoMapper;
using MasterDatabase;
using MasterDatabase.Models;
using MasterLib.Extensions;
using MasterService.Dtos.Master.Unit;
using MasterService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auth.Controllers
{
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly UnitService _unitService;

        public UnitController(UnitService unitService)
        {
            _unitService = unitService;
        }

        [HttpGet("units")]
        public async Task<IActionResult> GetUnits()
        {
            var units = await _unitService.GetUnits();
            return Ok(units);
        }

        [HttpGet("unit/{id}")]
        public async Task<IActionResult> GetUnit(long id)
        {
            var unit = await _unitService.GetUnit(id);
            return Ok(unit);
        }

        [HttpPost("unit")]
        public async Task<IActionResult> CreateUnit(UnitCreateDto obj)
        {
            var unit = await _unitService.CreateUnit(obj);
            return Ok(unit);
        }

        [HttpPut("unit/{id}")]
        public async Task<IActionResult> UpdateUnit(UnitUpdateDto obj, long id)
        {
            var unit = await _unitService.UpdateUnit(obj, id);
            return Ok(unit);
        }

        [HttpDelete("unit/{id}")]
        public async Task<IActionResult> DeleteUnit(long id)
        {
            await _unitService.DeleteUnit(id);
            return Ok("Successfully delete unit");
        }
    }
}
