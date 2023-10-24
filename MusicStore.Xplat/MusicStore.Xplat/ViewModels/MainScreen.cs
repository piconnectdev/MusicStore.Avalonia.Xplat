using ReactiveUI;

namespace MusicStore.Xplat.ViewModels;

public class MainScreen : ReactiveObject, IScreen
{
    public static MainScreen Instance { get; } = new();
    public RoutingState Router { get; } = new();
}