using CloudStorage.Domain.Interfaces;
using CloudStorage.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace CloudStorage.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgreSql");

        services.AddMinio(options => options
            .WithEndpoint(configuration["MinIO:Endpoint"])
            .WithCredentials(configuration["MinIO:AccessKey"], configuration["MinIO:SecretKey"])
            .WithSSL(false)
            .Build());

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IFolderRepository, FolderRepository>();
        services.AddScoped<IStorageRepository, StorageRepository>();

        return services;
    }
}