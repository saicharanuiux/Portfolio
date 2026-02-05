using Microsoft.EntityFrameworkCore;
using Portfolio.Entities;

namespace Portfolio.DataContext
{
    public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}
