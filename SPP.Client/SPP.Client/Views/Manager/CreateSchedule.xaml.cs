using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace SPP.Client.Views.Manager
{
    /// <summary>
    /// Логика взаимодействия для CreateSchedule.xaml
    /// </summary>
    public partial class CreateSchedule : Window
    {

        private readonly HttpClient _http = new();
        public CreateSchedule()
        {
            InitializeComponent();
        }
        private async void Send_Click(object sender, RoutedEventArgs e)
        {
            var message = UserInput.Text;

            if (string.IsNullOrWhiteSpace(message))
                return;

            var request = new
            {
                message = message
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync(
                "https://localhost:7048/api/chat",
                content);

            var result = await response.Content.ReadAsStringAsync();

            ChatOutput.Text += $"\n\nВы: {message}";
            ChatOutput.Text += $"\nGPT: {result}";

            UserInput.Clear();
        }
    }
}

