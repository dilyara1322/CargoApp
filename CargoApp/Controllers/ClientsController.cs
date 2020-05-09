using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CargoApp.Models;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

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
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _context.Clients
                .Include(c => c.RegData)
                .Include(c => c.Passport)
                .DefaultIfEmpty()
                .ToListAsync();
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _context.Clients
                .Include(c => c.RegData)
                .Include(c => c.Passport)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (client == null)
            {
                return NotFound();
            }
            return new ObjectResult(client);
            //return client;
        }
        /*
        // GET: api/Clients/5/requests
        [HttpGet("{id}/{requests}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetClientRequests(int id, Request requests)
        {
            // _context.Ratings.Add(new Rating { ClientId = 1, CompanyId = 5, MarkFromCompanyToUser = 4, MarkFromUserToConpany = 5 });
            // _context.SaveChanges();

            if (ClientExists(id))
            return await _context.Requests
                .Where(r => r.ClientId == id)
                .Include(r => r.CurrentStatus)
                .Include(r => r.Goods)
                .Include(r => r.Messages)
                .Include(r => r.SendingAddress)
                .Include(r => r.ReceivingAddress)
                .DefaultIfEmpty()
                .ToListAsync();

            else
                return NotFound();
        }

        // GET: api/Clients/5/marks
        [HttpGet("{id}/{marks}")]
        public async Task<ActionResult<IEnumerable<Rating>>> GetClientMarks(int id, Rating rating)
        {
            // _context.Ratings.Add(new Rating { ClientId = 1, CompanyId = 5, MarkFromCompanyToUser = 4, MarkFromUserToConpany = 5 });
            // _context.SaveChanges();
            if (ClientExists(id))
                return await _context.Ratings
                    .Where(r => r.ClientId == id)
                    .DefaultIfEmpty()
                    .ToListAsync();

            else
                return NotFound();
        }*/

        // GET: api/Clients/5/marks
        [HttpGet("{id}/{detail}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetClientDetails(int id, string detail)
        {
            if (ClientExists(id))
            {
                if (detail == "requests")
                    return await _context.Requests
                        .Where(r => r.ClientId == id)
                        .DefaultIfEmpty()
                        .ToListAsync();
                if (detail == "marks")
                    return await _context.Ratings
                        .Where(r => r.ClientId == id)
                        .DefaultIfEmpty()
                        .ToListAsync();
                if (detail == "messages")
                    return await _context.UserMessages
                        .Where(r => r.ClientId == id)
                        .DefaultIfEmpty()
                        .ToListAsync();

                return BadRequest();
            }
                
            return NotFound();
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
                if (client.Email != null)
                    x.Email = client.Email;
                if (client.PhoneNumber != null)
                    x.PhoneNumber = client.PhoneNumber;
                if (client.Rating != null)
                    x.Rating = client.Rating;
                if (client.RegData != null)
                {
                    if (client.RegData.Name != null)
                        x.RegData.Name = client.RegData.Name;
                    if (client.RegData.Password != null)
                    {
                        /*
                        using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
                        {
                            string saltedPass = client.RegData.Password + client.RegData.Salt;
                            byte[] bytes = System.Text.Encoding.GetBytes(saltedPass);
                            x.RegData.Password = 
                                sha256.ComputeHash(bytes);
                        }
                        */
                        x.RegData.Password = client.RegData.Password;
                    }

                }
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
                if (client.Email != null)
                    x.Email = client.Email;
                if (client.PhoneNumber != null)
                    x.PhoneNumber = client.PhoneNumber;
                if (client.Rating != null)
                    x.Rating = client.Rating;
                if (client.RegData != null)
                {
                    if (client.RegData.Name != null)
                        x.RegData.Name = client.RegData.Name;
                    if (client.RegData.Password != null)
                    {
                        /*
                        using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
                        {
                            string saltedPass = client.RegData.Password + client.RegData.Salt;
                            byte[] bytes = System.Text.Encoding.GetBytes(saltedPass);
                            x.RegData.Password = 
                                sha256.ComputeHash(bytes);
                        }
                        */
                        x.RegData.Password = client.RegData.Password;
                    }
                         
                }

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
