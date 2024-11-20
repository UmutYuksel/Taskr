using GorevYonetimSistemi.Business.Dtos.Duty;
using GorevYonetimSistemi.Business.Dtos.User;
using Microsoft.AspNetCore.Http;

namespace GorevYonetimSistemi.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(Guid userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> UpdateUserAsync(Guid userId,UserDto userDto);
        Task DeleteUserAsync(Guid userId);
    }
}