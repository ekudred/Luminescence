using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input.Platform;

namespace Luminescence.Services;

public class ClipboardService
{
    private readonly MainWindowProvider _mainWindowProvider;

    public ClipboardService(MainWindowProvider mainWindowProvider)
    {
        _mainWindowProvider = mainWindowProvider;
    }

    public IObservable<Unit> SetText(string text, Visual? visual = null)
    {
        TopLevel topLevel = GetTopLevel(visual);
        IClipboard? clipboard = topLevel.Clipboard;

        if (clipboard == null)
        {
            return Observable.Empty<Unit>();
        }

        return clipboard.SetTextAsync(text).ToObservable();
    }

    private TopLevel GetTopLevel(Visual? visual)
    {
        visual ??= _mainWindowProvider.GetMainWindow();

        return TopLevel.GetTopLevel(visual)!;
    }
}