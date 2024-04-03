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
        services.RegisterLazySingleton(() => new StorageService());
        services.RegisterLazySingleton(() => new AppFilePickerService(
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
            resolver.GetService<MeasurementSettingsFormViewModel>(),
            resolver.GetService<AppFilePickerService>(),
            resolver.GetService<SystemDialogService>(),
            resolver.GetService<DialogService>()
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
            resolver.GetService<ExpChartService>(),
            resolver.GetService<HidService>(),
            resolver.GetService<MeasurementSettingsFormService>(),
            resolver.GetService<MeasurementSettingsFormViewModel>(),
            resolver.GetService<DialogService>()
        ));
        services.RegisterLazySingleton(() => new HeaderViewModel(
            resolver.GetService<DialogService>(),
            resolver.GetService<ExpDeviceService>(),
            resolver.GetService<ExpChartService>(),
            resolver.GetService<MeasurementSettingsFormService>()
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
}