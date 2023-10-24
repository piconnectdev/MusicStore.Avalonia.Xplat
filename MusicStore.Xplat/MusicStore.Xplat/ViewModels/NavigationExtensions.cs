using System;
using DryIoc;
using ReactiveUI;

namespace MusicStore.Xplat.ViewModels;

public static class NavigationExtensions
{
    public static IObservable<IRoutableViewModel> NavigateAndReset<T>(this RoutingState router, params object[] parameters)
        where T : IRoutableViewModel
        => router
           .NavigateAndReset
           .Execute(Container.Instance.Resolve<T>(parameters));

    public static IObservable<IRoutableViewModel> Navigate<T>(this RoutingState router, params object[] parameters)
        where T : IRoutableViewModel
        => router
           .Navigate
           .Execute(Container.Instance.Resolve<T>(parameters));
}