using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Application.Services
{
    public class LoteService : ILoteService
    {
        private readonly IGeralRepository _geralRepository;
        private readonly ILoteRepository _loteRepository;
        private readonly IMapper _mapper;
        public LoteService(IGeralRepository geralRepository, ILoteRepository loteRepository, IMapper mapper)
        {
            _loteRepository = loteRepository;
            _geralRepository = geralRepository;
            _mapper = mapper;

        }

        public async Task AddLotes(int eventoId, LoteDto model)
        {
            try
            {
                var lote = _mapper.Map<Lote>(model);
                lote.EventoId = eventoId;

                _geralRepository.Add<Lote>(lote);

                await _geralRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes = await _loteRepository.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null)
                    return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddLotes(eventoId, model);
                    }
                    else
                    {
                        var lote = lotes.FirstOrDefault(lote => lote.Id == model.Id);
                        model.EventoId = eventoId;

                        _mapper.Map(model, lote);
                        _geralRepository.Update<Lote>(lote);

                        await _geralRepository.SaveChangesAsync();
                    }
                }

                var loteRetorno = await _loteRepository.GetLotesByEventoIdAsync(eventoId);
                return _mapper.Map<LoteDto[]>(loteRetorno);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                var lote = await _loteRepository.GetLoteByIdsAsync(eventoId, loteId);
                if (lote == null)
                    throw new Exception("Lote não encontrado.");

                _geralRepository.Delete<Lote>(lote);
                return await _geralRepository.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        public async Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                var result = await _loteRepository.GetLotesByEventoIdAsync(eventoId);
                if (result == null)
                    return null;

                return _mapper.Map<LoteDto[]>(result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            };
        }

        public async Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId)
        {
            try
            {
                var result = await _loteRepository.GetLoteByIdsAsync(eventoId, loteId);
                if (result == null)
                    return null;

                return _mapper.Map<LoteDto>(result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            };
        }

    }
}
