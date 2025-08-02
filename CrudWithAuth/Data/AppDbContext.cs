using CrudWithAuth.Model;
using Microsoft.EntityFrameworkCore;

namespace CrudWithAuth.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<ToDo> toDos { get; set; }
        public DbSet<UserModel> userModels { get; set; }

    }
}
