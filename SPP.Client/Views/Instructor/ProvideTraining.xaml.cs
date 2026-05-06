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
    /// Логика взаимодействия для ProvideTraining.xaml
    /// </summary>
    public partial class ProvideTraining : Window
    {
        public ProvideTraining()
        {
            InitializeComponent();
        }

        private void VvodnoeOznakimlenie_Click(object sender, RoutedEventArgs e)
        {
            var window = new SPP.Client.Views.Instructor.Moduls.IntroductoryMeetingWithTheInstructor();
            window.ShowDialog();
        }

        private void Potatoes_Click(object sender, RoutedEventArgs e)
        {
            var window = new SPP.Client.Views.Instructor.Moduls.CookingPotatoes();
            window.ShowDialog();
        }

        private void DrinksAndDesserts_Click(object sender, RoutedEventArgs e)
        {
            var window = new SPP.Client.Views.Instructor.Moduls.DrinksAndDesserts();
            window.ShowDialog();
        }

        private void Mayachok_Click(object sender, RoutedEventArgs e)
        {
            var window = new SPP.Client.Views.Instructor.Moduls.ReceivingAndPayingForOrdersAtTheCounter();
            window.ShowDialog();
        }

        private void Prilavok_Click(object sender, RoutedEventArgs e)
        {
            var window = new SPP.Client.Views.Instructor.Moduls.CollectingAndIssuingOrdersAtTheCounter();
            window.ShowDialog();
        }

        private void Delivery_Click(object sender, RoutedEventArgs e)
        {
            var window = new SPP.Client.Views.Instructor.Moduls.ServiceDelivery();
            window.ShowDialog();
        }

        private void HallAndKiosks_Click(object sender, RoutedEventArgs e)
        {
            var window = new SPP.Client.Views.Instructor.Moduls.HallAndKiosks();
            window.ShowDialog();
        }

        private void Oplata_Click(object sender, RoutedEventArgs e)
        {
            var window = new SPP.Client.Views.Instructor.Moduls.AcceptanceAndPaymentByCar();
            window.ShowDialog();
        }

        private void SborAvto_Click(object sender, RoutedEventArgs e)
        {
            var window = new SPP.Client.Views.Instructor.Moduls.CollectionAndDeliveryOfOrdersForCars();
            window.ShowDialog();
        }

        private void Grill_Click(object sender, RoutedEventArgs e)
        {
            var window = new SPP.Client.Views.Instructor.Moduls.Grilling();
            window.ShowDialog();
        }

        private void Frying_Click(object sender, RoutedEventArgs e)
        {
            var window = new SPP.Client.Views.Instructor.Moduls.DeepFrying();
            window.ShowDialog();
        }

        private void Osnovnoemenu_Click(object sender, RoutedEventArgs e)
        {
            var window = new SPP.Client.Views.Instructor.Moduls.PreparingTheMainMenu();
            window.ShowDialog();
        }

        private void Popolnenie_Click(object sender, RoutedEventArgs e)
        {
            var window = new SPP.Client.Views.Instructor.Moduls.AgingAndPreparationOfProducts();
            window.ShowDialog();
        }

        private void Cafe_Click(object sender, RoutedEventArgs e)
        {
            var window = new SPP.Client.Views.Instructor.Moduls.CafeBaking();
            window.ShowDialog();
        }
    }
}
