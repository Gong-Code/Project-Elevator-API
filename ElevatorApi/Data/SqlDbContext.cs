using Microsoft.EntityFrameworkCore;

namespace ElevatorApi.Data
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options)
        {
            
        }
    }
}
