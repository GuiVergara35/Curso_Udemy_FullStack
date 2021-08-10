using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using ProEventos.Application.Dtos;

namespace PRoEventos.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly IEventoService _service;

        public EventosController(IEventoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await _service.GetAllEventosAsync(true);
                if (eventos == null)
                    return NoContent();


                return Ok(eventos);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var evento = await _service.GetEventosByIdAsync(id, true);
                if (evento == null)
                    return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar evento. Erro: {ex.Message}");
            }
        }

        [HttpGet("/tema/{tema}")]
        public async Task<IActionResult> GetByTema(string tema)
        {
            try
            {
                var eventos = await _service.GetAllEventosByTemaAsync(tema, true);
                if (eventos == null)
                    return NoContent();

                return Ok(eventos);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = await _service.AddEvento(model);
                if (evento == null)
                    return BadRequest("Erro ao tentar adicionar eventos.");

                return Ok(evento);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar criar evento. Erro: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDto model)
        {
            try
            {
                var evento = await _service.UpdateEvento(id, model);
                if (evento == null)
                    return BadRequest("Erro ao tentar atualizar evento.");

                return Ok(evento);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar evento. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                return await _service.DeleteEvento(id) ? Ok(new { message = "Evento deletado." }) : BadRequest("Evento não deletado.");
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar evento. Erro: {ex.Message}");
            }
        }
    }
}
