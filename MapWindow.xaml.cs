using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TarkovMap.util;

namespace TarkovMap;

public partial class MapWindow : Window
{
    private string MapPath = "pack://application:,,,/assets/maps/";
    private ShortCutHandler scHandler;
    
    public MapWindow(string MapName)
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
        
        Background = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0));
        MapPath += Constants.MAP_PATH_LIST[Constants.MAPS_OF_TARKOV.IndexOf(MapName)] + ".jpg";

        Uri uri = new Uri(MapPath);
        MapImage.Source = new BitmapImage(uri);
    }

    private void WindowLoaded(object sender, RoutedEventArgs e)
    {
        scHandler.ShortCutRegister();
    }

    private void OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        GeneralTransform transform = MapImage.TransformToAncestor(this);
        Point canvasPosition = transform.Transform(new Point(0, 0));
        double height = MapImage.ActualHeight;
        double width = MapImage.ActualWidth;

        Rect bounds = new Rect(canvasPosition, new Vector(width, height));

        if (!bounds.Contains(e.GetPosition(this)))
        {
            scHandler.Close();
            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }
    }
}