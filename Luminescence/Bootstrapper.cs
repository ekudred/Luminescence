using Luminescence.Dialog;
using Luminescence.Services;
using Luminescence.ViewModels;
using Splat;

namespace Luminescence;

public static class Bootstrapper
{
    public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        RegisterServices(services, resolver);
        RegisterViewModels(services, resolver);
    }

    private static void RegisterServices(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton(() => new MainWindowProvider());
        services.RegisterLazySingleton(() => new DialogService(
            resolver.GetService<MainWindowProvider>()
        ));
        services.RegisterLazySingleton(() => new RosterFormService());
        services.RegisterLazySingleton(() => new OptionsDialogFormService());
    }

    private static void RegisterViewModels(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton(() => new ChartPanelViewModel(
            resolver.GetService<MainWindowViewModel>()
        ));
        services.RegisterLazySingleton(() => new MainWindowViewModel());
        services.RegisterLazySingleton(() => new OptionsDialogViewModel(
            resolver.GetService<OptionsDialogFormService>()
        ));
        services.RegisterLazySingleton(() => new RosterViewModel(
            resolver.GetService<RosterFormService>()
        ));
        services.RegisterLazySingleton(() => new ToolBarViewModel(
            resolver.GetService<DialogService>()
        ));
    }
}