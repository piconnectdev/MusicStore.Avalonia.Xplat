using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using MusicStore.Xplat.Models;
using ReactiveUI.Fody.Helpers;

namespace MusicStore.Xplat.ViewModels;

public class AlbumViewModel : ViewModelBase
{
    private readonly Album _album;

    public AlbumViewModel(Album album)
    {
        _album = album;
        Artist = album.Artist;
        Title = album.Title;
    }

    public string Artist { get; }
    public string Title { get; }

    [Reactive]
    public Bitmap? Cover { get; set; }

    public async Task LoadCoverAsync(CancellationToken cancellationToken = default)
    {
        await using var imageStream = await _album.LoadCoverBitmapAsync(cancellationToken);
        try
        {
            Cover =  await Task.Run(() => new Bitmap(imageStream), cancellationToken);
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task SaveToDiskAsync()
    {
        await _album.SaveAsync();

        if (Cover != null)
        {
            var bitmap = Cover;

            await Task.Run(() =>
            {
                using var fs = _album.SaveCoverBitmapStream();
                bitmap.Save(fs);
            });
        }
    }
}