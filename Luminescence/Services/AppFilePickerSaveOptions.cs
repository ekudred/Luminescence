using Avalonia.Platform.Storage;

namespace Luminescence.Services;

public class AppFilePickerSaveOptions
{
    public string Title { get; set; }
    public string? FileName { get; set; }
    public FilePickerFileType[]? FileTypeChoices { get; set; }
    public string? DefaultExtension { get; set; }
    public string? StartLocationDirectory { get; set; }
}