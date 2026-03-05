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
    public async Task PostUserAsync(UserNotRole user)
    {
        var resp = await _httpClient.PostAsJsonAsync(
            "api/users", user);
        if (!resp.IsSuccessStatusCode) { 
            string err = await resp.Content.ReadAsStringAsync();
            MessageBox.Show(resp.StatusCode.ToString() +":" +err);
        }
        else
        {
            MessageBox.Show("Пользователь успешно создан!");
        }
    }
    public async Task PostConformAsync(Verification c)
    {
        var resp = await _httpClient.PostAsJsonAsync(
            "api/verification", c);
        if (!resp.IsSuccessStatusCode)
        {
            string err = await resp.Content.ReadAsStringAsync();
            MessageBox.Show(resp.StatusCode.ToString() + ":" + err);
        }
        else
        {
            MessageBox.Show("Запись успешно добавлена!");
        }
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
    public async Task<List<Verification1>> GetTypaVerifications()
    {

        return await _httpClient.GetFromJsonAsync<List<Verification1>>
            ($"api/verification");

    }
    public async Task<List<User>> GetUsers()
    {

        return await _httpClient.GetFromJsonAsync<List<User>>
            ($"api/users");

    }
    public async Task<List<Role>> GetRoles()
    {

        return await _httpClient.GetFromJsonAsync<List<Role>>
            ($"api/roles");

    }
    public async Task UpdateVerification(Verification dto)
    {
        var response = await _httpClient.PutAsJsonAsync(
            $"api/verification/f",
            dto);

        response.EnsureSuccessStatusCode();
    }
}
