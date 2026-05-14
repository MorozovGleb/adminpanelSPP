using SPP.Client.DTO;
using SPP.Client.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
namespace SPP.Client.Views
{
    /// <summary>
    /// Логика взаимодействия для Schedule.xaml
    /// </summary>
    public partial class Schedule : Window
    {
        private readonly LoginResponseDto _user;
        private readonly ApiService _api;
        public Schedule(LoginResponseDto user)
        {
            InitializeComponent();
            _user = user;
            _api = new ApiService();
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UserNameText.Text = $"{_user.Surname} {_user.Name}";
            await LoadMySchedule();
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadMySchedule();
        }

        private async System.Threading.Tasks.Task LoadMySchedule()
        {
            try
            {
                RefreshButton.IsEnabled = false;
                ClearOutput();
                AppendText("📅 Загрузка вашего расписания...\n", "#FFD700");

                var scheduleData = await _api.GetMySchedule(_user.ID);

                if (scheduleData != null)
                {
                    // Заголовок
                    AppendText("═══════════════════════════════════════════", "#FFD700");
                    AppendText($"👤 {scheduleData.UserName}", "#4CAF50");
                    AppendText($"📊 Всего смен: {scheduleData.TotalShifts}", "#2196F3");
                    AppendText("═══════════════════════════════════════════\n", "#FFD700");

                    if (scheduleData.Shifts != null && scheduleData.Shifts.Any())
                    {
                        // Порядок дней недели
                        var dayOrder = new Dictionary<string, int>
                {
                     { "Понедельник", 1 },
    { "Вторник", 2 },
    { "Среда", 3 },
    { "Четверг", 4 },
    { "Пятница", 5 },
    { "Суббота", 6 },
    { "Воскресенье", 7 },
    // Добавь и английские на всякий случай
    { "Monday", 1 },
    { "Tuesday", 2 },
    { "Wednesday", 3 },
    { "Thursday", 4 },
    { "Friday", 5 },
    { "Saturday", 6 },
    { "Sunday", 7 }
                };

                        // Группируем и сортируем по дням недели
                        var groupedByDay = scheduleData.Shifts
                            .GroupBy(s => s.DayOfWeek)
                            .OrderBy(g => dayOrder.ContainsKey(g.Key) ? dayOrder[g.Key] : 99)
                            .ThenBy(g => g.Key);

                        double totalHours = 0;

                        foreach (var group in groupedByDay)
                        {
                            AppendText($"📅 {group.Key}:", "#FF9800");

                            // Сортируем смены внутри дня по времени
                            var sortedShifts = group.OrderBy(s => s.Date).ThenBy(s => s.Start);

                            foreach (var shift in sortedShifts)
                            {
                                AppendText($"  • {shift.Date}: {shift.Start} - {shift.End}");

                                // Считаем часы
                                if (TimeSpan.TryParse(shift.Start, out var start) &&
                                    TimeSpan.TryParse(shift.End, out var end))
                                {
                                    if (end > start)
                                        totalHours += (end - start).TotalHours;
                                    else
                                        totalHours += (end + TimeSpan.FromDays(1) - start).TotalHours;
                                }
                            }

                            AppendText("");
                        }

                        AppendText("─────────────────────────────────────────", "#FFD700");
                        AppendText($"⏱️ Итого часов: {totalHours:F1}", "#4CAF50");
                    }
                    else
                    {
                        AppendText("📋 У вас пока нет смен в расписании", "#FF9800");
                    }
                }
            }
            catch (Exception ex)
            {
                AppendText($"❌ Ошибка: {ex.Message}", "#f44336");
            }
            finally
            {
                RefreshButton.IsEnabled = true;
            }
        }

        private void ClearOutput()
        {
            Dispatcher.Invoke(() =>
            {
                OutputBox.Document.Blocks.Clear();
            });
        }

        private void AppendText(string text, string color = "#DCDCDC")
        {
            Dispatcher.Invoke(() =>
            {
                var run = new Run(text + "\n");
                run.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
                var paragraph = new Paragraph(run);
                paragraph.Margin = new Thickness(0);
                OutputBox.Document.Blocks.Add(paragraph);
                OutputBox.ScrollToEnd();
            });
        }
    }
}

