using System;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace Luminescence.Services;

public class AppFilePickerService
{
    private readonly MainWindowProvider _mainWindowProvider;

    public AppFilePickerService(MainWindowProvider mainWindowProvider)
    {
        _mainWindowProvider = mainWindowProvider;
    }

    public IObservable<object> Open(AppFilePickerOpenOptions options, Visual? visual = null)
    {
        return OpenStorageFile(options, visual)
            .Select(file => OpenText(file)).Switch();
    }

    private IObservable<IStorageFile> OpenStorageFile(AppFilePickerOpenOptions options, Visual? visual = null)
    {
        return Observable.Create(async (IObserver<IStorageFile> observer) =>
        {
            try
            {
                var topLevel = GetTopLevel(visual);

                IStorageFolder suggestedStartLocation =
                    await topLevel.StorageProvider.TryGetFolderFromPathAsync(options.StartLocationDirectory);

                FilePickerOpenOptions openOptions = new()
                {
                    Title = options.Title,
                    FileTypeFilter = options.FileTypeFilter,
                    AllowMultiple = false,
                    SuggestedStartLocation = suggestedStartLocation
                };
                var files = await topLevel.StorageProvider.OpenFilePickerAsync(openOptions);

                if (files == null || files.Count != 1)
                {
                    throw new Exception("File Open Error");
                }

                observer.OnNext(files[0]);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);

                observer.OnError(exception);
            }
            finally
            {
                observer.OnCompleted();
            }

            return Disposable.Empty;
        });
    }

    private IObservable<string> OpenText(IStorageFile file)
    {
        return Observable.Create(async (IObserver<string> observer) =>
        {
            try
            {
                await using var stream = await file.OpenReadAsync();
                using var streamReader = new StreamReader(stream);
                var fileContent = await streamReader.ReadToEndAsync();

                observer.OnNext(fileContent);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);

                observer.OnError(exception);
            }
            finally
            {
                observer.OnCompleted();
            }

            return Disposable.Empty;
        });
    }

    public IObservable<object> Save(string data, AppFilePickerSaveOptions options, Visual? visual = null)
    {
        return SaveStorageFile(options, visual)
            .Select(file => SaveText(file, data)).Switch();
    }

    private IObservable<IStorageFile> SaveStorageFile(AppFilePickerSaveOptions options, Visual? visual = null)
    {
        return Observable.Create(async (IObserver<IStorageFile> observer) =>
        {
            try
            {
                var topLevel = GetTopLevel(visual);

                IStorageFolder suggestedStartLocation =
                    await topLevel.StorageProvider.TryGetFolderFromPathAsync(options.StartLocationDirectory);

                FilePickerSaveOptions saveOptions = new()
                {
                    Title = options.Title,
                    SuggestedFileName = options.FileName,
                    FileTypeChoices = options.FileTypeChoices,
                    DefaultExtension = options.DefaultExtension,
                    SuggestedStartLocation = suggestedStartLocation
                };
                var file = await topLevel.StorageProvider.SaveFilePickerAsync(saveOptions);

                if (file == null)
                {
                    throw new Exception("File not found");
                }

                observer.OnNext(file);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);

                observer.OnError(exception);
            }
            finally
            {
                observer.OnCompleted();
            }

            return Disposable.Empty;
        });
    }

    private IObservable<object> SaveText(IStorageFile file, string data)
    {
        return Observable.Create(async (IObserver<object> observer) =>
        {
            try
            {
                await using var stream = await file.OpenWriteAsync();
                using var streamWriter = new StreamWriter(stream);
                await streamWriter.WriteLineAsync(data);

                observer.OnNext(default);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);

                observer.OnError(exception);
            }
            finally
            {
                observer.OnCompleted();
            }

            return Disposable.Empty;
        });
    }

    private TopLevel GetTopLevel(Visual? visual)
    {
        visual ??= _mainWindowProvider.GetMainWindow();

        return TopLevel.GetTopLevel(visual)!;
    }
}