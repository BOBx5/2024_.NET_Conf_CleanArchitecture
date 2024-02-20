using LibrarySolution.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibrarySolution.Infrastructure.DateTimeProvider;
public static class DependencyInjection
{
    public static IServiceCollection AddDateTimeService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // NTP Service Address
        // var ntpAddress = configuration["NtpService"]["DefaultConnection"]; // "https://www.someNtpAddress.org";
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        return services;
    }
}
