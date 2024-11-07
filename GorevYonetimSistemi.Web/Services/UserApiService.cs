using GorevYonetimSistemi.Business.Dtos.User;

namespace GorevYonetimSistemi.Web.Services
{
    public class UserApiService
    {
        private readonly HttpClient _httpClient;

        public UserApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UserDto>?> GetAllUserAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<UserDto>>("api/User");
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<UserDto>($"api/User/{id}");
        }

        public async Task CreateUserAsync(UserDto userDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/User", userDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateUserAsync(Guid id, UserDto userDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/user/{id}", userDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/user/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
