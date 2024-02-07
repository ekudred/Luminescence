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
        services.RegisterLazySingleton(() => new RosterFormService(
            resolver.GetService<ExpUsbDeviceService>()
        ));
        services.RegisterLazySingleton(() => new OptionsDialogFormService(
            resolver.GetService<ExpUsbDeviceService>()
        ));
        services.RegisterLazySingleton(() => new ExpUsbDeviceService());
        services.RegisterLazySingleton(() => new ExpChartService(
            resolver.GetService<ExpUsbDeviceService>()
        ));
    }

    private static void RegisterViewModels(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton(() => new ChartPanelViewModel(
            resolver.GetService<MainWindowViewModel>(),
            resolver.GetService<ExpChartService>()
        ));
        services.RegisterLazySingleton(() => new MainWindowViewModel());
        services.RegisterLazySingleton(() => new OptionsDialogViewModel(
            resolver.GetService<OptionsDialogFormService>()
        ));
        services.RegisterLazySingleton(() => new RosterViewModel(
            resolver.GetService<RosterFormService>()
        ));
        services.RegisterLazySingleton(() => new ToolBarViewModel(
            resolver.GetService<DialogService>(),
            resolver.GetService<ExpUsbDeviceService>()
        ));
    }
}