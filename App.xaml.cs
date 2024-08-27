using System.Configuration;
using System.Data;
using System.Windows;

namespace TarkovMap;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private void App_OnStartup(object sender, StartupEventArgs e)
    {
        TarkovAPIWrapper.InitializationTarkovData.Initialization();
    }
}