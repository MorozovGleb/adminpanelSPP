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

namespace SPP.Client.Views.Instructor
{
    /// <summary>
    /// Логика взаимодействия для AddVerificationWindow.xaml
    /// </summary>
    public partial class AddVerificationWindow : Window
    {
        private readonly ApiService _api;
        public AddVerificationWindow()
        {
            InitializeComponent();
            _api = new ApiService();
            Loaded += async (s, e) => await LoadData();

        }
        private async Task LoadData()
        {
            var list = await _api.GetTypaVerifications();

            VerificationBox.ItemsSource = list;
            VerificationBox.DisplayMemberPath = "Name";
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var conf = new Verification();
                conf.ID = int.Parse(IdBox.Text);
                conf.ID_User = int.Parse(IdUserBox.Text);
                var ver = (Verification1)VerificationBox.SelectedItem;
                conf.ID_Verification = ver.ID;
                conf._Date = DatePicker.DisplayDate;
                // validation place
                //
                await _api.PostConformAsync(conf);
            }catch(Exception ex)
            {

            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
