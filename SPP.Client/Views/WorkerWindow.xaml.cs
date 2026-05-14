using SPP.Client.DTO;
using SPP.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SPP.Client.Views.Worker;

namespace SPP.Client.Views
{
    /// <summary>
    /// Логика взаимодействия для WorkerWindow.xaml
    /// </summary>
    public partial class WorkerWindow : Window
    {
        private readonly LoginResponseDto _user;
        public WorkerWindow(LoginResponseDto user)
        {
            InitializeComponent();
            _user = user;

            HeaderText.Text =
                $"{_user.Role}\nДобро пожаловать,{_user.Name} {_user.Surname}";
        }
       
        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            var myScheduleWindow = new Schedule(_user);
            myScheduleWindow.ShowDialog();
        }

        private void HoursButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LearnButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new MyLearn(_user);
            window.ShowDialog();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
    "Вы уверены, что хотите выйти из аккаунта?",
    "Подтверждение выхода",
    MessageBoxButton.YesNo,
    MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Закрываем текущее окно
                var authWindow = new LoginWindow(); // Замените на имя вашего окна авторизации
                authWindow.Show();
                this.Close();

                // Показываем окно авторизации
            }
        }
    }
}
