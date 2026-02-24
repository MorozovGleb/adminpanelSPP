using SPP.Client.DTO;
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

namespace SPP.Client.Views
{
    /// <summary>
    /// Логика взаимодействия для ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        private readonly LoginResponseDto _user;
        public ManagerWindow(LoginResponseDto user)
        {
            InitializeComponent();
            _user = user;

            HeaderText.Text =
                $"{_user.Role}\nДобро пожаловать,{_user.Name} {_user.Surname}";
        }

        private void VerificationButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new SPP.Client.Views.Instructor.VerificationWindow();
            window.Show();
        }

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void HoursButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new SPP.Client.Views.Manager.ScheduleCreateWindow();
            window.Show();
        }

        private void CreateUserButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new SPP.Client.Views.Manager.AllWorkersWindow();
            window.Show();
        }
    }
}
