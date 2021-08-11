using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Interfaces
{
    public interface ILoteRepository
    {
        /// <summary>
        /// Método que retorna array de lotes de acordo com o Id do evento
        /// </summary>
        /// <param name="eventoId">Chave da tabela Eventos</param>
        /// <param name="loteId">Chave da tabela Lotes</param>
        /// <returns>Array de Lotes</returns>
        Task<Lote[]> GetLotesByEventoIdAsync(int eventoId);

        /// <summary>
        /// Método que retorna apenas um lote
        /// </summary>
        /// <param name="eventoId">Chave da tabela Eventos</param>
        /// <param name="loteId">Chave da tabela Lotes</param>
        /// <returns>Apenas um lote</returns>
        Task<Lote> GetLoteByIdsAsync(int eventoId, int loteId);

    }
}
