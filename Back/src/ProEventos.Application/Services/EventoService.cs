using System;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Application.Services
{
    public class EventoService : IEventoService
    {
        private readonly IGeralRepository _geralRepository;
        private readonly IEventoRepository _eventoRepository;
        private readonly IMapper _mapper;
        public EventoService(IGeralRepository geralRepository, IEventoRepository eventoRepository, IMapper mapper)
        {
            _eventoRepository = eventoRepository;
            _geralRepository = geralRepository;
            _mapper = mapper;

        }
        public async Task<EventoDto> AddEvento(EventoDto model)
        {
            try
            {
                var entity = _mapper.Map<Evento>(model);
                _geralRepository.Add<Evento>(entity);

                if (await _geralRepository.SaveChangesAsync())
                {
                    var retorno = await _eventoRepository.GetEventosByIdAsync(entity.Id, false);

                    return _mapper.Map<EventoDto>(retorno);
                }

                return null;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> UpdateEvento(int eventoId, EventoDto model)
        {
            try
            {
                var evento = await _eventoRepository.GetEventosByIdAsync(eventoId, false);
                if (evento == null)
                    return null;

                model.Id = evento.Id;
                var entity = _mapper.Map<Evento>(model);

                _geralRepository.Update<Evento>(entity);
                if (await _geralRepository.SaveChangesAsync())
                {
                    var retorno = await _eventoRepository.GetEventosByIdAsync(entity.Id, false);
                    return _mapper.Map<EventoDto>(retorno);
                }


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

        public async Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var result = await _eventoRepository.GetAllEventosAsync(false);
                if (result == null)
                    return null;

                return _mapper.Map<EventoDto[]>(result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            };
        }

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var result = await _eventoRepository.GetAllEventosByTemaAsync(tema, includePalestrantes);
                if (result == null)
                    return null;

                return _mapper.Map<EventoDto[]>(result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            };
        }

        public async Task<EventoDto> GetEventosByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var result = await _eventoRepository.GetEventosByIdAsync(eventoId, includePalestrantes);
                if (result == null)
                    return null;

                return _mapper.Map<EventoDto>(result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            };
        }

    }
}
