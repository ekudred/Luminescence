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
        RegisterDialogViewModels(services, resolver);
    }

    private static void RegisterServices(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton(() => new MainWindowProvider());
        services.RegisterLazySingleton(() => new HidService());
        services.RegisterLazySingleton(() => new DialogService(
            resolver.GetService<MainWindowProvider>()
        ));
        services.RegisterLazySingleton(() => new SystemDialogService(
            resolver.GetService<MainWindowProvider>()
        ));
        services.RegisterLazySingleton(() => new MeasurementSettingsFormService(
            resolver.GetService<ExpDeviceService>()
        ));
        services.RegisterLazySingleton(() => new ExpDeviceService(
            resolver.GetService<HidService>(),
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
        services.RegisterLazySingleton(() => new RosterViewModel(
            resolver.GetService<ExpDeviceService>()
        ));
        services.RegisterLazySingleton(() => new HeaderViewModel(
            resolver.GetService<DialogService>(),
            resolver.GetService<SystemDialogService>(),
            resolver.GetService<ExpDeviceService>()
        ));
    }

    private static void RegisterDialogViewModels(
        IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver
    )
    {
        services.Register(() => new SettingsDialogViewModel(
            resolver.GetService<MeasurementSettingsFormService>(),
            resolver.GetService<ExpDeviceService>(),
            resolver.GetService<DialogService>()
        ));
    }
}