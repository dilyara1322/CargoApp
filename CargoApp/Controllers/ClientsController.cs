using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CargoApp.Models;
using Newtonsoft.Json.Linq;
using CargoApp.Tools;
using Microsoft.Data.SqlClient;

namespace CargoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ApplicationContext _context;
       // private readonly ILogger<Client> _logger;

        public ClientsController(ApplicationContext context) //, ILogger<Client> logger)
        {
            _context = context;
            //_logger = logger;
        }


        // GET - получение данных 

        // GET: api/Clients?limit=30&offset=5
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients(string orderby = "id", int limit = 10, int offset = 0, string filters = null)
        {
            if (filters == null && orderby.ToLower() == "id")
            {
                return await _context.Clients
                    .Include(c => c.RegData)
                    .Include(c => c.Passport)
                    .Skip(offset)
                    .Take(limit)
                    .AsNoTracking()
                    //.DefaultIfEmpty()
                    .ToListAsync();
            }
            else
            {
                string where = "";
                if (filters != null)
                {
                    Filters filtersObj = new Filters(filters);
                    if (filtersObj.Message != null)
                        return BadRequest(filtersObj.Message);

                    where = filtersObj.GetWhere("Clients");
                }
                string sortby = "";
                switch (orderby.ToLower())
                {
                    case "id": break;
                    case "login": sortby = " ORDER BY Clients.login "; break;
                    case "rating": sortby = " ORDER BY Clients.rating "; break;
                    default: return BadRequest();
                }

                List<Client> clients = null;
                try
                {
                    string sql = $"SELECT * FROM Clients {where} {sortby}";
                    if (sortby == "")
                        clients = await _context.Clients
                            .FromSqlRaw(sql)
                            .Include(c => c.RegData)
                            .Include(c => c.Passport)
                            .Skip(offset)
                            .Take(limit)
                            .AsNoTracking()
                            //.DefaultIfEmpty()
                            .ToListAsync();
                    else
                        clients = await _context.Clients
                            .FromSqlRaw(sql)
                            .AsNoTracking()
                            //.DefaultIfEmpty()
                            .ToListAsync();
                    return clients;
                }
                catch (SqlException e)
                {
                    return BadRequest("Неверный JSON");
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _context.Clients
                .Include(c => c.RegData)
                .Include(c => c.Passport)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (client == null)
                return NotFound();

            return client;
        }

        // GET: api/Clients/5/marks
        [HttpGet("{id}/{detail}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetClientDetails(int id, string detail, int limit = 10, int offset = 0)
        {
            if (ClientExists(id))
            {
                if (detail == "requests")
                    return await _context.Requests
                        .Include(r => r.SendingAddress)
                        .Include(r => r.ReceivingAddress)
                        .Where(r => r.ClientId == id)
                        .Skip(offset)
                        .Take(limit)
                        .AsNoTracking()
                        //.DefaultIfEmpty()
                        .ToListAsync();
                
                if (detail == "marks")
                    return await _context.Ratings
                        .Where(r => r.ClientId == id)
                        .Skip(offset)
                        .Take(limit)
                        .AsNoTracking()
                        //.DefaultIfEmpty()
                        .ToListAsync();
                
                if (detail == "messages")
                    return await _context.UserMessages
                        .Where(r => r.ClientId == id)
                        .Skip(offset)
                        .Take(limit)
                        .AsNoTracking()
                        //.DefaultIfEmpty()
                        .ToListAsync();

                return BadRequest();
            }
                
            return NotFound();
        }

        // POST - добавление данных 

        // POST: api/Clients
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient([FromBody] Client client)
        {
            if (client == null)
            {
                return BadRequest();
            }

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = client.Id }, client);

            //return Ok(client);
        }

        // POST: api/Clients/5/Passport
        [HttpPost("{id}/{detail}")]
        public async Task<ActionResult<Object>> PostClientDetail(int id, string detail, [FromBody] JObject obj)
        {
            if (obj == null)
                return BadRequest();

            if (detail.ToLower() == "passport")
            {
                var client = await _context.Clients
                    .Include(c => c.Passport)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (client == null)
                    return NotFound();

                Passport passport = null;
                try { passport = obj.ToObject<Passport>(); } 
                catch { return BadRequest(); }

                if (client.Passport != null || passport == null)
                    return BadRequest();

                passport.ClientId = id;

                _context.Passports.Add(passport);
                await _context.SaveChangesAsync();

                return Ok(await _context.Passports.FirstOrDefaultAsync(p => p.ClientId == id));
            }

            if (detail.ToLower() == "mark")
            {
                if (!ClientExists(id))
                    return NotFound();

                Rating mark = null;
                try { mark = obj.ToObject<Rating>(); }
                catch { return BadRequest(); }

                if (mark.CompanyId <= 0 || mark.MarkFromUserToCompany == null)
                    return BadRequest();

                if (await _context.Ratings
                        .Where(r => r.CompanyId == mark.CompanyId)
                        .FirstOrDefaultAsync(r => r.ClientId == id)
                            != null)
                    return BadRequest();

                mark.ClientId = id;
                mark.MarkFromCompanyToUser = null;

                _context.Ratings.Add(mark);
                await _context.SaveChangesAsync();

                return Ok(await _context.Ratings
                    .Where(r => r.CompanyId == mark.CompanyId)
                    .FirstOrDefaultAsync(r => r.ClientId == id));
            }

            return NotFound();

        }

        /*Content-Type: application/json; charset=utf-8 */
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


        // PUT - полное изменение данных 

        /* put without id
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
        */

        /* put
        // PUT api/clients/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Client>> PutClient([FromRoute ]int id, [FromBody] Client client)
        {
            if (client == null || id != client.Id)
            {
                return BadRequest();
            }
            if (!ClientExists(id))
            {
                return NotFound();
            }

            _context.Update(client);
            await _context.SaveChangesAsync();
            return Ok(client);
        }
        */

        // PATCH - частичное изменение данных 

        /* patch without id
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
                        
                        //using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
                        {
                        //    string saltedPass = client.RegData.Password + client.RegData.Salt;
                          //  byte[] bytes = System.Text.Encoding.GetBytes(saltedPass);
                            //x.RegData.Password = 
                              //  sha256.ComputeHash(bytes);
                        }
                        
                        x.RegData.Password = client.RegData.Password;
                    }

                }
                _context.Entry(x).State = EntityState.Modified;
                //_context.SaveChanges();
            }
       
            await _context.SaveChangesAsync();
            return Ok(client);
        }
        */

        // PATCH api/clients/5
        [HttpPatch("{id}")]
        public async Task<ActionResult<Client>> PatchClient(int id, [FromBody] Client client)
        {
            if (client == null)
                return BadRequest();

            Client x = await _context.Clients
                .Include(c => c.RegData)
                .Include(c => c.Passport)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (x == null) 
                return NotFound();

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

            if (client.Passport != null)
            {
                if (x.Passport == null)
                    x.Passport = new Passport();

                if (client.Passport.Series != null)
                    x.Passport.Series = client.Passport.Series;
                if (client.Passport.Number != null)
                    x.Passport.Number = client.Passport.Number;
                if (client.Passport.Surname != null)
                    x.Passport.Surname = client.Passport.Surname;
                if (client.Passport.FirstName != null)
                    x.Passport.FirstName = client.Passport.FirstName;
                if (client.Passport.Patronymic != null)
                    x.Passport.Patronymic = client.Passport.Patronymic;
                if (client.Passport.Sex != null)
                    x.Passport.Sex = client.Passport.Sex;
                if (client.Passport.BirthDate != null)
                    x.Passport.BirthDate = client.Passport.BirthDate;

                if (client.Passport.BirthPlace != null)
                    x.Passport.BirthPlace = client.Passport.BirthPlace;

                /*
                    if (client.Passport.BirthPlace != null)
                    {
                    if (x.Passport.BirthPlace == null)
                        x.Passport.BirthPlace = new Address();

                    if (client.Passport.BirthPlace.Index != null)
                        x.Passport.BirthPlace.Index = client.Passport.BirthPlace.Index;
                    if (client.Passport.BirthPlace.Country != null)
                        x.Passport.BirthPlace.Country = client.Passport.BirthPlace.Country;
                    if (client.Passport.BirthPlace.Region != null)
                        x.Passport.BirthPlace.Region = client.Passport.BirthPlace.Region;
                    if (client.Passport.BirthPlace.City != null)
                        x.Passport.BirthPlace.City = client.Passport.BirthPlace.City;
                    if (client.Passport.BirthPlace.Street != null)
                        x.Passport.BirthPlace.Street = client.Passport.BirthPlace.Street;
                    if (client.Passport.BirthPlace.House != null)
                        x.Passport.BirthPlace.House = client.Passport.BirthPlace.House;
                    if (client.Passport.BirthPlace.Flat != null)
                        x.Passport.BirthPlace.Flat = client.Passport.BirthPlace.Flat;
                    if (client.Passport.BirthPlace.Addition != null)
                        x.Passport.BirthPlace.Addition = client.Passport.BirthPlace.Addition;
                    }
                */

                if (client.Passport.IssuedBy != null)
                    x.Passport.IssuedBy = client.Passport.IssuedBy;
                if (client.Passport.IssuedDate != null)
                    x.Passport.IssuedDate = client.Passport.IssuedDate;
                if (client.Passport.Code != null)
                    x.Passport.Code = client.Passport.Code;
            }

            _context.Entry(x).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return Ok(await _context.Clients
                .Include(c => c.RegData)
                .Include(c => c.Passport)
                .FirstOrDefaultAsync(x => x.Id == id));
        }

        // PATCH api/clients/5/passport
        [HttpPatch("{id}/{detail}")]
        public async Task<ActionResult<Client>> PatchClientDetail(int id, string detail, [FromBody] JObject obj)
        {
            if (!ClientExists(id))
                return NotFound();
            if (obj == null)
                return BadRequest();

            if (detail.ToLower() == "passport")
            {
                Passport passport = null;
                try { passport = obj.ToObject<Passport>(); }
                catch { return BadRequest(); }

                Passport x = await _context.Passports
                    .FirstOrDefaultAsync(x => x.ClientId == id);

                if (x == null)
                    return NotFound();

                if (passport.Series != null)
                    x.Series = passport.Series;
                if (passport.Number != null)
                    x.Number = passport.Number;
                if (passport.Surname != null)
                    x.Surname = passport.Surname;
                if (passport.FirstName != null)
                    x.FirstName = passport.FirstName;
                if (passport.Patronymic != null)
                    x.Patronymic = passport.Patronymic;
                if (passport.Sex != null)
                    x.Sex = passport.Sex;
                if (passport.BirthDate != null)
                    x.BirthDate = passport.BirthDate;
                if (passport.BirthPlace != null)
                    x.BirthPlace = passport.BirthPlace;
                if (passport.IssuedBy != null)
                    x.IssuedBy = passport.IssuedBy;
                if (passport.IssuedDate != null)
                    x.IssuedDate = passport.IssuedDate;
                if (passport.Code != null)
                    x.Code = passport.Code;

                _context.Entry(x).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return Ok(await _context.Passports
                    .FirstOrDefaultAsync(x => x.ClientId == id));
            }

            /*
            if (detail.ToLower() == "mark")
            {
                Rating mark = null;
                try { mark = obj.ToObject<Rating>(); }
                catch { return BadRequest(); }

                if (mark.CompanyId <= 0 || mark.MarkFromUserToCompany == null)
                    return BadRequest();

                Rating x = await _context.Ratings
                        .Where(r => r.CompanyId == mark.CompanyId)
                        .FirstOrDefaultAsync(r => r.ClientId == id);

                if (x == null)
                    return NotFound();

                if (mark.MarkFromUserToCompany != null)
                    x.MarkFromUserToCompany = mark.MarkFromUserToCompany;
                else 
                    return BadRequest();

                _context.Entry(x).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(await _context.Ratings
                    .Where(r => r.CompanyId == mark.CompanyId)
                    .FirstOrDefaultAsync(r => r.ClientId == id));
            }
            */

            return NotFound();
        }

        // PATCH api/clients/5/passport
        [HttpPatch("{id}/{detail}/{detailId}")]
        public async Task<ActionResult<Client>> PatchClientDetailWithId(int id, string detail, int detailId, [FromBody] JObject obj)
        {
            if (!ClientExists(id))
                return NotFound();
            if (obj == null)
                return BadRequest();

            if (detail.ToLower() == "mark")
            {
                Rating mark = null;
                try { mark = obj.ToObject<Rating>(); }
                catch { return BadRequest(); }

                if (detailId <= 0 || mark.MarkFromUserToCompany == null)
                    return BadRequest();

                Rating x = await _context.Ratings
                        .Where(r => r.CompanyId == detailId)
                        .FirstOrDefaultAsync(r => r.ClientId == id);

                if (x == null)
                    return NotFound();

               // if (mark.MarkFromUserToCompany != null)

                x.MarkFromUserToCompany = mark.MarkFromUserToCompany;

               // else return BadRequest();

                _context.Entry(x).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(await _context.Ratings
                    .Where(r => r.CompanyId == detailId)
                    .FirstOrDefaultAsync(r => r.ClientId == id));
            }

            return NotFound();
        }

        // DELETE - удаление данных 

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Client>> DeleteClient(int id)
        {
            var client = await _context.Clients
                .Include(c => c.RegData)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (client == null)
                return NotFound();

            _context.UserRegData.Remove(client.RegData);

            //var client = await _context.Clients.FindAsync(id);
            //if (client == null) return NotFound();
            //_context.Clients.Remove(client);

            await _context.SaveChangesAsync();

            return Ok(client);
        }

        // DELETE: api/Clients/5/passport
        [HttpDelete("{id}/{detail}")]
        public async Task<ActionResult<object>> DeleteClientDetail(int id, string detail)
        {
            if (!ClientExists(id))
                return NotFound();

            if (detail.ToLower() == "passport")
            {
                Passport passport = await _context.Passports
                    .FirstOrDefaultAsync(p => p.ClientId == id);

                if (passport == null)
                    return NotFound();

                _context.Passports.Remove(passport);
                await _context.SaveChangesAsync();

                return passport;
            }

            return NotFound();
        }

        // DELETE: api/Clients/5/mark/6       (6 - companyId)
        [HttpDelete("{id}/{detail}/{detailId}")]
        public async Task<ActionResult<object>> DeleteClientDetailWithId(int id, string detail, int detailId)
        {
            if (!ClientExists(id))
                return NotFound();

            if (detail.ToLower() == "mark")
            {
                Rating mark = await _context.Ratings
                    .Where(m => m.CompanyId == detailId)
                    .FirstOrDefaultAsync(m => m.ClientId == id);

                if (mark == null)
                    return NotFound();

                mark.MarkFromUserToCompany = null;

                //_context.Ratings.Remove(mark);

                _context.Entry(mark).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(mark);
            }

            return NotFound();
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
