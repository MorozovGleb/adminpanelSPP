using SPP.Client.DTO;
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
    /// Логика взаимодействия для AllWorkersWindow.xaml
    /// </summary>
    public partial class AllWorkersWindow : Window
    {
        private readonly ApiService _api;
        private ObservableCollection<User> _data;
        public AllWorkersWindow()
        {
            InitializeComponent();
            _api = new ApiService();
            Loaded += async (s, e) => await LoadData();
        }
        private async Task LoadData()
        {
            var list = await _api.GetUsers();

            _data = new ObservableCollection<User>(list);

            WorkersGrid.ItemsSource = _data;
        }

    }
}
