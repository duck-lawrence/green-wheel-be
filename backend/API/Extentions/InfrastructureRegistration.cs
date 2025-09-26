using Infrastructure.ApplicationDbContext;
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
    }
}
