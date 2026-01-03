
using MasterDatabase.Models;
using MasterLib;
using MasterLib.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MasterDatabase
{
    public class MasterDbContext : DbContext
    {
        private UserContext _userContext;

        public MasterDbContext(DbContextOptions<MasterDbContext> options, UserContext userContext)
            : base(options)
        {
            _userContext = userContext;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Unit> Units { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserTokens)
                .WithOne(ut => ut.User)
                .HasForeignKey(ut => ut.UserId)
                .HasPrincipalKey(u => u.UserId);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseModel).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var propertyMethod = Expression.Property(parameter, nameof(BaseModel.DeletedOn));
                    var compare = Expression.Equal(propertyMethod, Expression.Constant(null, propertyMethod.Type));
                    var lambda = Expression.Lambda(compare, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }

        public override int SaveChanges()
        {
            ApplyAuditInfo();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInfo();
            return base.SaveChangesAsync(cancellationToken);
        }

        public int SaveChangesWithoutAudit()
        {
            return base.SaveChanges();
        }

        public Task<int> SaveChangesWithoutAuditAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditInfo()
        {
            try
            {
                var now = DateTime.UtcNow;
                var userId = _userContext.UserId;

                foreach (var entry in ChangeTracker.Entries<BaseModel>())
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.CreatedOn = now;
                            entry.Entity.CreatedBy = userId;
                            break;

                        case EntityState.Modified:
                            entry.Entity.LastModifiedOn = now;
                            entry.Entity.LastModifiedBy = userId;
                            break;

                        case EntityState.Deleted:
                            // Soft delete
                            entry.State = EntityState.Modified;
                            entry.Entity.DeletedOn = now;
                            entry.Entity.DeletedBy = userId;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Warning("Fail to log", ex);
            }
        }
    }

}
