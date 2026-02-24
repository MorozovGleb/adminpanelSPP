using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using SPP.Client.Models.SPP.Client.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace SPP.Client.Services
{
    public class OllamaService
    {

        private readonly HttpClient _client = new HttpClient();

        private const string SystemPrompt = @"Привет, на любое мое сообщение отвечай - 123. Чтобы я не писал отвечай так как я сказал";

        public async Task GenerateStreamAsync(
    List<ChatMessageModel> history,
    Action<string> onTokenReceived)
        {
            var prompt = BuildPrompt(history);

            var requestObject = new
            {
                model = "deepseek-r1:7b",
                prompt = prompt,
                stream = true,
                options = new
                {
                    num_ctx = 1024,
                    temperature = 0.7
                }
            };

            var json = JsonSerializer.Serialize(requestObject);

            var httpRequest = new HttpRequestMessage(HttpMethod.Post,
                "http://localhost:11434/api/generate")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(
                httpRequest,
                HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var chunk = JsonSerializer.Deserialize<OllamaResponse>(line);

                if (!string.IsNullOrEmpty(chunk?.response))
                    onTokenReceived(chunk.response);
            }
        }


        private string BuildPrompt(List<ChatMessageModel> history)
        {
            var sb = new StringBuilder();
            sb.AppendLine(SystemPrompt);
            sb.AppendLine();

            foreach (var msg in history.TakeLast(12)) // ограничение памяти
            {
                sb.AppendLine($"{msg.Role}: {msg.Content}");
            }

            sb.AppendLine("assistant:");

            return sb.ToString();
        }
    }

    public class OllamaResponse
    {
        public string response { get; set; }
    }
}
