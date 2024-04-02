using System;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace Luminescence.Services;

public class FilePickerService
{
    private readonly MainWindowProvider _mainWindowProvider;

    public FilePickerService(MainWindowProvider mainWindowProvider)
    {
        _mainWindowProvider = mainWindowProvider;
    }

    public IObservable<object> Save(string data, IFilePickerSaveOptions options, Visual? visual = null)
    {
        return GetStorageFile(options, visual)
            .Select(file => SaveText(file, data)).Switch();
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

    private IObservable<IStorageFile> GetStorageFile(IFilePickerSaveOptions options, Visual? visual = null)
    {
        return Observable.Create(async (IObserver<IStorageFile> observer) =>
        {
            try
            {
                visual ??= _mainWindowProvider.GetMainWindow();

                var topLevel = TopLevel.GetTopLevel(visual);

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

                IStorageFile? file = await topLevel.StorageProvider.SaveFilePickerAsync(saveOptions);

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
}