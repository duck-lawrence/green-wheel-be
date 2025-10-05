using Application;
using Application.Abstractions;
using Application.AppSettingConfigurations;
using Application.Repositories;
using Application.Services;
using Infrastructure.ApplicationDbContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace API.Extentions
{
    public static class InfrastructureRegistration
    {
        public static void AddInfrastructue(this IServiceCollection services, string mssqlConnectionString)
        {
            services.AddDbContext<IGreenWheelDbContext, GreenWheelDbContext>(options =>
            {
                options.UseSqlServer(mssqlConnectionString);
            });
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));

            // Đăng ký Cloudinary repository và service mới
            serviceCollection.AddScoped<ICloudinaryRepository, CloudinaryRepository>();
            serviceCollection.AddScoped<IPhotoService, CloudinaryService>();

            return serviceCollection;
        }
    }
}