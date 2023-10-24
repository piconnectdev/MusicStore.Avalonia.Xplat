using Avalonia;
using DryIoc;
using MusicStore.Xplat.ViewModels;
using MusicStore.Xplat.Views;
using ReactiveUI;
using Splat.DryIoc;

namespace MusicStore.Xplat;

public static class Startup
{
    public static AppBuilder InitializeMusicStore(this AppBuilder builder)
    {
        var container = Container.Instance;
        container.AddNavigation()
                 .UseDryIocDependencyResolver();
        return builder;
    }

    private static IContainer AddNavigation(this IContainer container)
    {
        container.Register<IViewFor<HomeViewModel>, HomeView>();
        container.Register<IViewFor<MusicStoreViewModel>, MusicStoreView>();
        return container;
    }
}