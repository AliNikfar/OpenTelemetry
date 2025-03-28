using Microsoft.EntityFrameworkCore;

namespace FirstApplication.Models
{
    public class PersonDbContext :DbContext
    {
        public DbSet<Person> Persons { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=PersonDB;Integrated Security=True;TrustServerCertificate=True;");
        }
    }
}
