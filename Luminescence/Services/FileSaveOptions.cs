using Avalonia.Platform.Storage;

namespace Luminescence.Services;

public class FileSaveOptions
{
    public string Title;
    public string? FileName;

    public FilePickerFileType[]? FileTypeChoices;

    public string? DefaultExtension;
    public string? StartLocationDirectory;
}