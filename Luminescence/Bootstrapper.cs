using Luminescence.Dialog;
using Luminescence.Services;
using Luminescence.UsbHid;
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
        services.RegisterLazySingleton(() => new ClipboardService(
            resolver.GetService<MainWindowProvider>()
        ));
        services.RegisterLazySingleton(() => new UsbHidService());
        services.RegisterLazySingleton(() => new DialogService(
            resolver.GetService<MainWindowProvider>()
        ));
        services.RegisterLazySingleton(() => new MeasurementSettingsFormService(
            resolver.GetService<ExpDevice>(),
            resolver.GetService<StorageService>()
        ));
        services.RegisterLazySingleton(() => new ExpDevice(
            resolver.GetService<UsbHidService>(),
            resolver.GetService<DialogService>()
        ));
        services.RegisterLazySingleton(() => new ExpChartService(
            resolver.GetService<ExpDevice>(),
            resolver.GetService<MeasurementSettingsFormViewModel>(),
            resolver.GetService<AppFilePickerService>(),
            resolver.GetService<ClipboardService>(),
            resolver.GetService<DialogService>()
        ));
    }

    private static void RegisterViewModels(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton(() => new ChartPanelViewModel(
            resolver.GetService<MainWindowViewModel>(),
            resolver.GetService<ExpChartService>()
        ));
        services.RegisterLazySingleton(() => new MainWindowViewModel(
            resolver.GetService<ExpDevice>(),
            resolver.GetService<ExpChartService>(),
            resolver.GetService<UsbHidService>(),
            resolver.GetService<MeasurementSettingsFormService>(),
            resolver.GetService<MeasurementSettingsFormViewModel>(),
            resolver.GetService<DialogService>()
        ));
        services.RegisterLazySingleton(() => new HeaderViewModel(
            resolver.GetService<DialogService>(),
            resolver.GetService<ExpDevice>(),
            resolver.GetService<ExpChartService>(),
            resolver.GetService<MeasurementSettingsFormService>()
        ));
        services.RegisterLazySingleton(() => new MeasurementSettingsFormViewModel());
        services.RegisterLazySingleton(() => new TestDialogViewModel(
            resolver.GetService<ExpDevice>()
        ));
    }

    private static void RegisterDialogViewModels(
        IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver
    )
    {
        services.Register(() => new SettingsDialogViewModel(
            resolver.GetService<MeasurementSettingsFormService>(),
            resolver.GetService<MeasurementSettingsFormViewModel>(),
            resolver.GetService<DialogService>()
        ));
    }
}