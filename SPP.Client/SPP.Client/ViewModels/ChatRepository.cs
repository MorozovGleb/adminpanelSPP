using SPP.Client.Models;
using SPP.Client.Models.SPP.Client.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SPP.Client.Services
{
    public class ChatRepository
    {
        private readonly HttpClient _http = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7048/") // Адрес вашего сервера
        };

        public async Task SaveMessageAsync(Guid sessionId, string role, string content)
        {
            var message = new ChatMessageModel
            {
                SessionId = sessionId,
                Role = role,
                Content = content
            };

            await _http.PostAsJsonAsync("api/Chat/SaveMessage", message);
        }

        public async Task<List<ChatMessageModel>> GetMessagesAsync(Guid sessionId)
        {
            return await _http.GetFromJsonAsync<List<ChatMessageModel>>($"api/Chat/GetMessages/{sessionId}");
        }
    }
}