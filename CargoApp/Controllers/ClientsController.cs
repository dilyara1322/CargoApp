using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CargoApp.Models;
using Microsoft.Extensions.Logging;

namespace CargoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<Client> _logger;

        public ClientsController(ApplicationContext context, ILogger<Client> logger)
        {
            _context = context;
            _logger = logger; 
            
            if (!_context.Clients.Any<Client>())
            {
                _context.Clients.Add(new Client { RegData = new UserRegData { Login = "client1", Name = "123", Password = "123", Salt = "0" } });
                _context.Clients.Add(new Client { RegData = new UserRegData { Login = "client2", Name = "123123", Password = "123", Salt = "0" } });
                _context.SaveChanges();
            }
        }


        // GET - получение данных (Receive)

        // GET: api/Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> Get() //GetClients()
        {
            return await _context.Clients.ToListAsync();
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> Get(int id) //GetClients(int id)
        {
            var client = await _context.Clients.FindAsync(id); //FirstOrDefaultAsync(x => x.Id == id);

            if (client == null)
            {
                return NotFound();
            }
            return new ObjectResult(client);
            //return client;
        }

        // POST - добавление данных (Create)

        // POST: api/Clients
        [HttpPost]
        public async Task<ActionResult<Client>> Post([FromBody] Client client)
        {
            if (client == null)
            {
                return BadRequest();
            }

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            // return CreatedAtAction("GetClient", new { id = client.Id }, client);

            return Ok(client);
        }

        /* POST body (example):
         {
  "login":"ghf",
  "regData":{
    "login":"ghf",
    "password":"123",
    "salt":"1"
  }
}
             */


        // PUT - полное изменение данных (Update)

        // PUT api/clients/
        [HttpPut]
        public async Task<ActionResult<Client>> Put([FromBody] Client client)
        {
            if (client == null)
            {
                return BadRequest();
            }
            if (!_context.Clients.Any(x => x.Id == client.Id))
            {
                return NotFound();
            }

            _context.Update(client);
            await _context.SaveChangesAsync();
            return Ok(client);
        }

        // PUT api/clients/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Client>> Put([FromRoute ]int id, [FromBody] Client client)
        {
            if (client == null)
            {
                return BadRequest();
            }
            if (!_context.Clients.Any(x => x.Id == id))
            {
                return NotFound();
            }

            _context.Update(client);
            await _context.SaveChangesAsync();
            return Ok(client);
        }


        // PATCH - частичное изменение данных (Update)

        // PATCH api/clients/
        [HttpPatch]
        public async Task<ActionResult<Client>> Patch ([FromBody] Client client)
        {
            if (client == null || client.Id == 0)
            {
                return BadRequest();
            }
            if (!_context.Clients.Any(x => x.Id == client.Id))
            {
                return NotFound();
            }

            Client x = _context.Clients.FirstOrDefault<Client>(x => x.Id == client.Id);
            if (x != null)
            {
                x.Rating = client.Rating;
                _context.Entry(x).State = EntityState.Modified;
                //_context.SaveChanges();
            }
       
            await _context.SaveChangesAsync();
            return Ok(client);
        }

        // PATCH api/clients/5
        [HttpPatch("{id}")]
        public async Task<ActionResult<Client>> Patch([FromRoute] int id, [FromBody] Client client)
        {
            if (client == null)
            {
                return BadRequest();
            }
            if (!_context.Clients.Any(x => x.Id == id))
            {
                return NotFound();
            }

            Client x = _context.Clients.FirstOrDefault<Client>(x => x.Id == id);
            if (x != null)
            {
                x.Rating = client.Rating;
                _context.Entry(x).State = EntityState.Modified;
                //_context.SaveChanges();
            }

            await _context.SaveChangesAsync();
            return Ok(client);
        }

        // DELETE - удаление данных (Delete)

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Client>> Delete(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return Ok(client);
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
