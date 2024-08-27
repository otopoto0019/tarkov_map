using System.Windows;
using System.Windows.Controls;
using TarkovMap.util;

namespace TarkovMap;

public partial class SelectingMapsWindow : Window
{
    private ShortCutHandler scHandler;
    
    public SelectingMapsWindow()
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
    }
    
    private void WindowLoaded(object sender, RoutedEventArgs e)
    {
        scHandler.ShortCutRegister();
        
        List<string> maps = Constants.MAPS_OF_TARKOV;
        
        MainGrid.RowDefinitions.Clear();
        for (int i = 0; i < maps.Count + 1; i++)
        {
            RowDefinition rowDef = new RowDefinition();
            rowDef.Height = new GridLength(Constants.MAP_GRID_HEIGHT);
            MainGrid.RowDefinitions.Add(rowDef);

            Button button = new Button();
            if (i == maps.Count)
            {
                button.Content = "MIN";
                button.Click += HandleMinButton;
            }
            else
            {
                button.Content = maps[i];
                button.Click += HandleEachMapButton;
            }
            
            Grid.SetRow(button, MainGrid.RowDefinitions.Count - 1);
            MainGrid.Children.Add(button);
        }
        

        MainGrid.Width = Constants.MAP_GRID_WIDTH;
        MainGrid.Height = Constants.MAP_GRID_HEIGHT * (maps.Count + 1);
        Width = Constants.MAP_GRID_WIDTH;
        Height = Constants.MAP_GRID_HEIGHT * (maps.Count + 1);
    }

    private void HandleMinButton(object sender, RoutedEventArgs e)
    {
        scHandler.Close();
        MainWindow window = new MainWindow();
        window.Show();
        Close();
    }

    private void HandleEachMapButton(object sender, RoutedEventArgs e)
    {
        scHandler.Close();
        Button button = (Button)sender;
        MapWindow window = new MapWindow(button.Content.ToString());
        window.Show();
        Close();
        scHandler.Close();
    }
}