using SPP.Client.Models;
using SPP.Client.Models.SPP.Client.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SPP.Client.Data
{
    public class ChatService
    {
        private readonly HttpClient _http;

        public ChatService(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri("https://localhost:7048/");
        }

        public async Task SaveMessageAsync(ChatMessageModel message)
        {
            await _http.PostAsJsonAsync("api/Chat/SaveMessage", message);
        }

        public async Task<List<ChatMessageModel>> GetMessagesAsync(Guid sessionId)
        {
            return await _http.GetFromJsonAsync<List<ChatMessageModel>>($"api/Chat/GetMessages/{sessionId}");
        }
    }
}