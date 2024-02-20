using LibrarySolution.Application.Interfaces;
using LibrarySolution.Domain.Aggregates.Books.Repositories;
using LibrarySolution.Domain.Aggregates.Rents.Repositories;
using LibrarySolution.Domain.Aggregates.Users.Repositories;
using LibrarySolution.Infrastructure.Persistence.CachedRepositories;
using LibrarySolution.Infrastructure.Persistence.Interceptors;
using LibrarySolution.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibrarySolution.Infrastructure.Persistence;
public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            
            // 아웃박스 인터셉터 추가
            options.AddDomainEventsToOutboxMessageInterceptor(serviceProvider, throwExceptionWhenNotRegistered: false);
        });

        // 두 스코프 모두 같은 인스턴스(ApplicationDbContext)를 반환하지만 용도에 따라 호출할 수 있는 메서드를 다르게 갖는다.
        // IApplicationDbContext: 선언된 Repository Interface만 노출합니다.
        // IUnitOfWork          : SaveChangesAsync 메소드만 노출합니다.
        services.AddScoped<IApplicationDbContext>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());

        // Repository
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRentRepository, RentRepository>();

        // Cached Repository
        //services.AddMemoryCache();
        //services.AddScoped<IBookRepository, CachedBookRepository>();
        //services.AddScoped<IUserRepository, CachedUserRepository>();
        //services.AddScoped<IRentRepository, CachedRentRepository>();
        return services;
    }
}
