using Avalonia.Platform.Storage;

namespace Luminescence.Services;

public interface IFilePickerSaveOptions
{
    public string Title { get; set; }
    public string? FileName { get; set; }
    public FilePickerFileType[]? FileTypeChoices { get; set; }
    public string? DefaultExtension { get; set; }
    public string? StartLocationDirectory { get; set; }
}