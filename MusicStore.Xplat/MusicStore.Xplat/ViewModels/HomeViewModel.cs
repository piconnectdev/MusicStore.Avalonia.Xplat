using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using MusicStore.Xplat.Models;
using ReactiveUI;

namespace MusicStore.Xplat.ViewModels;

public class HomeViewModel : ViewModelBase, IRoutableViewModel, IActivatableViewModel
{
    public ICommand BuyMusicCommand { get; }
    public Interaction<AlbumViewModel?, Unit> AlbumSelectionInteraction { get; } = new();

    public ObservableCollection<AlbumViewModel> Albums { get; } = new();

    public HomeViewModel()
    {
        Activator = new ViewModelActivator();
        BuyMusicCommand = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.Navigate.Execute(new MusicStoreViewModel(AlbumSelectionInteraction)));

        RxApp.MainThreadScheduler.Schedule(LoadAlbums);
        AlbumSelectionInteraction.RegisterHandler(async interactionContext =>
        {
            AlbumViewModel? album = interactionContext.Input;
            if(album != null)
            {
                Albums.Add(album);
                await album.SaveToDiskAsync();
            }
            interactionContext.SetOutput(Unit.Default);
        });
    }

    private async void LoadAlbums()
    {
        var albums = (await Album.LoadCachedAsync()).Select(x => new AlbumViewModel(x));

        foreach(var album in albums)
        {
            Albums.Add(album);
        }

        foreach(var album in Albums.ToList())
        {
            await album.LoadCoverAsync();
        }
    }

    public string UrlPathSegment => "Main Screen";
    public IScreen HostScreen { get; } = MainScreen.Instance;
    public ViewModelActivator Activator { get; }
}