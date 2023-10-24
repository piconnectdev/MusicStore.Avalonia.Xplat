using System.Reactive.Disposables;
using Avalonia.ReactiveUI;
using MusicStore.Xplat.ViewModels;
using ReactiveUI;

namespace MusicStore.Xplat.Views;

public partial class MusicStoreView : ReactiveUserControl<MusicStoreViewModel>
{
    public MusicStoreView()
    {
        InitializeComponent();
        // this.WhenActivated(disposables =>
        // {
        //     this.WhenAnyObservable(v => v.ViewModel!.BuyMusicCommand)
        //         .Subscribe(Close)
        //         .DisposeWith(disposables);
        // });
    }
}