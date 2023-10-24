using Avalonia.ReactiveUI;
using MusicStore.Avalonia.ViewModels;

namespace MusicStore.Avalonia.Views;

public partial class MusicStoreView : ReactiveUserControl<MusicStoreViewModel>
{
    public MusicStoreView()
    {
        InitializeComponent();
    }
}