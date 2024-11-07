using AutoMapper;
using GorevYonetimSistemi.Business.Dtos.User;
using GorevYonetimSistemi.Business.Services.Interfaces;
using GorevYonetimSistemi.Data.Entities;
using GorevYonetimSistemi.Data.Repositories.Interfaces;
using SQLitePCL;

namespace GorevYonetimSistemi.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly PasswordService _passwordService;

        public UserService(IUserRepository userRepository, IMapper mapper, PasswordService passwordService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordService = passwordService;
        }

        public async Task<UserDto> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var user = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<IEnumerable<UserDto>>(user);
        }

        public async Task<UserDto> CreateUserAsync(UserDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.Password))
            {
                throw new ArgumentException("Password cannot be null or empty");
            }

            var user = _mapper.Map<User>(userDto);

            user.UserId = Guid.NewGuid();

            user.Password = _passwordService.HashPassword(userDto.Password);

            await _userRepository.CreateUserAsync(user);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateUserAsync(Guid userId, UserDto userDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            _mapper.Map(userDto, user);
            await _userRepository.UpdateUserAsync(user);
            return _mapper.Map<UserDto>(user);
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            await _userRepository.DeleteUserAsync(userId);
        }
    }
}