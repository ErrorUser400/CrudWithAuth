using CrudWithAuth.Entitites;
using Microsoft.EntityFrameworkCore;

namespace CrudWithAuth.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> users { get; set; }
        public DbSet<ToDo> toDos { get; set; }
    }
}
