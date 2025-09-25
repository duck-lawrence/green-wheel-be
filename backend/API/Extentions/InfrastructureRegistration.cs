using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;


namespace API.Extentions
{
    public static class InfrastructureRegistration
    {
        public static void AddInfrastructue(this IServiceCollection services, string mssqlConnectionString)
        {
            services.AddDbContext<IAppDbContext, AppDbContext>(options =>
            {
                options.UseSqlServer(mssqlConnectionString);
            });
        }
    }
}
