using System.Configuration;
using System.Data;
using System.Windows;
using SPP.Client.DTO;

namespace SPP.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static LoginResponseDto? CurrentUser { get; set; }
    }

}
