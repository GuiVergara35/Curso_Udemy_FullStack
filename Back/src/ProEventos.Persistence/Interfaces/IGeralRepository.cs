using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Interfaces
{
    public interface IGeralRepository
    {
        //CRUD padrão menos Selects(Get)
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void DeleteRange<T>(T[] entity) where T : class;

        Task<bool> SaveChangesAsync();

        //Eventos

        //Palestrantes

    }
}
