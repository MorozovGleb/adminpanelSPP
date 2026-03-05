using System.IO;
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

        private async void Generate_Click(object sender, RoutedEventArgs e)
        {
            using var client = new HttpClient();

            var stream = await client.GetStreamAsync("https://localhost:7048/api/schedule/stream");

            using var reader = new StreamReader(stream);

            ChatOutput.Clear();

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                if (!string.IsNullOrWhiteSpace(line))
                {
                    Dispatcher.Invoke(() =>
                    {
                        ChatOutput.AppendText(line + "\n");
                        ChatOutput.ScrollToEnd();
                    });
                }
            }
        }
    }
}

