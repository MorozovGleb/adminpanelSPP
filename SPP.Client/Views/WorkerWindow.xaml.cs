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

        }

        private void HoursButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LearnButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new MyLearn(_user);
            window.Show();
        }
    }
}
