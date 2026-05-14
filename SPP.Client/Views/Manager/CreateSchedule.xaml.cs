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
                AppendText("Загрузка существующего расписания...");

                var fromDate = DateTime.Today;
                var toDate = DateTime.Today.AddDays(7);

                var response = await _http.GetAsync(
                    $"{BaseUrl}/api/Schedule/existing?fromDate={fromDate:yyyy-MM-dd}&toDate={toDate:yyyy-MM-dd}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var scheduleData = JsonSerializer.Deserialize<ScheduleResponse>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (scheduleData != null && scheduleData.Users.Any())
                    {
                        AppendText($"✅ Найдено {scheduleData.TotalUsers} пользователей, {scheduleData.TotalShifts} смен");
                        AppendText("");

                        foreach (var user in scheduleData.Users)
                        {
                            AppendText($"👤 {user.UserName} (ID: {user.UserId}) - {user.TotalShifts} смен:");

                            foreach (var shift in user.Shifts)
                            {
                                AppendText($"  📅 {shift.Date} ({shift.DayOfWeek}): {shift.Start} - {shift.End}");
                            }

                            AppendText(""); // Пустая строка между пользователями
                        }
                    }
                    else
                    {
                        AppendText("📋 Нет смен в выбранном периоде");
                    }
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    AppendText($"❌ Ошибка сервера: {error}");
                }
            }
            catch (Exception ex)
            {
                AppendText($"❌ Ошибка: {ex.Message}");
            }
            finally
            {
                ViewExistingButton.IsEnabled = true;
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
    public class ScheduleResponse
    {
        public int TotalUsers { get; set; }
        public int TotalShifts { get; set; }
        public List<UserSchedule> Users { get; set; }
    }

    public class UserSchedule
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int TotalShifts { get; set; }
        public List<ShiftDto> Shifts { get; set; }
    }

    public class ShiftDto
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string DayOfWeek { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }
}