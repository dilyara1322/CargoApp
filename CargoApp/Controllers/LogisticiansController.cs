using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CargoApp.Models;
using CargoApp.Tools;
using Microsoft.Data.SqlClient;

namespace CargoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogisticiansController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public LogisticiansController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Logisticians
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Logistician>>> GetLogisticians(string orderby = "id", int limit = 10, int offset = 0, string filters = null)
        {
            if (filters == null && orderby.ToLower() == "id")
            {
                return await _context.Logisticians
                    .Include(l => l.Company)
                    .Include(l => l.RegData)
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

                    where = filtersObj.GetWhere("Logisticians");
                }
                string sortby = "";
                switch (orderby.ToLower())
                {
                    case "id": break;
                    case "login": sortby = " ORDER BY Logisticians.login "; break;
                    case "companyid": sortby = " ORDER BY Logisticians.companyId "; break;
                    default: return BadRequest();
                }


                List<Logistician> logisticians = null;
                try
                {
                    string sql = $"SELECT * FROM Logisticians {where}";
                    if (sortby == "")
                        logisticians = await _context.Logisticians
                            .FromSqlRaw(sql)
                            .Include(l => l.Company)
                            .Include(l => l.RegData)
                            .Skip(offset)
                            .Take(limit)
                            .AsNoTracking()
                            //.DefaultIfEmpty()
                            .ToListAsync();
                    else
                        logisticians = await _context.Logisticians
                            .FromSqlRaw(sql)
                            .AsNoTracking()
                            //.DefaultIfEmpty()
                            .ToListAsync();
                    return logisticians;
                }
                catch (SqlException e)
                {
                    return BadRequest("Неверный JSON");
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

                //if (logisticians.Count <= offset) { logisticians.Clear(); return logisticians; }
                //if (logisticians.Count < offset + limit) { limit = logisticians.Count - offset; }
                //return logisticians.GetRange(offset, limit);
            }
        }

        // GET: api/Logisticians/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Logistician>> GetLogistician(int id)
        {
            var logistician = await _context.Logisticians
                    .Include(l => l.Company)
                    .Include(l => l.RegData)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(l => l.Id == id);

            if (logistician == null)
                return NotFound();

            return logistician;
        }

        // GET: api/logisticians/5/messages
        [HttpGet("{id}/{detail}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetLogisticianDetails(int id, string detail, int limit = 10, int offset = 0)
        {
            if (LogisticianExists(id))
            {
                if (detail.ToLower() == "messages")
                    return await _context.UserMessages
                        .Where(m => m.LogisticianId == id)
                        .Skip(offset)
                        .Take(limit)
                        .AsNoTracking()
                        //.DefaultIfEmpty()
                        .ToListAsync();

                //return BadRequest();
            }

            return NotFound();
        }

        /* put
        // PUT: api/Logisticians/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLogistician(int id, Logistician logistician)
        {
            if (id != logistician.Id)
            {
                return BadRequest();
            }

            _context.Entry(logistician).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogisticianExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        */

        // POST: api/Logisticians
        [HttpPost]
        public async Task<ActionResult<Logistician>> PostLogistician([FromBody] Logistician logistician)
        {
            if (logistician == null)
            {
                return BadRequest();
            }

            _context.Logisticians.Add(logistician);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLogistician", new { id = logistician.Id }, logistician);
        }

        // PATCH api/logisticians/5
        [HttpPatch("{id}")]
        public async Task<ActionResult<Client>> PatchLogistician(int id, [FromBody] Logistician logistician)
        {
            if (logistician == null || id != logistician.Id)
                return BadRequest();
            if (!LogisticianExists(id))
                return NotFound();

            Logistician x = await _context.Logisticians
                .Include(l => l.RegData)
                .FirstOrDefaultAsync<Logistician>(x => x.Id == id);

            if (logistician.RegData != null)
            {
                if (logistician.RegData.Name != null)
                    x.RegData.Name = logistician.RegData.Name;
                if (logistician.RegData.Password != null)
                    x.RegData.Password = logistician.RegData.Password;

            }
            if (logistician.CompanyId > 0)
                x.CompanyId = logistician.CompanyId;

            _context.Entry(x).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(logistician);
        }

        // DELETE: api/Logisticians/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Logistician>> DeleteLogistician(int id)
        {
            var logistician = await _context.Logisticians
                .Include(l => l.RegData)
                .FirstOrDefaultAsync(l => l.Id == id);
            if (logistician == null)
                return NotFound();

            // UserRegData logisticianReg = await _context.UserRegData
            //.FirstOrDefaultAsync(r => r.Login == logistician.Login);
           // _context.Logisticians.Remove(logistician);

            _context.UserRegData.Remove(logistician.RegData);
            await _context.SaveChangesAsync();

            return Ok(logistician);
        }

        private bool LogisticianExists(int id)
        {
            return _context.Logisticians.Any(e => e.Id == id);
        }
    }
}
