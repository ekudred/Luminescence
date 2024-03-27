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
    public IObservable<bool> Save(Visual? visual = null)
    {
        return Observable.Create(async (IObserver<bool> observer) =>
        {
            var topLevel = TopLevel.GetTopLevel(visual);

            var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Save Text File"
            });

            if (file is not null)
            {
                await using var stream = await file.OpenWriteAsync();
                using var streamWriter = new StreamWriter(stream);
                await streamWriter.WriteLineAsync("Hello World!");
            }

            observer.OnNext(true);
            observer.OnCompleted();

            return Disposable.Empty;
        });
    }
}