using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Persistence.Repository
{
    public class LoteRepository : ILoteRepository
    {
        private readonly ApplicationDbContext _database;

        public LoteRepository(ApplicationDbContext database)
        {
            _database = database;

        }

        public async Task<Lote> GetLoteByIdsAsync(int eventoId, int loteId)
        {
            IQueryable<Lote> query = _database.Lotes;

            query = query.AsNoTracking()
                            .Where(lote => lote.EventoId == eventoId && lote.Id == loteId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Lote[]> GetLotesByEventoIdAsync(int eventoId)
        {
            IQueryable<Lote> query = _database.Lotes;

            query = query.AsNoTracking()
                            .Where(lote => lote.EventoId == eventoId);

            return await query.ToArrayAsync();
        }
    }
}
