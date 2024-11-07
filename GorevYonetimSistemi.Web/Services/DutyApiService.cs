using GorevYonetimSistemi.Business.Dtos.Duty;

namespace GorevYonetimSistemi.Web.Services
{
    public class DutyApiService
    {
        private readonly HttpClient _httpClient;

        public DutyApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DutyDto?> GetDutyByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<DutyDto>($"api/Duty/{id}");
        }

        public async Task<List<DutyDto>?> GetAllDutiesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<DutyDto>>("api/Duty");
        }

        public async Task CreateDutyAsync(DutyDto dutyDto)
        {
             var response = await _httpClient.PostAsJsonAsync("api/Duty", dutyDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateDutyAsync(Guid id, DutyDto dutyDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Duty/{id}", dutyDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteDutyAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Duty/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}