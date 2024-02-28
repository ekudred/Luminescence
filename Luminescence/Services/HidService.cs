using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using Luminescence.Usb;

namespace Luminescence.Services;

public class HidService
{
    private readonly object _locker = new();

    public IObservable<int> Init()
    {
        return Observable.Create((IObserver<int> observer) =>
        {
            lock (_locker)
            {
                int result = HidApi.hid_init();

                if (result < 0)
                {
                    observer.OnError(new Exception("Failed to init"));
                    observer.OnCompleted();

                    return Disposable.Empty;
                }

                observer.OnNext(result);
                observer.OnCompleted();

                return Disposable.Empty;
            }
        });
    }

    public IObservable<int> Exit()
    {
        return Observable.Create((IObserver<int> observer) =>
        {
            lock (_locker)
            {
                int result = HidApi.hid_exit();

                if (result < 0)
                {
                    observer.OnError(new Exception("Failed to exit"));
                    observer.OnCompleted();

                    return Disposable.Empty;
                }

                observer.OnNext(result);
                observer.OnCompleted();

                return Disposable.Empty;
            }
        });
    }

    public IObservable<nint> Open(ushort vendorId, ushort productId, string serialNumber)
    {
        return Observable.Create((IObserver<nint> observer) =>
        {
            lock (_locker)
            {
                nint result = HidApi.hid_open(vendorId, productId, serialNumber);

                if (result == null)
                {
                    observer.OnError(new Exception("Failed to open"));
                    observer.OnCompleted();

                    return Disposable.Empty;
                }

                observer.OnNext(result);
                observer.OnCompleted();

                return Disposable.Empty;
            }
        });
    }

    public IObservable<bool> Close(nint deviceHandle)
    {
        return Observable.Create((IObserver<bool> observer) =>
        {
            lock (_locker)
            {
                HidApi.hid_close(deviceHandle);

                observer.OnNext(true);
                observer.OnCompleted();

                return Disposable.Empty;
            }
        });
    }

    public IObservable<int> Write(nint deviceHandle, byte[] data)
    {
        return Observable.Create((IObserver<int> observer) =>
        {
            lock (_locker)
            {
                byte[] report = new byte[data.Length];
                Array.Copy(data, report, report.Length);

                int result = HidApi.hid_write(deviceHandle, report, (uint)report.Length);

                if (result < 0)
                {
                    observer.OnError(new Exception("Failed to write"));
                    observer.OnCompleted();

                    return Disposable.Empty;
                }

                observer.OnNext(result);
                observer.OnCompleted();

                return Disposable.Empty;
            }
        });
    }

    public IObservable<byte[]> Read(nint deviceHandle, int reportLength)
    {
        return Observable.Create((IObserver<byte[]> observer) =>
        {
            lock (_locker)
            {
                if (reportLength <= 0)
                {
                    observer.OnError(new Exception("Incorrect report length"));
                    observer.OnCompleted();

                    return Disposable.Empty;
                }

                byte[] report = new byte[reportLength];

                int result = HidApi.hid_read_timeout(deviceHandle, report, (uint)report.Length, 1);

                if (result < 0)
                {
                    observer.OnError(new Exception("Failed to read"));
                    observer.OnCompleted();

                    return Disposable.Empty;
                }

                byte[] data = new byte[result];
                Array.Copy(report, 0, data, 0, result);

                observer.OnNext(data);
                observer.OnCompleted();

                return Disposable.Empty;
            }
        });
    }

    public IObservable<nint> Enumerate(ushort vendorId, ushort productId)
    {
        return Observable.Create((IObserver<nint> observer) =>
        {
            lock (_locker)
            {
                nint result = HidApi.hid_enumerate(vendorId, productId);

                if (result == null)
                {
                    observer.OnError(new Exception("Failed to enumerate"));
                    observer.OnCompleted();

                    return Disposable.Empty;
                }

                observer.OnNext(result);
                observer.OnCompleted();

                return Disposable.Empty;
            }
        });
    }

    public IObservable<string> GetManufacturerString(nint deviceHandle)
    {
        return Observable.Create((IObserver<string> observer) =>
        {
            StringBuilder builder = new StringBuilder(1024);

            int result = HidApi.hid_get_manufacturer_string(deviceHandle, builder, (uint)builder.Capacity / 4);

            if (result < 0)
            {
                observer.OnError(new Exception("Failed to get manufacturer string"));
                observer.OnCompleted();

                return Disposable.Empty;
            }

            observer.OnNext(builder.ToString());
            observer.OnCompleted();

            return Disposable.Empty;
        });
    }

    public IObservable<string> GetProductString(nint deviceHandle)
    {
        return Observable.Create((IObserver<string> observer) =>
        {
            StringBuilder builder = new StringBuilder(1024);

            int result = HidApi.hid_get_product_string(deviceHandle, builder, (uint)builder.Capacity / 4);

            if (result < 0)
            {
                observer.OnError(new Exception("Failed to get product string"));
                observer.OnCompleted();

                return Disposable.Empty;
            }

            observer.OnNext(builder.ToString());
            observer.OnCompleted();

            return Disposable.Empty;
        });
    }

    public IObservable<string> GetSerialNumberString(nint deviceHandle)
    {
        return Observable.Create((IObserver<string> observer) =>
        {
            StringBuilder builder = new StringBuilder(1024);

            int result = HidApi.hid_get_serial_number_string(deviceHandle, builder, (uint)builder.Capacity / 4);

            if (result < 0)
            {
                observer.OnError(new Exception("Failed to get serial number string"));
                observer.OnCompleted();

                return Disposable.Empty;
            }

            observer.OnNext(builder.ToString());
            observer.OnCompleted();

            return Disposable.Empty;
        });
    }
}