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
        RegisterValues(services, resolver);
    }

    private static void RegisterServices(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton(() => new MainWindowProvider());
        services.RegisterLazySingleton(() => new StorageService());
        services.RegisterLazySingleton(() => new FileService(
            resolver.GetService<MainWindowProvider>()
        ));
        services.RegisterLazySingleton(() => new HidService());
        services.RegisterLazySingleton(() => new DialogService(
            resolver.GetService<MainWindowProvider>()
        ));
        services.RegisterLazySingleton(() => new SystemDialogService(
            resolver.GetService<MainWindowProvider>()
        ));
        services.RegisterLazySingleton(() => new MeasurementSettingsFormService(
            resolver.GetService<ExpDeviceService>(),
            resolver.GetService<StorageService>()
        ));
        services.RegisterLazySingleton(() => new ExpDeviceService(
            resolver.GetService<HidService>(),
            resolver.GetService<DialogService>()
        ));
        services.RegisterLazySingleton(() => new ExpChartService(
            resolver.GetService<ExpDeviceService>(),
            resolver.GetService<StorageService>(),
            resolver.GetService<ExpChartsData>()
        ));
    }

    private static void RegisterViewModels(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton(() => new ChartPanelViewModel(
            resolver.GetService<MainWindowViewModel>(),
            resolver.GetService<ExpChartService>(),
            resolver.GetService<ExpDeviceService>()
        ));
        services.RegisterLazySingleton(() => new MainWindowViewModel(
            resolver.GetService<ExpDeviceService>(),
            resolver.GetService<HidService>(),
            resolver.GetService<MeasurementSettingsFormService>(),
            resolver.GetService<MeasurementSettingsFormViewModel>()
        ));
        services.RegisterLazySingleton(() => new HeaderViewModel(
            resolver.GetService<DialogService>(),
            resolver.GetService<SystemDialogService>(),
            resolver.GetService<FileService>(),
            resolver.GetService<ExpDeviceService>(),
            resolver.GetService<MeasurementSettingsFormService>(),
            resolver.GetService<ExpChartsData>()
        ));
        services.RegisterLazySingleton(() => new MeasurementSettingsFormViewModel());
    }

    private static void RegisterDialogViewModels(
        IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver
    )
    {
        services.Register(() => new SettingsDialogViewModel(
            resolver.GetService<MeasurementSettingsFormService>(),
            resolver.GetService<MeasurementSettingsFormViewModel>(),
            resolver.GetService<SystemDialogService>()
        ));
    }

    private static void RegisterValues(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton(() => new ExpChartsData());
    }
}