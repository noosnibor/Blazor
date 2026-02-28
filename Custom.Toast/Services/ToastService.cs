using Custom.Toast.Models;

namespace Custom.Toast.Services;

public interface IToastService
{
    event Action? OnClear;
    event Action<ToastMessage>? OnShow;

    void Clear();
    void Show(ToastType type, string message);
    void ShowError(string message);
    void ShowInfo(string message);
    void ShowSuccess(string message);
    void ShowWarning(string message);
}

public class ToastService : IToastService
{
    public event Action<ToastMessage>? OnShow;
    public event Action? OnClear;

    public void ShowSuccess(string message)
        => Show(ToastType.Success, message);

    public void ShowWarning(string message)
        => Show(ToastType.Warning, message);

    public void ShowError(string message)
        => Show(ToastType.Danger, message);

    public void ShowInfo(string message)
        => Show(ToastType.Primary, message);

    public void Show(ToastType type, string message)
    {
        OnShow?.Invoke(new ToastMessage
        {
            Type = type,
            Message = message
        });
    }

    public void Clear() => OnClear?.Invoke();
}
