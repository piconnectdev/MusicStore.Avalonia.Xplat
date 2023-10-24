# Overview

This is the cross-platform alternative of the Avalonia's official [MusicStore](https://docs.avaloniaui.net/docs/next/tutorials/music-store-app/) sample application that targets both Desktop (Mac, Windows, Linux) and Mobile (Android, iOS) platforms.

## Changes from the original sample

To be able truly cross-platform, the following changes were made:

1. We can't use multi-window approach on mobile platforms, so the app is single-windowed on mobile platforms. This means we had to sacrifice the dialog-approach of showing the `MusicStore`
2. Instead of dialogs we've used ReactiveUI's routing system in combination with `RoutedViewHost` and `Interactions` to handle the back navigation with `Album` from the `MusicStoreView` to the main window where saved albums are displayed. In the future, we may integrate Avalonia.DialogViewHost to get cross-platform dialog implementation
3. Instead of using hardcoded `.\Cache` path for saving data, we use platform-specific `Environment.SpecialFolder.LocalApplicationData` path
4. I usually prefer `DryIoc` as my main DI container. So, the special `Splat.DryIoc` nuget was used to replace the ReactiveUI's default DI container with Dry.Ioc