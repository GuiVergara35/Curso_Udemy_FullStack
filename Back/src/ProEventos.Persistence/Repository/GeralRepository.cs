using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Persistence.Repository
{
    public class GeralRepository : IGeralRepository
    {
        private readonly ApplicationDbContext _database;

        public GeralRepository(ApplicationDbContext database)
        {
            _database = database;
        }
        public void Add<T>(T entity) where T : class
        {
            _database.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _database.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _database.Remove(entity);
        }

        public void DeleteRange<T>(T[] entity) where T : class
        {
            _database.RemoveRange(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _database.SaveChangesAsync()) > 0;
        }
    }
}
