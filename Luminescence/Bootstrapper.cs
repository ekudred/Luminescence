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
        services.RegisterLazySingleton(() => new HidService());
        services.RegisterLazySingleton(() => new DialogService(
            resolver.GetService<MainWindowProvider>()
        ));
        services.RegisterLazySingleton(() => new RosterFormService(
            resolver.GetService<ExpDeviceService>()
        ));
        services.RegisterLazySingleton(() => new OptionsDialogFormService(
            resolver.GetService<ExpDeviceService>()
        ));
        services.RegisterLazySingleton(() => new ExpDeviceService(
            resolver.GetService<HidService>(),
            resolver.GetService<MainWindowProvider>(),
            resolver.GetService<DialogService>()
        ));
        services.RegisterLazySingleton(() => new ExpChartService(
            resolver.GetService<ExpDeviceService>()
        ));
    }

    private static void RegisterViewModels(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton(() => new ChartPanelViewModel(
            resolver.GetService<MainWindowViewModel>(),
            resolver.GetService<ExpChartService>()
        ));
        services.RegisterLazySingleton(() => new MainWindowViewModel(
            resolver.GetService<ExpDeviceService>(),
            resolver.GetService<HidService>()
        ));
        services.RegisterLazySingleton(() => new OptionsDialogViewModel(
            resolver.GetService<OptionsDialogFormService>()
        ));
        services.RegisterLazySingleton(() => new RosterViewModel(
            resolver.GetService<ExpDeviceService>(),
            resolver.GetService<RosterFormService>()
        ));
        services.RegisterLazySingleton(() => new ToolBarViewModel(
            resolver.GetService<DialogService>(),
            resolver.GetService<ExpDeviceService>()
        ));
    }
}