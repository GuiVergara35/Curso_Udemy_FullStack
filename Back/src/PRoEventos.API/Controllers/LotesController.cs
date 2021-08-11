using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;

namespace PRoEventos.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LotesController : ControllerBase
    {
        private readonly ILoteService _service;

        public LotesController(ILoteService service)
        {
            _service = service;
        }

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {
            try
            {
                var lotes = await _service.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null)
                    return NoContent();


                return Ok(lotes);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar lotes. Erro: {ex.Message}");
            }
        }

        [HttpPut("{eventoId}")]
        public async Task<IActionResult> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes = await _service.SaveLotes(eventoId, models);
                if (lotes == null)
                    return NoContent();

                return Ok(lotes);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar lote. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{eventoId}/{loteId}")]
        public async Task<IActionResult> Delete(int eventoId, int loteId)
        {
            try
            {
                var lote = await _service.GetLoteByIdsAsync(eventoId, loteId);
                return await _service.DeleteLote(lote.EventoId, lote.Id) ? Ok(new { message = "Lote deletado." }) : BadRequest("Lote não deletado.");
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar lote. Erro: {ex.Message}");
            }
        }
    }
}
