using ElevatorApi.Data.Entities;
using ElevatorApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ElevatorApi.Data
{
    public class SqlDbContext : DbContext
    {
        private readonly IUserService _userService;

        public DbSet<ElevatorEntity> Elevators { get; set; } = null!;
        public DbSet<ErrandEntity> Errands { get; set; } = null!;

        public SqlDbContext(DbContextOptions<SqlDbContext> options, IUserService userService) : base(options)
        {
            _userService = userService;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            var id = _userService.GetCurrentUserId() ?? throw new ArgumentNullException();
            var name = _userService.GetCurrentUserName();

            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    case EntityState.Deleted:
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastEditedDateUtc = DateTime.UtcNow;
                        entry.Entity.LastEditedById = id;
                        break;
                    case EntityState.Added:
                        entry.Entity.CreatedDateUtc = DateTime.UtcNow;
                        entry.Entity.CreatedById = id;
                        entry.Entity.CreatedByName = name;
                        entry.Entity.LastEditedDateUtc = DateTime.UtcNow;
                        entry.Entity.LastEditedById = id;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
