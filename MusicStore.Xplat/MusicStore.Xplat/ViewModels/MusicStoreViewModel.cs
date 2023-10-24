using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using MusicStore.Xplat.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MusicStore.Xplat.ViewModels;

public class MusicStoreViewModel : ViewModelBase, IRoutableViewModel
{
    private ReactiveCommand<string?, Unit> SearchCommand { get; }
    private CancellationTokenSource? _cancellationTokenSource;

    public MusicStoreViewModel(IInteraction<AlbumViewModel?, Unit> albumSelectionInteraction)
    {
        SearchCommand = ReactiveCommand.CreateFromObservable<string?, Unit>(searchText => Observable.StartAsync(ct => SearchAsync(searchText)));
        this.WhenAnyValue(vm => vm.SearchText)
            .Throttle(TimeSpan.FromMilliseconds(400))
            .ObserveOn(RxApp.MainThreadScheduler)
            .InvokeCommand(SearchCommand);

        BuyMusicCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await albumSelectionInteraction.Handle(SelectedAlbum);
            HostScreen.Router.NavigateBack.Execute();
        });
    }

    [Reactive]
    public string? SearchText { get; set; }

    [Reactive]
    public bool IsBusy { get; set; }

    public ObservableCollection<AlbumViewModel> SearchResults { get; } = new();

    [Reactive]
    public AlbumViewModel? SelectedAlbum { get; set; }

    public ReactiveCommand<Unit, Unit> BuyMusicCommand { get; }

    private async Task SearchAsync(string? searchText)
    {
        IsBusy = true;
        SearchResults.Clear();

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = _cancellationTokenSource.Token;

        try
        {
            if(!string.IsNullOrWhiteSpace(searchText))
            {
                var albums = await Album.SearchAsync(searchText);
                foreach(var album in albums)
                {
                    SearchResults.Add(new AlbumViewModel(album));
                }

                if(!cancellationToken.IsCancellationRequested)
                {
                    LoadCovers(cancellationToken);
                }
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async void LoadCovers(CancellationToken cancellationToken)
    {
        try
        {
            foreach(AlbumViewModel albumViewModel in SearchResults.ToList())
            {
                await albumViewModel.LoadCoverAsync(cancellationToken);
                if(cancellationToken.IsCancellationRequested)
                {
                    return;
                }
            }
        }
        catch(TaskCanceledException)
        {
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public string UrlPathSegment => "Music Store";
    public IScreen HostScreen { get; } = MainScreen.Instance;
}