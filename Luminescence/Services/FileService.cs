using System;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace Luminescence.Services;

public class FileService
{
    private readonly MainWindowProvider _mainWindowProvider;

    public FileService(MainWindowProvider mainWindowProvider)
    {
        _mainWindowProvider = mainWindowProvider;
    }

    public IObservable<bool> Save(string data, FileSaveOptions options, Visual? visual = null)
    {
        return Observable.Create(async (IObserver<bool> observer) =>
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

            try
            {
                var file = await topLevel.StorageProvider.SaveFilePickerAsync(saveOptions);

                if (file is not null)
                {
                    await using var stream = await file.OpenWriteAsync();
                    using var streamWriter = new StreamWriter(stream);
                    await streamWriter.WriteLineAsync(data);
                }

                observer.OnNext(true);
                observer.OnCompleted();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);

                observer.OnError(exception);
                observer.OnCompleted();
            }

            return Disposable.Empty;
        });
    }
}