using System.Reactive.Disposables;
using Avalonia.ReactiveUI;
using MusicStore.Xplat.ViewModels;
using ReactiveUI;

namespace MusicStore.Xplat.Views;

public partial class MainView : ReactiveUserControl<MainScreen>
{
    public MainView()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.Router, v => v.RoutedViewHost.Router)
                .DisposeWith(disposables);
        });
    }
}