using Domain.Commons;


//using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
namespace Infrastructure.ApplicationDbContext;

public interface IAppDbContext
{
    //public DbSet<User> Users { get; set; }
    //public DbSet<RefreshToken> RefreshTokens { get; set; }


    public DbSet<T> Set<T>() where T : class, IEntity;

    public  Task<int> SaveChangesAsync();
    public EntityEntry<T> Entry<T>(T entity) where T : class, IEntity;
}
