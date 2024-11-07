using GorevYonetimSistemi.Business.Dtos.User;

namespace GorevYonetimSistemi.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(Guid userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> CreateUserAsync(UserDto userDto);
        Task<UserDto> UpdateUserAsync(Guid userId,UserDto userDto);
        Task DeleteUserAsync(Guid userId);
    }
}