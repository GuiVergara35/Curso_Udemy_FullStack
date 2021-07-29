using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PRoEventos.API.Data;
using PRoEventos.API.Models;

namespace PRoEventos.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EventoController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        public EventoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return _context.Eventos;
        }

        [HttpGet("{id}")]
        public Evento GetById(int id)
        {
            return _context.Eventos.FirstOrDefault(evento => evento.EventoId == id);
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
