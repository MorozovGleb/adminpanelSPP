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

namespace SPP.Client.Views.Instructor.Moduls
{
    /// <summary>
    /// Логика взаимодействия для PreparingTheMainMenu.xaml
    /// </summary>
    public partial class PreparingTheMainMenu : Window
    {
        private int currentPage = 1;
        private const int TotalPages = 15;
        public PreparingTheMainMenu()
        {
            InitializeComponent();
            ShowPage(1);
        }
        private void ShowPage(int page)
        {
            // Скрываем все страницы
            Page1.Visibility = Visibility.Collapsed;
            Page2.Visibility = Visibility.Collapsed;
            Page3.Visibility = Visibility.Collapsed;
            Page4.Visibility = Visibility.Collapsed;
            Page5.Visibility = Visibility.Collapsed;
            Page6.Visibility = Visibility.Collapsed;
            Page7.Visibility = Visibility.Collapsed;
            Page8.Visibility = Visibility.Collapsed;
            Page9.Visibility = Visibility.Collapsed;
            Page10.Visibility = Visibility.Collapsed;
            Page11.Visibility = Visibility.Collapsed;
            Page12.Visibility = Visibility.Collapsed;
            Page13.Visibility = Visibility.Collapsed;
            Page14.Visibility = Visibility.Collapsed;
            PageEnd.Visibility = Visibility.Collapsed;

            // Показываем нужную
            switch (page)
            {
                case 1: Page1.Visibility = Visibility.Visible; break;
                case 2: Page2.Visibility = Visibility.Visible; break;
                case 3: Page3.Visibility = Visibility.Visible; break;
                case 4: Page4.Visibility = Visibility.Visible; break;
                case 5: Page5.Visibility = Visibility.Visible; break;
                case 6: Page6.Visibility = Visibility.Visible; break;
                case 7: Page7.Visibility = Visibility.Visible; break;
                case 8: Page8.Visibility = Visibility.Visible; break;
                case 9: Page9.Visibility = Visibility.Visible; break;
                case 10: Page10.Visibility = Visibility.Visible; break;
                case 11: Page11.Visibility = Visibility.Visible; break;
                case 12: Page12.Visibility = Visibility.Visible; break;
                case 13: Page13.Visibility = Visibility.Visible; break;
                case 14: Page14.Visibility = Visibility.Visible; break;
                case 15: PageEnd.Visibility = Visibility.Visible; break;
            }
            MainScrollViewer.ScrollToTop();
        }

        private void NextPage(object sender, MouseButtonEventArgs e)
        {
            if (currentPage < TotalPages)
            {
                currentPage++;
                ShowPage(currentPage);
            }
        }

        private void PrevPage(object sender, MouseButtonEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                ShowPage(currentPage);
            }
        }

        private void RestartTraining(object sender, MouseButtonEventArgs e)
        {
            currentPage = 1;
            ShowPage(1);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
