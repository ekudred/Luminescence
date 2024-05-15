using System.Reactive.Linq;

namespace Luminescence.Shared.Utils;

public static class ObservableUtil
{
    public static IObservable<(T? Previous, T? Current)> CombineWithPrevious<T>(this IObservable<T> source)
    {
        (T? Previous, T? Current) seed = (default, default);

        return source.Scan(seed, (combination, latest) => (combination.Current, latest));
    }
}