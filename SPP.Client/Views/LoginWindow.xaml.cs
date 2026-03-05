using SPP.Client.DTO;
using SPP.Client.Services;
using System.Windows;

namespace SPP.Client.Views;

public partial class LoginWindow : Window
{
    private readonly ApiService _api = new();

    public LoginWindow()
    {
        InitializeComponent();
    }

    private async void Login_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var result = await _api.LoginAsync(new LoginRequestDto
            {
                Login = LoginBox.Text,
                Password = PasswordBox.Password
            });

            if (result == null)
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка входа",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            App.CurrentUser = result;
           
            MessageBox.Show($"Добро пожаловать, {result.Name} {result.Surname} ({result.Role})",
                "Успешный вход", MessageBoxButton.OK, MessageBoxImage.Information);

            OpenRoleWindow(result.Role);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при входе: {ex.Message}", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
       
    }

    private void OpenRoleWindow(string role)
    {
        var user = App.CurrentUser;
        Window window = role?.Trim() switch
        {
            "Менеджер" => new ManagerWindow(user),
            "Работник" => new WorkerWindow(user),
            "Инструктор по обучению" => new InstructorWindow(user),
        };

        if (window != null)
        {
            window.Show();
            this.Close(); 
        }
        else
        {
            MessageBox.Show($"Неизвестная роль: {role}. Обратитесь к администратору.",
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

    }
}