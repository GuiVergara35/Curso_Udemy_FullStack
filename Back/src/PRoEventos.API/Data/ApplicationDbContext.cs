using Microsoft.EntityFrameworkCore;
using PRoEventos.API.Models;

namespace PRoEventos.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Evento> Eventos { get; set; }

    }
}
