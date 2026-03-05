using SPP.Client.Models;
using SPP.Client.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SPP.Client.Views.Manager
{
    /// <summary>
    /// Логика взаимодействия для AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        private readonly ApiService _api;
        public AddUserWindow()
        {
            InitializeComponent();
            _api = new ApiService();
            Loaded += async (s, e) => await LoadData();

        }
        private async Task LoadData()
        {
            var list = await _api.GetRoles();

            RolBox.ItemsSource = list;
            RolBox.DisplayMemberPath = "Name";
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var user = new UserNotRole();
            user.ID = int.Parse(IdBox.Text);
            user.Name = NameBox.Text;
            user.Surname = SurNameBox.Text;
            user.Phone_number = PhoneBox.Text;
            user.Login= LoginBox.Text;
            user.Password = PasswordBox.Text;
            var role = (Role)RolBox.SelectedItem;
            user.ID_Role = role.ID;
            user.Role = role;
            //validation place
            //
            await _api.PostUserAsync(user);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
