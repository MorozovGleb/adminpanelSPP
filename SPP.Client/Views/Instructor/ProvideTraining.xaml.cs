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
            window.Show();
        }

        private void Potatoes_Click(object sender, RoutedEventArgs e)
        {
            var window = new SPP.Client.Views.Instructor.Moduls.CookingPotatoes();
            window.Show();
        }
    }
}
