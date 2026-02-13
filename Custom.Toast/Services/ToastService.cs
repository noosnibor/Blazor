using Custom.Toast.Models;

namespace Custom.Toast.Services;

public class ToastService
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

    private void Show(ToastType type, string message)
    {
        OnShow?.Invoke(new ToastMessage
        {
            Type = type,
            Message = message
        });
    }

    public void Clear() => OnClear?.Invoke();
}
