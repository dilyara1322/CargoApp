﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CargoApp.Models;
using CargoApp.Tools;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace CargoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public RequestsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests(string orderby = "id", int limit = 10, int offset = 0, string filters = null)
        {
            if (filters == null && orderby.ToLower() == "id")
            {
                return await _context.Requests
                    .Include(r => r.Client)
                    .Include(r => r.Company)
                    .Include(r => r.Driver)
                    .Include(r => r.SendingAddress)
                    .Include(r => r.ReceivingAddress)
                    .Include(r => r.Goods)
                    // .Include(r => r.Messages)
                    .Skip(offset)
                    .Take(limit)
                    .AsNoTracking()
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

                    where = filtersObj.GetWhere("Requests");
                }
                string sortby = "";
                switch (orderby.ToLower())
                {
                    case "id": break;
                    case "clientid": sortby = " ORDER BY Requests.clientid "; break;
                    case "companyid": sortby = " ORDER BY Requests.companyid "; break;
                    case "driverid": sortby = " ORDER BY Requests.driverid "; break;
                    case "currentstatus": sortby = " ORDER BY Requests.currentstatus "; break;
                    case "sendingdatetime": sortby = " ORDER BY Requests.sendingdatetime "; break;
                    case "recievingdatetime": sortby = " ORDER BY Requests.recievingdatetime "; break;
                    default: return BadRequest();
                }

                List<Request> requests = null;
                try
                {
                    string sql = $"SELECT * FROM Requests {where}";
                    if (sortby == "")
                        requests = await _context.Requests
                            .FromSqlRaw(sql)
                            .Include(r => r.Client)
                            .Include(r => r.Company)
                            .Include(r => r.Driver)
                            .Include(r => r.SendingAddress)
                            .Include(r => r.ReceivingAddress)
                            .Include(r => r.Goods)
                            .Skip(offset)
                            .Take(limit)
                            .AsNoTracking()
                            //.DefaultIfEmpty()
                            .ToListAsync();

                    else
                        requests = await _context.Requests
                            .FromSqlRaw(sql)
                            .AsNoTracking()
                            //.DefaultIfEmpty()
                            .ToListAsync();
                    return requests;
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

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.Requests
                .Include(r => r.Client)
                .Include(r => r.Company)
                .Include(r => r.Driver)
                .Include(r => r.SendingAddress)
                .Include(r => r.ReceivingAddress)
                .Include(r => r.Goods)
                //.Include(r => r.Messages)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
                return NotFound();

            return request;
        }

        [HttpGet("{id}/{detail}")]
        public async Task<ActionResult<IEnumerable<object>>> GetRequestDetail(int id, string detail, int limit = 10, int offset = 0)
        {
            if (!RequestExists(id))
                return NotFound();

            if (detail.ToLower() == "messages")
            {
                return await _context.UserMessages
                    .Where(m => m.RequestId == id)
                    .Skip(offset)
                    .Take(limit)
                    .AsNoTracking()
                    .ToListAsync();
            }

            if (detail.ToLower() == "goods")
            {
                return await _context.Goods
                    .Where(g => g.RequestId == id)
                    .Skip(offset)
                    .Take(limit)
                    .AsNoTracking()
                    .ToListAsync();
            }

            if (detail.ToLower() == "currentplace")
            {
                string path = $"http://...?id={id}";
                string jsonStr = "";

                try
                {
                    System.Net.WebRequest reqGET = System.Net.WebRequest.Create(path);
                    System.Net.WebResponse resp = reqGET.GetResponse();
                    System.IO.Stream stream = resp.GetResponseStream();
                    System.IO.StreamReader sr = new System.IO.StreamReader(stream);
                    jsonStr = sr.ReadToEnd();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

                JObject j;
                try 
                {
                    j = JObject.Parse(jsonStr);
                    float latitude = j.GetValue("Latitude").ToObject<float>();
                    float longitude = j.GetValue("Longitude").ToObject<float>();

                    var request = await _context.Requests
                        .FirstOrDefaultAsync(r => r.Id == id);
                    request.CurrentLatitude = latitude;
                    request.CurrentLongitude = longitude;

                    return Ok(request);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            return NotFound();
        }

        /* put
        // PUT: api/Requests/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (request == null || id != request.Id)
            {
                return BadRequest();
            }
            if (!RequestExists(id))
            {
                return NotFound();
            }

            _context.Entry(request).State = EntityState.Modified;

            _context.Update(request);
            await _context.SaveChangesAsync();
            return Ok(request);
        }
        */

        // POST: api/Requests
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest([FromBody] Request request)
        {
            if (request == null)
                return BadRequest();

            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        [HttpPost("{id}/{detail}")]
        public async Task<ActionResult<Request>> PostRequestDetail(int id, string detail, [FromBody] JObject obj)
        {
            if (!RequestExists(id))
                return NotFound();
            if (obj == null)
                return BadRequest();

            if (detail.ToLower() == "message")
            {
                UserMessage message = null;
                try { message = obj.ToObject<UserMessage>(); } 
                catch { return BadRequest(); }

                if (message == null || message.ClientId <= 0 || message.LogisticianId <= 0)
                    return BadRequest();

                message.RequestId = id;
                _context.UserMessages.Add(message);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetMessage", new { id = message.Id, }, message);
            }

            if (detail.ToLower() == "good")
            {
                Good good = null;
                try { good = obj.ToObject<Good>(); }
                catch { return BadRequest(); }

                if (good == null)
                    return BadRequest();

                good.RequestId = id;
                _context.Goods.Add(good);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetGood", new { id = good.Id, }, good);
            }

            return NotFound();
        }

        // PATCH api/requests/5
        [HttpPatch("{id}")]
        public async Task<ActionResult<Client>> PatchRequest(int id, [FromBody] Request request)
        {
            if (request == null)
                return BadRequest();

            Request x = await _context.Requests
                .Include(x => x.SendingAddress)
                .Include(x => x.ReceivingAddress)
                //.Include(x => x.Goods)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (x == null)
                return NotFound();

            if (request.CompanyId != null)
                x.CompanyId = request.CompanyId;
            if (request.DriverId != null)
                x.DriverId = request.DriverId;
            if (request.CurrentStatus != null)
                x.CurrentStatus = request.CurrentStatus;

            if (request.SendingAddress != null)
            {
                if (x.SendingAddress == null)
                    x.SendingAddress = new Address();

                if (request.SendingAddress.Index != null)
                    x.SendingAddress.Index = request.SendingAddress.Index;
                if (request.SendingAddress.Country != null)
                    x.SendingAddress.Country = request.SendingAddress.Country;
                if (request.SendingAddress.Region != null)
                    x.SendingAddress.Region = request.SendingAddress.Region;
                if (request.SendingAddress.City != null)
                    x.SendingAddress.City = request.SendingAddress.City;
                if (request.SendingAddress.Street != null)
                    x.SendingAddress.Street = request.SendingAddress.Street;
                if (request.SendingAddress.House != null)
                    x.SendingAddress.House = request.SendingAddress.House;
                if (request.SendingAddress.Flat != null)
                    x.SendingAddress.Flat = request.SendingAddress.Flat;
                if (request.SendingAddress.Addition != null)
                    x.SendingAddress.Addition = request.SendingAddress.Addition;
            }
            if (request.ReceivingAddress != null)
            {
                if (x.ReceivingAddress == null)
                    x.ReceivingAddress = new Address();

                if (request.ReceivingAddress.Index != null)
                    x.ReceivingAddress.Index = request.ReceivingAddress.Index;
                if (request.ReceivingAddress.Country != null)
                    x.ReceivingAddress.Country = request.ReceivingAddress.Country;
                if (request.ReceivingAddress.Region != null)
                    x.ReceivingAddress.Region = request.ReceivingAddress.Region;
                if (request.ReceivingAddress.City != null)
                    x.ReceivingAddress.City = request.ReceivingAddress.City;
                if (request.ReceivingAddress.Street != null)
                    x.ReceivingAddress.Street = request.ReceivingAddress.Street;
                if (request.ReceivingAddress.House != null)
                    x.ReceivingAddress.House = request.ReceivingAddress.House;
                if (request.ReceivingAddress.Flat != null)
                    x.ReceivingAddress.Flat = request.ReceivingAddress.Flat;
                if (request.ReceivingAddress.Addition != null)
                    x.ReceivingAddress.Addition = request.ReceivingAddress.Addition;
            }

            if (request.SendingDateTime != null)
                x.SendingDateTime = request.SendingDateTime;
            if (request.ReceivingDateTime != null)
                x.ReceivingDateTime = request.ReceivingDateTime;
            if (request.Addition != null)
                x.Addition = request.Addition;

            _context.Entry(x).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return Ok(await _context.Requests
                .Include(x => x.SendingAddress)
                .Include(x => x.ReceivingAddress)
               // .Include(x => x.Goods)
                .FirstOrDefaultAsync(x => x.Id == id));
        }

        // PATCH api/requests/5/good/7
        [HttpPatch("{id}/{detail}/{detailId}")]
        public async Task<ActionResult<Client>> PatchRequestDetails(int id, string detail, int detaild, [FromBody] JObject obj)
        {
            if (!RequestExists(id))
                return NotFound();
            if (obj == null)
                return BadRequest();

            if (detail.ToLower() == "good")
            {
                Good good = null;
                try { good = obj.ToObject<Good>(); }
                catch { return BadRequest(); }

                Good x = await _context.Goods
                    .Where(g => g.RequestId == id)
                    .FirstOrDefaultAsync(g => g.Id == detaild);

                if (x == null || good == null)
                    return NotFound();

                if (good.Type != null)
                    x.Type = good.Type;
                if (good.Name != null)
                    x.Name = good.Name;
                if (good.Weight != null)
                    x.Weight = good.Weight;
                if (good.Length != null)
                    x.Length = good.Length;
                if (good.Height != null)
                    x.Height = good.Height;
                if (good.Width != null)
                    x.Width = good.Width;
                if (good.Type != null)
                    x.Type = good.Type;

                _context.Entry(x).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return Ok(await _context.Goods
                    .FirstOrDefaultAsync(g => g.Id == detaild));
            }

            return NotFound();
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Request>> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return request;
        }

        // DELETE: api/Requests/5/driver
        [HttpDelete("{id}/{detail}")]
        public async Task<ActionResult<object>> DeleteRequestDetail(int id, string detail)
        {
            if (!RequestExists(id))
                return NotFound();

            if (detail.ToLower() == "driver")
            {
                Request request = await _context.Requests
                    .Include(r => r.Driver)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (request.Driver == null)
                    return NotFound();

                request.Driver = null;
                request.DriverId = null;

                _context.Entry(request).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(request);
            }

            if (detail.ToLower() == "company")
            {
                Request request = await _context.Requests
                    .Include(r => r.Company)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (request.Company == null)
                    return NotFound();

                request.Company = null;
                request.CompanyId = null;

                _context.Entry(request).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(request);
            }

            return NotFound();
        }

        // DELETE: api/Requests/5/good/5
        [HttpDelete("{id}/{detail}/{detailId}")]
        public async Task<ActionResult<object>> DeleteRequestDetailWithId(int id, string detail, int detailId)
        {
            if (!RequestExists(id))
                return NotFound();

            if (detail.ToLower() == "good")
            {
                Good good = await _context.Goods
                    .Where(g => g.RequestId == id)
                    .FirstOrDefaultAsync(g => g.Id == detailId);

                if (good == null)
                    return NotFound();

                _context.Goods.Remove(good);
                await _context.SaveChangesAsync();

                return good;
            }

            return NotFound();
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
