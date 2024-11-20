using GorevYonetimSistemi.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GorevYonetimSistemi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDutyController : ControllerBase
    {
        private readonly IUserDutyService _userDutyService;

        public UserDutyController
        (
            IUserDutyService userDutyService
        )
        {
            _userDutyService = userDutyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserDuties()
        {
            try
            {
                var userDuties = await _userDutyService.GetAllUserDutiesAsync();
                return Ok(userDuties); // Veriyi doğrudan döner
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetUserDutyByUserId(Guid userId)
        {
            try
            {
                var userDuties = await _userDutyService.GetUserDutiesByUserIdAsync(userId);
                return Ok(userDuties); // Veriyi doğrudan döner
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }

        [HttpGet("{dutyId:guid}/users")]
        public async Task<IActionResult> GetUsersByDutyId(Guid dutyId)
        {
            try
            {
                var users = await _userDutyService.GetUsersByDutyIdAsync(dutyId);

                if (users == null || !users.Any())
                {
                    return NotFound("No users assigned to this duty.");
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{userId:guid}/duties/{dutyId:guid}")]
        public async Task<IActionResult> AssignUserToDuty(Guid userId, Guid dutyId)
        {
            try
            {
                await _userDutyService.AssignUserToDutyAsync(userId, dutyId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{userId:guid}/duties/{dutyId:guid}")]
        public async Task<IActionResult> RemoveUserFromDuty(Guid userId, Guid dutyId)
        {
            try
            {
                await _userDutyService.RemoveUserFromDutyAsync(userId, dutyId);
                return NoContent(); // Successfully removed
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}