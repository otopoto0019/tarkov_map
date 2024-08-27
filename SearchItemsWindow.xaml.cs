using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TarkovMap.util;

namespace TarkovMap;

public partial class SearchItemsWindow : Window
{
    private ShortCutHandler scHandler;
    private TextBlock navigationTextOfSearchResultItemsStakPanel;
    
    public SearchItemsWindow()
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
        Background = new SolidColorBrush(Color.FromArgb(64, 0, 0, 0));

        var watermarkText = new TextBlock
        {
            Text = "Enter item name here",
            FontSize = 20,
            Foreground = Brushes.Gray,
            VerticalAlignment = VerticalAlignment.Center,
            IsHitTestVisible = false
        };
        SearchBox.TextChanged += (_, _) =>
        {
            watermarkText.Visibility = string.IsNullOrEmpty(SearchBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        };
        Canvas.SetTop(watermarkText, 40);
        Canvas.SetLeft(watermarkText, 0);

        navigationTextOfSearchResultItemsStakPanel = new TextBlock
        {
            Text = "Will display search result items",
            FontSize = 20,
            Foreground = Brushes.Beige,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            IsHitTestVisible = false
        };
        var textSize = MeasureTextSize(navigationTextOfSearchResultItemsStakPanel.Text, new FontFamily("Arial"), 20, FontStyles.Normal, FontWeights.Normal);
        
        Canvas.SetTop(navigationTextOfSearchResultItemsStakPanel, (120 + 880) / 2);
        Canvas.SetLeft(navigationTextOfSearchResultItemsStakPanel, (480 / 2) - textSize.Width / 2);
        
        MainCanvas.Children.Add(watermarkText);
        MainCanvas.Children.Add(navigationTextOfSearchResultItemsStakPanel);
    }
    
    private void WindowLoaded(object sender, RoutedEventArgs e)
    {
        scHandler.ShortCutRegister();
    }

    private void OnEnterKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
        {
            return;
        }
        
        if (ResultItemsStackPanel.Children.Count > 0)
        {
            ResultItemsStackPanel.Children.Clear();
        }
        
        var textBox = sender as TextBox;

        if (textBox.Text == "" || textBox.Text.Length < 3)
        {
            ResultItemsStackPanel.Children.Clear();
            navigationTextOfSearchResultItemsStakPanel.Text = "文字数は三文字以上";
            var textSize = MeasureTextSize(navigationTextOfSearchResultItemsStakPanel.Text, new FontFamily("Arial"), 20, FontStyles.Normal, FontWeights.Normal);
            Canvas.SetLeft(navigationTextOfSearchResultItemsStakPanel, 480 / 2 - textSize.Width / 2);
            navigationTextOfSearchResultItemsStakPanel.Visibility = Visibility.Visible;
            return;
        }
        
        var resultItems = TarkovAPIWrapper.ItemService.GetItemsInfoByItemName(textBox.Text);
        
        if (!(resultItems.Count > 0))
        {
            ResultItemsStackPanel.Children.Clear();
            navigationTextOfSearchResultItemsStakPanel.Text = "この文字列を含むアイテムは存在しない";
            var textSize = MeasureTextSize(navigationTextOfSearchResultItemsStakPanel.Text, new FontFamily("Arial"), 20, FontStyles.Normal, FontWeights.Normal);
            Canvas.SetLeft(navigationTextOfSearchResultItemsStakPanel, 480 / 2 - textSize.Width / 2);
            navigationTextOfSearchResultItemsStakPanel.Visibility = Visibility.Visible;
            return;
        }

        foreach (var resultItem in resultItems)
        {
            var panel = new Border
            {
                Background = Brushes.Gray,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Width = ResultItemsStackPanel.Width,
                Height = 50,
                Child = new TextBlock
                {
                    Text = resultItem.name,
                    FontSize = 18,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left,
                }
            };
            panel.MouseLeftButtonDown += async (sender, e) =>
            {
                if (ItemInfoGrid.Children.Count > 0)
                {
                    ItemInfoGrid.Children.Clear();
                    ItemInfoGrid.RowDefinitions.Clear();
                    ItemInfoGrid.ColumnDefinitions.Clear();
                }
                
                for (int i = 0; i < 6; i++)
                {
                    ItemInfoGrid.RowDefinitions.Add(new RowDefinition());
                }

                for (int i = 0; i < 2; i++)
                {
                    ItemInfoGrid.ColumnDefinitions.Add(new ColumnDefinition());
                }
                
                var sellingInfo = await TarkovAPIWrapper.ItemService.GetItemSalesInfoByItemId(resultItem.id);

                var textBlocks = new List<Border>();
                Border border1 = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0,4,4,0),
                    Child = new TextBlock { Text = "ID", FontSize = 30, Background = Brushes.Red }
                };

                Border border2 = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0,4,4,0),
                    Child = new TextBlock { Text = "NAME", FontSize = 30, Background = Brushes.Red }
                };

                Border border3 = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0,4,4,0),
                    Child = new TextBlock { Text = "SHORT NAME", FontSize = 30, Background = Brushes.Red }
                };
                
                Border border11 = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0,4,4,0),
                    Child = new TextBlock { Text = "avg24price", FontSize = 30, Background = Brushes.Red }
                };
                
                Border border12 = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0,4,4,0),
                    Child = new TextBlock { Text = "price", FontSize = 30, Background = Brushes.Red }
                };
                
                Border border13 = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0,4,4,0),
                    Child = new TextBlock { Text = "source", FontSize = 30, Background = Brushes.Red }
                };


                textBlocks.Add(border1);
                textBlocks.Add(border2);
                textBlocks.Add(border3);
                textBlocks.Add(border11);
                textBlocks.Add(border12);
                textBlocks.Add(border13);
                
                
                Grid.SetRow(textBlocks[0], 0);
                Grid.SetColumn(textBlocks[0], 0);
                ItemInfoGrid.Children.Add(textBlocks[0]);
                Grid.SetRow(textBlocks[1], 1);
                Grid.SetColumn(textBlocks[1], 0);
                ItemInfoGrid.Children.Add(textBlocks[1]);
                Grid.SetRow(textBlocks[2], 2);
                Grid.SetColumn(textBlocks[2], 0);
                ItemInfoGrid.Children.Add(textBlocks[2]);
                
                Grid.SetRow(textBlocks[3], 3);
                Grid.SetColumn(textBlocks[3], 0);
                ItemInfoGrid.Children.Add(textBlocks[3]);
                Grid.SetRow(textBlocks[4], 4);
                Grid.SetColumn(textBlocks[4], 0);
                ItemInfoGrid.Children.Add(textBlocks[4]);
                Grid.SetRow(textBlocks[5], 5);
                Grid.SetColumn(textBlocks[5], 0);
                ItemInfoGrid.Children.Add(textBlocks[5]);
                
                Border border4 = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0,4,4,0),
                    Child = new TextBlock { Text = resultItem.id, FontSize = 30, Background = Brushes.Transparent }
                };

                Border border5 = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0,4,4,0),
                    Child = new TextBlock { Text = resultItem.name, FontSize = 30, Background = Brushes.Transparent }
                };

                Border border6 = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0,4,4,0),
                    Child = new TextBlock { Text = resultItem.shortName, FontSize = 30, Background = Brushes.Transparent }
                };
                
                Border border14 = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0,4,4,0),
                    Child = new TextBlock { Text = sellingInfo.avg24hPrice.ToString(), FontSize = 30, Background = Brushes.Transparent }
                };
                
                Border border15 = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0,4,4,0),
                    Child = new TextBlock { Text = sellingInfo.sellingPricesAndSource["fence"].ToString(), FontSize = 30, Background = Brushes.Transparent }
                };
                
                Border border16 = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0,4,4,0),
                    Child = new TextBlock { Text = "fence", FontSize = 30, Background = Brushes.Transparent }
                };
                
                textBlocks.Clear();
                textBlocks.Add(border4);
                textBlocks.Add(border5);
                textBlocks.Add(border6);
                textBlocks.Add(border14);
                textBlocks.Add(border15);
                textBlocks.Add(border16);

                Grid.SetRow(textBlocks[0], 0);
                Grid.SetColumn(textBlocks[0], 1);
                ItemInfoGrid.Children.Add(textBlocks[0]);
                Grid.SetRow(textBlocks[1], 1);
                Grid.SetColumn(textBlocks[1], 1);
                ItemInfoGrid.Children.Add(textBlocks[1]);
                Grid.SetRow(textBlocks[2], 2);
                Grid.SetColumn(textBlocks[2], 1);
                ItemInfoGrid.Children.Add(textBlocks[2]);
                
                Grid.SetRow(textBlocks[3], 3);
                Grid.SetColumn(textBlocks[3], 1);
                ItemInfoGrid.Children.Add(textBlocks[3]);
                Grid.SetRow(textBlocks[4], 4);
                Grid.SetColumn(textBlocks[4], 1);
                ItemInfoGrid.Children.Add(textBlocks[4]);
                Grid.SetRow(textBlocks[5], 5);
                Grid.SetColumn(textBlocks[5], 1);
                ItemInfoGrid.Children.Add(textBlocks[5]);

                
                ItemImage.Source = new BitmapImage(new Uri("pack://application:,,,/assets/icon/apexunt.png"));
            };
            ResultItemsStackPanel.Children.Add(panel);
        }

        navigationTextOfSearchResultItemsStakPanel.Visibility = Visibility.Collapsed;
    }

    private Size MeasureTextSize(string text, FontFamily fontFamily, double fontSize, FontStyle fontStyle, FontWeight fontWeight)
    {
        FormattedText formattedText = new FormattedText(
            text,
            System.Globalization.CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            new Typeface(fontFamily, fontStyle, fontWeight, FontStretches.Normal),
            fontSize,
            Brushes.Black,
            new NumberSubstitution(),
            VisualTreeHelper.GetDpi(this).PixelsPerDip);
        
        return new Size(formattedText.Width, formattedText.Height);
    }

    private void HandleBackButton(object sender, RoutedEventArgs e)
    {
        scHandler.Close();
        var window = new MainWindow();
        window.Show();
        Close();
    }
}