using ApiTareasInteligente.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiTareasInteligente.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Tarea> Tareas { get; set; }
    }
}