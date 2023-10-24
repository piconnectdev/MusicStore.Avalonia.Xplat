using System.Reactive.Disposables;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using MusicStore.Avalonia.ViewModels;
using ReactiveUI;

namespace MusicStore.Avalonia.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.BindInteraction(ViewModel, vm => vm.ShowMusicStoreDialog, ShowMusicStoreDialogAsync)
                .DisposeWith(disposables);
        });
    }

    private async Task ShowMusicStoreDialogAsync(IInteractionContext<MusicStoreViewModel, AlbumViewModel?> interaction)
    {
        var dialog = new MusicStoreWindow
        {
            DataContext = interaction.Input
        };
        var result = await dialog.ShowDialog<AlbumViewModel?>(this);
        interaction.SetOutput(result);
    }
}