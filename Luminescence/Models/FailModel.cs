namespace Luminescence.Models;

public class FailModel
{
    public string Message { get; }

    public FailModel(string? message)
    {
        Message = message ?? "Произошла ошибка";
    }
}