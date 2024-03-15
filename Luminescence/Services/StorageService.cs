using System;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Luminescence.Services;

public class StorageService
{
    public IObservable<T?> Get<T>(string storageName)
    {
        return Read<T>(storageName);
    }

    public IObservable<T> Set<T>(string storageName, T data)
    {
        return Create(storageName, data);
    }

    private IObservable<T> Create<T>(string storageName, T data)
    {
        return Observable.Create(async (IObserver<T> observer) =>
        {
            Delete(storageName);

            string storagePath = GetStoragePath(storageName);

            await using (StreamWriter streamWriter = File.CreateText(storagePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(streamWriter, data);
            }

            observer.OnNext(data);
            observer.OnCompleted();
        });
    }

    private IObservable<T?> Read<T>(string storageName)
    {
        return Observable.Create(async (IObserver<T?> observer) =>
        {
            string storagePath = GetStoragePath(storageName);

            if (!File.Exists(storagePath))
            {
                observer.OnNext(default!);
                observer.OnCompleted();

                return Disposable.Empty;
            }

            string json = await File.ReadAllTextAsync(storagePath);
            JObject data = JObject.Parse(json);
            T? result = data.ToObject<T>();

            observer.OnNext(result);
            observer.OnCompleted();

            return Disposable.Empty;
        });
    }

    private void Delete(string storageName)
    {
        string storagePath = GetStoragePath(storageName);

        if (!File.Exists(storagePath))
        {
            return;
        }

        File.Delete(storagePath);
    }

    private string GetStoragePath(string storageName)
    {
        string solutionName = Path
            .GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName)
            .Replace(".exe", string.Empty);

        return $"_{solutionName}.{storageName}.json";
    }
}