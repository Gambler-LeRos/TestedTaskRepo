using Microsoft.EntityFrameworkCore;

namespace TestedTask
{
    public class DataBaseContext: DbContext
    {
        public DbSet<Request> Requests { get; set; }
        public DataBaseContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Request;Trusted_Connection=true;");
        }

    }
}
