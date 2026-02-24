using SPP.Client.DTO;
using SPP.Client.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SPP.Client.Views.Instructor
{
    public partial class VerificationWindow : Window
    {
        private readonly ApiService _api;
        private ObservableCollection<Verification> _data;
        private bool _isEditing = false;

        public VerificationWindow()
        {
            InitializeComponent();
            _api = new ApiService();
            Loaded += async (s, e) => await LoadData();
        }

        private async Task LoadData()
        {
                var list = await _api.GetVerifications();

                _data = new ObservableCollection<Verification>(list);

                ConfirmationGrid.ItemsSource = _data;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            _isEditing = !_isEditing;

            ConfirmationGrid.IsReadOnly = !_isEditing;
            ConfirmationGrid.CanUserAddRows = _isEditing;

            EditButton.Content = _isEditing ? "Завершить" : "Редактировать";
        }

        private async void ConfirmationGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction != DataGridEditAction.Commit)
                return;

            var editedItem = e.Row.Item as Verification;

            if (editedItem == null)
                return;

            try
            {
                await _api.UpdateVerification(editedItem);
                MessageBox.Show("Запись обновлена.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }
    }
}