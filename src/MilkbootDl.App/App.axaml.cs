using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MilkbootDl.App.Settings;
using MilkbootDl.App.Theming;
using MilkbootDl.App.ViewModels;
using MilkbootDl.App.Views;

namespace MilkbootDl.App;

public partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        // Settings are applied before the first window exists, so it never flashes the default style.
        var settings = AppSettings.Load();
        StyleManager.Apply(this, settings.Style, settings.Theme);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow { DataContext = new MainViewModel(this, settings) };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
