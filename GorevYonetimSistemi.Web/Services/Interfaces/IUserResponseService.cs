using GorevYonetimSistemi.Business.Dtos.User;

namespace GorevYonetimSistemi.Web.Services.Interfaces
{
    public interface IUserResponseService
    {
        Task<(UserDto?, string)> GetUserByIdAsync(Guid userId);
        Task<(List<UserDto>?, string)> GetAllUsersAsync();
        Task<(UserDto?, string)> UpdateUserAsync(Guid userId, UserDto userDto);
        Task<(bool, string)> DeleteUserAsync(Guid userId);
    }
}