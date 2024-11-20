using System.Text;
using GorevYonetimSistemi.Business.Dtos.User;
using GorevYonetimSistemi.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;

namespace GorevYonetimSistemi.Web.Services
{
    public class UserResponseSevice : IUserResponseService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "http://localhost:5113/api/user";

        public UserResponseSevice(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(UserDto?, string)> GetUserByIdAsync(Guid userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<UserDto>(json);
                    return (user, string.Empty);
                }
                return (null, "User not found.");
            }
            catch (Exception)
            {
                return (null, "An error occurred while retrieving the user.");
            }
        }

        public async Task<(List<UserDto>?, string)> GetAllUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiBaseUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<List<UserDto>>(json);
                    return (users, string.Empty);
                }
                return (null, "Users not found.");
            }
            catch (Exception ex)
            {
                // Hata mesajını loglayın
                return (null, $"An error occurred while retrieving the users: {ex.Message}");
            }
        }

        public async Task<(UserDto?, string)> UpdateUserAsync(Guid userId, UserDto userDto)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(userDto);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_apiBaseUrl}/{userId}", content);

                // Başarısız yanıt durumunda istisna fırlatır
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var updatedUser = JsonConvert.DeserializeObject<UserDto>(json);
                return (updatedUser, string.Empty);
            }
            catch (HttpRequestException ex)
            {
                // HTTP hataları için
                Console.WriteLine($"HTTP Error: {ex.Message}");
                return (null, "User update failed due to HTTP error.");
            }
            catch (Exception ex)
            {
                // Diğer hatalar için
                Console.WriteLine($"Error updating user: {ex.Message}");
                return (null, "An error occurred while updating the user.");
            }
        }
    

        public async Task<(bool, string)> DeleteUserAsync(Guid userId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    return (true, string.Empty);
                }

                return (false, "User deletion failed.");
            }
            catch (Exception)
            {
                return (false, "An error occurred while deleting the user.");
            }
        }

    }
}