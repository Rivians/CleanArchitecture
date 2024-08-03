using CleanArchitecture.Domain.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanArchitecture.Persistance.Context
{
    public sealed class AppDbContext : DbContext
    {
        // override yöntemi yerine bu yöntemi kullanıyoruz. Artısı ise ConnectionString'i appsettings.json dan ayarlıyoruz
        public AppDbContext(DbContextOptions options) : base(options)  
        {
        }


        // burada sadece 1 metot ile : 100 tane configuration da eklesem otomatik olarak benim dbContextim bu konflarını dbContextime bağlayacak.
        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyReference).Assembly);

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<Entity>();
            foreach (var entry in entries)
            {
                if(entry.State == EntityState.Added)
                {
                    entry.Property(p => p.CreatedDate).CurrentValue = DateTime.UtcNow;
                }

                if(entry.State == EntityState.Modified)
                {
                    entry.Property(p => p.UpdatedDate).CurrentValue = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
