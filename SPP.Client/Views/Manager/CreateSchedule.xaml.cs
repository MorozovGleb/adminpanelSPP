using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Documents;

namespace SPP.Client.Views.Manager
{
    /// <summary>
    /// Логика взаимодействия для CreateSchedule.xaml
    /// </summary>
    public partial class CreateSchedule : Window
    {
        private readonly HttpClient _http = new();
        private const string BaseUrl = "https://localhost:7048";

        public CreateSchedule()
        {
            InitializeComponent();
        }

        // Вспомогательный метод для очистки RichTextBox
        private void ClearOutput()
        {
            ChatOutput.Document.Blocks.Clear();
        }

        // Вспомогательный метод для добавления текста
        private void AppendText(string text)
        {
            Dispatcher.Invoke(() =>
            {
                var paragraph = new Paragraph(new Run(text));
                ChatOutput.Document.Blocks.Add(paragraph);
                ChatOutput.ScrollToEnd();
            });
        }

        // Вспомогательный метод для добавления цветного текста
        private void AppendColoredText(string text, string color = "#DCDCDC")
        {
            Dispatcher.Invoke(() =>
            {
                var run = new Run(text);
                run.Foreground = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(color));

                var paragraph = new Paragraph(run);
                ChatOutput.Document.Blocks.Add(paragraph);
                ChatOutput.ScrollToEnd();
            });
        }

        private async void Generate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Блокируем кнопку на время выполнения
                GenerateButton.IsEnabled = false;
                ClearOutput();
                AppendColoredText("Начинаем генерацию расписания...", "#FFD700");

                // Создаем запрос на генерацию
                var request = new
                {
                    startDate = DateTime.Today,
                    daysCount = 7
                };

                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Отправляем POST запрос на генерацию
                var response = await _http.PostAsync($"{BaseUrl}/api/schedule/generate", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Парсим ответ
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var result = JsonSerializer.Deserialize<GenerateResponse>(responseContent, options);

                    AppendColoredText($"✅ {result.Message}", "#4CAF50");
                    AppendText($"Сгенерировано смен: {result.Count}");
                    AppendText($"Дата начала: {result.StartDate:dd.MM.yyyy}");
                    AppendText($"Количество дней: {result.DaysCount}");
                    AppendText("");

                    // Показываем первые несколько смен для примера
                    if (result.Schedules != null && result.Schedules.Count > 0)
                    {
                        AppendColoredText("Пример сгенерированных смен:", "#FFD700");
                        foreach (var schedule in result.Schedules.Take(5))
                        {
                            AppendText($"  • Сотрудник {schedule.ID_User}, " +
                                $"Верификация {schedule.ID_Verification}, " +
                                $"Дата: {schedule._Date:dd.MM.yyyy}, " +
                                $"Время: {schedule._Start:hh\\:mm} - {schedule._End:hh\\:mm}");
                        }

                        if (result.Schedules.Count > 5)
                        {
                            AppendText($"  ... и еще {result.Schedules.Count - 5} смен");
                        }
                    }
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    AppendColoredText($"❌ Ошибка при генерации: {error}", "#f44336");
                }
            }
            catch (HttpRequestException ex)
            {
                AppendColoredText($"❌ Ошибка соединения с сервером: {ex.Message}", "#f44336");
                AppendText($"Проверьте, что сервер запущен на {BaseUrl}");
            }
            catch (Exception ex)
            {
                AppendColoredText($"❌ Неожиданная ошибка: {ex.Message}", "#f44336");
            }
            finally
            {
                GenerateButton.IsEnabled = true;
            }
        }

        private async void ViewExisting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewExistingButton.IsEnabled = false;
                ClearOutput();
                AppendColoredText("Загрузка существующего расписания...", "#FFD700");

                var fromDate = DateTime.Today;
                var toDate = DateTime.Today.AddDays(7);

                var response = await _http.GetAsync(
                    $"{BaseUrl}/api/schedule/existing?fromDate={fromDate:yyyy-MM-dd}&toDate={toDate:yyyy-MM-dd}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var schedules = JsonSerializer.Deserialize<List<ScheduleDto>>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (schedules != null && schedules.Any())
                    {
                        AppendColoredText($"📋 Найдено {schedules.Count} смен:", "#4CAF50");
                        AppendText("");

                        var groupedByDate = schedules.GroupBy(s => s._Date.Date);
                        foreach (var group in groupedByDate.OrderBy(g => g.Key))
                        {
                            AppendColoredText($"📅 {group.Key:dd.MM.yyyy}:", "#2196F3");
                            foreach (var schedule in group.OrderBy(s => s._Start))
                            {
                                AppendText($"  • Сотрудник {schedule.ID_User}, " +
                                    $"Верификация {schedule.ID_Verification}, " +
                                    $"{schedule._Start:hh\\:mm} - {schedule._End:hh\\:mm}");
                            }
                            AppendText("");
                        }
                    }
                    else
                    {
                        AppendColoredText("📋 Нет смен в выбранном диапазоне дат", "#FF9800");
                    }
                }
                else
                {
                    AppendColoredText($"❌ Ошибка при загрузке: {response.StatusCode}", "#f44336");
                }
            }
            catch (Exception ex)
            {
                AppendColoredText($"❌ Ошибка: {ex.Message}", "#f44336");
            }
            finally
            {
                ViewExistingButton.IsEnabled = true;
            }
        }

        private async void CheckStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckStatusButton.IsEnabled = false;
                ClearOutput();
                AppendColoredText("Проверка статуса сервера...", "#FFD700");

                var response = await _http.GetAsync($"{BaseUrl}/api/schedule/status");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var status = JsonSerializer.Deserialize<StatusResponse>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    AppendColoredText("✅ Сервер работает", "#4CAF50");
                    AppendColoredText("📊 Статистика:", "#FFD700");
                    AppendText($"  • Пользователей: {status.Counts.Users}");
                    AppendText($"  • Верификаций: {status.Counts.Verifications}");
                    AppendText($"  • Смен в расписании: {status.Counts.Schedules}");
                    AppendText($"  • Подтверждений навыков: {status.Counts.Confirmations}");
                    AppendText("");
                    AppendText($"🕐 Время сервера: {status.Timestamp:dd.MM.yyyy HH:mm:ss}");
                }
                else
                {
                    AppendColoredText($"❌ Сервер недоступен: {response.StatusCode}", "#f44336");
                }
            }
            catch (Exception ex)
            {
                AppendColoredText($"❌ Ошибка соединения: {ex.Message}", "#f44336");
                AppendText($"Проверьте, что сервер запущен на {BaseUrl}");
            }
            finally
            {
                CheckStatusButton.IsEnabled = true;
            }
        }

        private async void ClearSchedule_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Вы уверены, что хотите удалить расписание на следующую неделю?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                ClearScheduleButton.IsEnabled = false;
                ClearOutput();
                AppendColoredText("Удаление расписания...", "#FFD700");

                var fromDate = DateTime.Today;
                var toDate = DateTime.Today.AddDays(7);

                var response = await _http.DeleteAsync(
                    $"{BaseUrl}/api/schedule/clear?fromDate={fromDate:yyyy-MM-dd}&toDate={toDate:yyyy-MM-dd}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    AppendColoredText($"✅ {content}", "#4CAF50");
                }
                else
                {
                    AppendColoredText($"❌ Ошибка при удалении: {response.StatusCode}", "#f44336");
                }
            }
            catch (Exception ex)
            {
                AppendColoredText($"❌ Ошибка: {ex.Message}", "#f44336");
            }
            finally
            {
                ClearScheduleButton.IsEnabled = true;
            }
        }
    }

    // DTO классы для десериализации ответов
    public class GenerateResponse
    {
        public string Message { get; set; }
        public int Count { get; set; }
        public DateTime StartDate { get; set; }
        public int DaysCount { get; set; }
        public List<ScheduleDto> Schedules { get; set; }
    }

    public class ScheduleDto
    {
        public int ID { get; set; }
        public int ID_User { get; set; }
        public int ID_Verification { get; set; }
        public DateTime _Date { get; set; }
        public TimeSpan _Start { get; set; }
        public TimeSpan _End { get; set; }
        public int ID_Day { get; set; }
    }

    public class StatusResponse
    {
        public string Status { get; set; }
        public DateTime Timestamp { get; set; }
        public CountsDto Counts { get; set; }
    }

    public class CountsDto
    {
        public int Users { get; set; }
        public int Verifications { get; set; }
        public int Schedules { get; set; }
        public int Confirmations { get; set; }
    }
}