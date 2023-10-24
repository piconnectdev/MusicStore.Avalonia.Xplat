using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MusicStore.Xplat.ViewModels;
using MusicStore.Xplat.Views;
using ReactiveUI;
using Splat;

namespace MusicStore.Xplat;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var mainScreen = MainScreen.Instance;
        switch(ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                desktop.MainWindow = new MainWindow
                {
                    DataContext = mainScreen
                };
                break;
            case ISingleViewApplicationLifetime singleViewPlatform:
                singleViewPlatform.MainView = new MainView
                {
                    DataContext = mainScreen
                };
                break;
        }

        mainScreen.Router.NavigateAndReset<HomeViewModel>();
        base.OnFrameworkInitializationCompleted();
    }
}