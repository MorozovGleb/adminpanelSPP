using SPP.Client.DTO;
using SPP.Client.Views.Worker;
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
using SPP.Client.Views.Instructor;

namespace SPP.Client.Views
{
    /// <summary>
    /// Логика взаимодействия для InstructorWindow.xaml
    /// </summary>
    public partial class InstructorWindow : Window
    {
        private readonly LoginResponseDto _user;
        public InstructorWindow(LoginResponseDto user)
        {
            InitializeComponent();
            _user = user;

            HeaderText.Text =
                $"{_user.Role}\nДобро пожаловать,{_user.Name} {_user.Surname}";
        }


        private void VerificationButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new SPP.Client.Views.Instructor.VerificationWindow();
            window.ShowDialog();
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
            var window = new SPP.Client.Views.Instructor.ProvideTraining();
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
