using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PRoEventos.API.Models;

namespace PRoEventos.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EventoController : ControllerBase
    {
        public IEnumerable<Evento> _eventos = new Evento[] {
                new Evento(){
                EventoId = 1,
                Tema = "Angular e .Net",
                Local = "SP",
                Lote = "Lote 1",
                QtdPessoas = 250,
                DataEvento = DateTime.Now.AddDays(2).ToString(),
                ImagemUrl = "aracna.png"
                },
                new Evento(){
                EventoId = 2,
                Tema = "Angular e .Net",
                Local = "SP",
                Lote = "Lote 1",
                QtdPessoas = 250,
                DataEvento = DateTime.Now.AddDays(2).ToString(),
                ImagemUrl = "aracna.png"
                }
            };

        public EventoController()
        {

        }

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return _eventos;
        }

        [HttpGet("{id}")]
        public IEnumerable<Evento> GetById(int id)
        {
            return _eventos.Where(evento => evento.EventoId == id);
        }

        [HttpPost]
        public string Post()
        {
            return "PostDoido";
        }

        [HttpPut("{id}")]
        public string Put(int id)
        {
            return $"PutDoido com id = {id}";
        }

        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            return $"DeleteDoido com id = {id}";
        }
    }
}
