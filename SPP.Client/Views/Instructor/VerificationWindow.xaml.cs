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
        private ObservableCollection<VerificationView> _data;
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
                var ver = await _api.GetTypaVerifications();
            var view = new List<VerificationView>();
                foreach( var verification in list ) {
                var vi = new VerificationView();
                vi.ID = verification.ID;
                vi.ID_User = verification.ID_User;
                vi._Date = verification._Date;
                vi.Position = ver.FirstOrDefault(v => v.ID == verification.ID_Verification).Name;
                view.Add(vi);
            }
                _data = new ObservableCollection<VerificationView>(view);

                ConfirmationGrid.ItemsSource = _data;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new SPP.Client.Views.Instructor.AddVerificationWindow();
            window.Show();
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