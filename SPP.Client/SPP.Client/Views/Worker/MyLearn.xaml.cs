using SPP.Client.DTO;
using SPP.Client.Services;
using System.Windows;

namespace SPP.Client.Views.Worker
{
    public partial class MyLearn : Window
    {
        private readonly LoginResponseDto _user;

        public MyLearn(LoginResponseDto user)
        {
            InitializeComponent();
            _user = user;

           
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
          

            var api = new ApiService();
            var data = await api.GetUserVerifications(_user.ID);

            

            VerificationsGrid.ItemsSource = data;
        }
    }
}
