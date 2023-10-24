using System;
using DryIoc;

namespace MusicStore.Xplat;

public static class Container
{
    private static readonly Lazy<IContainer> LazyContainer;

    static Container()
    {
        LazyContainer = new Lazy<IContainer>(() =>
        {
            var container = new DryIoc.Container(Rules.Default.WithConcreteTypeDynamicRegistrations(reuse:Reuse.Transient)
                                                      .With(Made.Of(FactoryMethod.ConstructorWithResolvableArguments))
                                                      .WithFuncAndLazyWithoutRegistration()
                                                      .WithUseInterpretation()
                                                      .WithTrackingDisposableTransients()
                                                      .WithoutThrowOnRegisteringDisposableTransient()
                                                      .WithFactorySelector(Rules.SelectLastRegisteredFactory()));
            container.RegisterInstance<IContainer>(container);
            return container;
        });
    }

    public static IContainer Instance => LazyContainer.Value;
}