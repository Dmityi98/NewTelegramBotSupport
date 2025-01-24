using Bot.Core;
using Microsoft.EntityFrameworkCore;

namespace Bot.Database
{
    public class DbContextBot : DbContext
    {
        public DbContextBot(DbContextOptions<DbContextBot> options) : base(options) { }

        public DbSet<TeachersDb> TechersDb { get; set; }
    }
}
