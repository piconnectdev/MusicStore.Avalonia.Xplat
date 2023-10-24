using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MusicStore.Xplat.ViewModels;

namespace MusicStore.Xplat.Views;

public partial class HomeView : ReactiveUserControl<HomeViewModel>
{
    public HomeView()
    {
        InitializeComponent();
    }
}