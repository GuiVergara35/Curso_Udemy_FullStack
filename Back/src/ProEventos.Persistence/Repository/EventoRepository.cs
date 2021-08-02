using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Persistence.Repository
{
    public class EventoRepository : IEventoRepository
    {
        private readonly ApplicationDbContext _database;

        public EventoRepository(ApplicationDbContext database)
        {
            _database = database;
            _database.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _database.Eventos
                .Include(e => e.Lote)
                .Include(e => e.RedesSociais);
            if (includePalestrantes)
                query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);

            query = query.OrderBy(e => e.Id);

            return await query.ToArrayAsync();

        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _database.Eventos
                .Include(e => e.Lote)
                .Include(e => e.RedesSociais);
            if (includePalestrantes)
                query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);

            query = query.OrderBy(e => e.Id)
                .Where(e => e.Tema.ToLower().Contains(tema.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventosByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _database.Eventos
                .Include(e => e.Lote)
                .Include(e => e.RedesSociais);
            if (includePalestrantes)
                query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);

            query = query.OrderBy(e => e.Id)
                .Where(e => e.Id == eventoId);

            return await query.FirstOrDefaultAsync();
        }
    }
}
