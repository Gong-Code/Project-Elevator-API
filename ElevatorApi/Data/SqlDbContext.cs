using ElevatorApi.Data.Entities;
using ElevatorApi.Services;
using Microsoft.EntityFrameworkCore;

namespace ElevatorApi.Data
{
    public class SqlDbContext : DbContext
    {
        private readonly IUserService _userService;

        public DbSet<ElevatorEntity> Elevators { get; set; } = null!;
        public DbSet<ErrandEntity> Errands { get; set; } = null!;
        public DbSet<CommentEntity> Comments { get; set; } = null!;

        public SqlDbContext(DbContextOptions<SqlDbContext> options, IUserService userService) : base(options)
        {
            _userService = userService;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            var (userId, userName) = await _userService.GetUserData();

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
                        entry.Entity.LastEditedById = userId;
                        break;
                    case EntityState.Added:
                        entry.Entity.CreatedDateUtc = DateTime.UtcNow;
                        entry.Entity.CreatedById = userId;
                        entry.Entity.CreatedByName = userName;
                        entry.Entity.LastEditedDateUtc = DateTime.UtcNow;
                        entry.Entity.LastEditedById = userId;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
