using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Persistence.Repository
{
    public class PalestranteRepository : IPalestranteRepository
    {
        private readonly ApplicationDbContext _database;

        public PalestranteRepository(ApplicationDbContext database)
        {
            _database = database;
        }

        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _database.Palestrantes
                .Include(p => p.RedesSociais);

            if (includeEventos)
                query = query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento);

            query = query.OrderBy(p => p.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _database.Palestrantes
                .Include(p => p.RedesSociais);

            if (includeEventos)
                query = query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento);

            query = query.OrderBy(p => p.Id)
                    .Where(p => p.Nome.ToLower().Contains(nome.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetPalestranteByIdAsync(int palestranteId, bool includeEventos)
        {
            IQueryable<Palestrante> query = _database.Palestrantes
                 .Include(p => p.RedesSociais);

            if (includeEventos)
                query = query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento);

            query = query.OrderBy(p => p.Id)
                    .Where(p => p.Id == palestranteId);

            return await query.FirstOrDefaultAsync();
        }
    }
}
