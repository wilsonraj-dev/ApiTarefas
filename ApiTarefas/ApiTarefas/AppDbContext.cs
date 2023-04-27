using Microsoft.EntityFrameworkCore;

namespace ApiTarefas
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Tarefa> Tarefas => Set<Tarefa>();
    }
}
