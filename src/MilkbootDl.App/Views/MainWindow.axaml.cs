using Avalonia.Controls;
using MilkbootDl.App.ViewModels;

namespace MilkbootDl.App.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Opened += async (_, _) =>
        {
            if (DataContext is MainViewModel main)
            {
                await main.Ships.LoadAsync();
            }
        };
    }
}
