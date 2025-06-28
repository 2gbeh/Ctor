using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Ctor.Entities;
using Ctor.Models;

namespace Ctor.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    private static void SetSoftDeleteFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : Model
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(e => e.DeletedAt == null);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply global filter to all entities inheriting Base
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(Model).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(AppDbContext)
                    .GetMethod(nameof(SetSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)?
                    .MakeGenericMethod(entityType.ClrType);

                method?.Invoke(null, new object[] { modelBuilder });
            }
        }
    }
    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<Model>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChanges();
    }
}
