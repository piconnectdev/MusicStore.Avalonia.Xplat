using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using iTunesSearch.Library;

namespace MusicStore.Xplat.Models;

public class Album
{
    private static readonly iTunesSearchManager SearchManager = new();

    private static readonly string CacheDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AlbumCache");
    public string Artist { get; set; }
    public string Title { get; set; }
    public string CoverUrl { get; set; }

    public Album(string artist, string title, string coverUrl)
    {
        Artist = artist;
        Title = title;
        CoverUrl = coverUrl;
    }

    public static async Task<IEnumerable<Album>> SearchAsync(string searchTerm)
    {
        var query = await SearchManager.GetAlbumsAsync(searchTerm)
                                       .ConfigureAwait(false);

        return query.Albums.Select(x =>
                                       new Album(x.ArtistName, x.CollectionName,
                                                 x.ArtworkUrl100.Replace("100x100bb", "600x600bb")));
    }

    private static readonly HttpClient HttpClient = new();
    private string CachePath => $"{CacheDir}/{Artist} - {Title}";

    public async Task<Stream> LoadCoverBitmapAsync(CancellationToken cancellationToken = default)
    {
        if(File.Exists(CachePath + ".bmp"))
        {
            return File.OpenRead(CachePath + ".bmp");
        }
        else
        {
            var data = await HttpClient.GetByteArrayAsync(CoverUrl, cancellationToken);
            return new MemoryStream(data);
        }
    }

    public async Task SaveAsync()
    {
        if(!Directory.Exists(CacheDir))
        {
            Directory.CreateDirectory(CacheDir);
        }

        await using var fs = File.OpenWrite(CachePath);
        await SaveToStreamAsync(this, fs);
    }

    public Stream SaveCoverBitmapStream()
    {
        return File.OpenWrite(CachePath + ".bmp");
    }

    private static async Task SaveToStreamAsync(Album data, Stream stream)
    {
        await JsonSerializer.SerializeAsync(stream, data).ConfigureAwait(false);
    }

    public static async Task<Album> LoadFromStream(Stream stream)
    {
        return (await JsonSerializer.DeserializeAsync<Album>(stream).ConfigureAwait(false))!;
    }

    public static async Task<IEnumerable<Album>> LoadCachedAsync()
    {
        if(!Directory.Exists(CacheDir))
        {
            Directory.CreateDirectory(CacheDir);
        }

        var results = new List<Album>();

        foreach(var file in Directory.EnumerateFiles(CacheDir))
        {
            if(!string.IsNullOrWhiteSpace(new DirectoryInfo(file).Extension)) continue;

            await using var fs = File.OpenRead(file);
            results.Add(await LoadFromStream(fs).ConfigureAwait(false));
        }

        return results;
    }
}