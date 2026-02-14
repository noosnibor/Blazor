namespace PortalWeb.Services;

public static class FormValidation
{
    public static string Required<T>(bool isRequired, T? value)
    {
        if(IsEmpty(value) && isRequired)
        return "bg-warning-subtle border-warning";

        return "bg-success-subtle border-success";
    }

    private static bool IsEmpty<T>(T? value)
    {
        if (value == null)
            return true;

        var type = typeof(T);

        // String
        if (type == typeof(string))
            return string.IsNullOrWhiteSpace(value as string);

        // Nullable DateTime
        if (type == typeof(DateTime?) || type == typeof(DateTime))
            return EqualityComparer<T>.Default.Equals(value, default!);

        // Numeric types
        if (type == typeof(int) ||
            type == typeof(int?) ||
            type == typeof(decimal) ||
            type == typeof(decimal?) ||
            type == typeof(double) ||
            type == typeof(double?) ||
            type == typeof(float) ||
            type == typeof(float?))
        {
            return EqualityComparer<T>.Default.Equals(value, default!);
        }

        return false;
    }

}
