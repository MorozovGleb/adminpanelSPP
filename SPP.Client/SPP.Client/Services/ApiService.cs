using SPP.Client.DTO;
using SPP.Client.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;


namespace SPP.Client.Services;
public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7048/")
        };
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "api/auth/login", request);

        if (!response.IsSuccessStatusCode)
            return null;    

        return await response.Content
            .ReadFromJsonAsync<LoginResponseDto>();
    }
    public async Task<List<VerificationStatusDto>> GetUserVerifications(int userId)
    {

            return await _httpClient.GetFromJsonAsync<List<VerificationStatusDto>>
                ($"api/verification/user/{userId}");

    }
    public async Task<List<Verification>> GetVerifications()
    {

        return await _httpClient.GetFromJsonAsync<List<Verification>>
            ($"api/verification/f");

    }
    public async Task UpdateVerification(Verification dto)
    {
        var response = await _httpClient.PutAsJsonAsync(
            $"api/verification/f",
            dto);

        response.EnsureSuccessStatusCode();
    }
}
