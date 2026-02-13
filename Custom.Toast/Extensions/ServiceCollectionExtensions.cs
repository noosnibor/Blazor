using Custom.Toast.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Custom.Toast.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomToast(this IServiceCollection services)
    {
        services.AddScoped<ToastService>();
        return services;
    }
}
