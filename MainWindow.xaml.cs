using System.Windows;
using System.Windows.Media;
using TarkovMap.util;
using Application = System.Windows.Application;
using Color = System.Windows.Media.Color;

namespace TarkovMap;

public partial class MainWindow : Window
{
    private ShortCutHandler scHandler;
    
    public MainWindow()
    {
        InitializeComponent();
        scHandler = new ShortCutHandler(this, (w,n) =>
        {
            if (!n.Visible)
            {
                w.Hide();
                n.Visible = true;
            }
            else
            {
                w.Show();
                n.Visible = false;
            }
        });

        Background = new SolidColorBrush(Color.FromArgb(64, 0,0,0));
    }

    private void HandleExitButton(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
        scHandler.Close();
    }

    private void HandleMapsButton(object sender, RoutedEventArgs e)
    {
        scHandler.Close();
        SelectingMapsWindow window = new SelectingMapsWindow();
        window.Show();
        Close();
    }

    private void WindowLoaded(object sender, RoutedEventArgs e)
    {
        scHandler.ShortCutRegister();
    }
}