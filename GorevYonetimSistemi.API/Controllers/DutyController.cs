using GorevYonetimSistemi.Business.Dtos.Duty;
using GorevYonetimSistemi.Business.Services.Interfaces;
using GorevYonetimSistemi.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GorevYonetimSistemi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DutyController : ControllerBase
    {
        private readonly IDutyService _dutyService;

        public DutyController(IDutyService dutyService)
        {
            _dutyService = dutyService;
        }

        [HttpGet("{dutyId:guid}")]
        public async Task<IActionResult> GetDutyById(Guid dutyId)
        {
            try
            {
                var duty = await _dutyService.GetDutyByIdAsync(dutyId);

                if (duty == null)
                {
                    return NotFound("Duty not found.");
                }
                return Ok(duty);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDuties()
        {
            try
            {
                var duties = await _dutyService.GetAllDutiesAsync();

                if (duties == null)
                {
                    return NotFound("Duties not found.");
                }
                return Ok(duties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDuty([FromBody] DutyDto dutyDto)
        {
            try
            {   
                if(!ModelState.IsValid)
                {
                    return  BadRequest(ModelState);
                }
                
                var createdDuty = await _dutyService.CreateDutyAsync(dutyDto);

                if (createdDuty == null)
                {
                    return BadRequest("Duty could not be created.");
                }
                return CreatedAtAction(nameof(GetDutyById), new { dutyId = createdDuty.DutyId }, createdDuty);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{dutyId:guid}")]
        public async Task<IActionResult> UpdateDuty(Guid dutyId, DutyDto dutyDto)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedDuty = await _dutyService.UpdateDutyAsync(dutyId, dutyDto);

                if (updatedDuty == null)
                {
                    return BadRequest("Duty not found.");
                }
                return Ok(updatedDuty);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("User not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [HttpDelete("{dutyId:guid}")]
        public async Task<IActionResult> DeleteDuty(Guid dutyId)
        {
            try
            {
                await _dutyService.DeleteDutyAsync(dutyId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}