using GorevYonetimSistemi.Business.Dtos.User;
using GorevYonetimSistemi.Business.Services;
using GorevYonetimSistemi.Data.Entities;
using GorevYonetimSistemi.Web.Models;
using GorevYonetimSistemi.Web.Services;
using GorevYonetimSistemi.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GorevYonetimSistemi.Web.Controllers
{
    public class AdminController : Controller
    {

        private readonly IUserResponseService _userResponse;

        public AdminController(IUserResponseService userResponse)
        {
            _userResponse = userResponse;
        }
        public async Task<IActionResult> Index()
        {
            var (users, message) = await _userResponse.GetAllUsersAsync();

            if (users != null)
            {
                return View(users);
            }

            TempData["Error Message"] = message;
            return View(new List<UserDto>());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(Guid userId, UserDto userDto)
        {
            if (userId != userDto.UserId)
            {
                TempData["Error Message"] = "User ID mismatch";
                return RedirectToAction("Index");
            }

            var (updatedUser, message) = await _userResponse.UpdateUserAsync(userId, userDto);

            if (updatedUser != null)
            {
                TempData["SuccessMessage"] = "User updated successfully";
                return RedirectToAction("Index");
            }

            TempData["Error Message"] = message;
            return View("Index");
        }
    }
}