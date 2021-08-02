using System;
using System.Threading.Tasks;
using ProEventos.Application.Interfaces;
using ProEventos.Domain;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Application.Services
{
    public class EventoService : IEventoService
    {
        private readonly IGeralRepository _geralRepository;
        private readonly IEventoRepository _eventoRepository;
        public EventoService(IGeralRepository geralRepository, IEventoRepository eventoRepository)
        {
            _eventoRepository = eventoRepository;
            _geralRepository = geralRepository;

        }
        public async Task<Evento> AddEvento(Evento model)
        {
            try
            {
                _geralRepository.Add<Evento>(model);
                if (await _geralRepository.SaveChangesAsync())
                    return await _eventoRepository.GetEventosByIdAsync(model.Id, false);

                return null;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> UpdateEvento(int eventoId, Evento model)
        {
            try
            {
                var evento = await _eventoRepository.GetEventosByIdAsync(eventoId, false);
                if (evento == null)
                    return null;

                model.Id = evento.Id;

                _geralRepository.Update(model);
                if (await _geralRepository.SaveChangesAsync())
                    return await _eventoRepository.GetEventosByIdAsync(model.Id, false);

                return null;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteEvento(int eventoId)
        {
            try
            {
                var evento = await _eventoRepository.GetEventosByIdAsync(eventoId, false);
                if (evento == null)
                    throw new Exception("Evento não encontrado.");

                _geralRepository.Delete<Evento>(evento);
                return await _geralRepository.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var result = await _eventoRepository.GetAllEventosAsync(false);
                if (result == null)
                    return null;

                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            };
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var result = await _eventoRepository.GetAllEventosByTemaAsync(tema, includePalestrantes);
                if (result == null)
                    return null;

                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            };
        }

        public async Task<Evento> GetEventosByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var result = await _eventoRepository.GetEventosByIdAsync(eventoId, includePalestrantes);
                if (result == null)
                    return null;

                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            };
        }

    }
}
