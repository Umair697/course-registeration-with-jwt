using CourseRegistration.Application.Abstractions;
using CourseRegistration.Application.Services;
using CourseRegistration.Infrastructure.Persistence;
using CourseRegistration.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CourseRegistration.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<CourseRegistrationService>();

        services.AddScoped<ICourseRegistrationService, TransactionalCourseRegistrationService>();

        return services;
    }
}