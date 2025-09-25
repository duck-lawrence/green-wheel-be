using Domain.Commons;





//using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.RegularExpressions;

namespace Infrastructure.ApplicationDbContext;

public class AppDbContext : DbContext, IAppDbContext
{

    //public DbSet<User> Users { get; set; }
    //public DbSet<RefreshToken> RefreshTokens { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }
    public DbSet<T> Set<T>() where T : class,IEntity => base.Set<T>();

    public EntityEntry<T> Entry<T>(T entity) where T : class, IEntity
    {
        return base.Entry(entity);
    }
    //cấu hình map dữ liệu với trường snake_case trong db
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Đổi tên bảng
            entity.SetTableName(ToSnakeCase(entity.GetTableName()));

            // Đổi tên cột
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(ToSnakeCase(property.Name));
            }
        }
    }
    private static string ToSnakeCase(string name)
    {
        if (string.IsNullOrEmpty(name)) return name;

        var result = Regex.Replace(
            name,
            @"([a-z0-9])([A-Z])",
            "$1_$2"
        );

        return result.ToLower();
    }
}

