using Avalonia.Platform.Storage;

namespace Luminescence.Services;

public class AppFilePickerOpenOptions
{
    public string Title { get; set; }
    public FilePickerFileType[]? FileTypeFilter { get; set; }
    public string? StartLocationDirectory { get; set; }
}