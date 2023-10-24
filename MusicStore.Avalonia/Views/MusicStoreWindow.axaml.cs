using System;
using System.Reactive.Disposables;
using Avalonia.ReactiveUI;
using MusicStore.Avalonia.ViewModels;
using ReactiveUI;

namespace MusicStore.Avalonia.Views;

public partial class MusicStoreWindow : ReactiveWindow<MusicStoreViewModel>
{
    public MusicStoreWindow()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.WhenAnyObservable(v => v.ViewModel!.BuyMusicCommand)
                .Subscribe(Close)
                .DisposeWith(disposables);
        });
    }
}