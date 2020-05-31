using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CargoApp.Models;
using System.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using CargoApp.Tools;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace CargoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public DriversController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Drivers https://localhost:44341/api/drivers?filter={%22field%22:%22d%22,%22operand%22:%22d%22,%22value%22:%22e%22}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Driver>>> GetDrivers(string orderby = "id", int limit = 10, int offset = 0, string filters = null) // "[{\"field\":\"id\",\"operator\":\">\",\"value\":\"2\"}]")
        {
            if (filters == null && orderby.ToLower() == "id")
            {
                return await _context.Drivers
                    .Include(d => d.RegData)
                    .Include(d => d.Company)
                    .Include(d => d.Cars)
                    .Include(d => d.DeliveryArea)
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

                    where = filtersObj.GetWhere("Drivers");
                }
                string sortby = "";
                switch (orderby.ToLower())
                {
                    case "id": break;
                    case "login": sortby = " ORDER BY Drivers.login "; break;
                    case "companyid": sortby = " ORDER BY Drivers.companyId "; break;
                    default: return BadRequest();
                }

                List<Driver> drivers = null;
                try
                {
                    string sql = $"SELECT * FROM Drivers {where}";
                    if (sortby == "")
                        drivers = await _context.Drivers
                            .FromSqlRaw(sql)
                            .Include(d => d.RegData)
                            .Include(d => d.Company)
                            .Include(d => d.Cars)
                            .Include(d => d.DeliveryArea)
                            .Skip(offset)
                            .Take(limit)
                            .AsNoTracking()
                            //.DefaultIfEmpty()
                            .ToListAsync();
                    else
                        drivers = await _context.Drivers
                            .FromSqlRaw(sql)
                            .AsNoTracking()
                            //.DefaultIfEmpty()
                            .ToListAsync();
                    return drivers;
                }
                catch (SqlException e)
                {
                    return BadRequest("Неверный JSON");
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

                //if (drivers.Count <= offset) { drivers.Clear(); return drivers; }
                //if (drivers.Count < offset + limit) { limit = drivers.Count - offset; }
                //return drivers.GetRange(offset, limit);
            }

        }

        // GET: api/Drivers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Driver>> GetDriver(int id)
        {
            var driver = await _context.Drivers
                .Include(d => d.RegData)
                .Include(d => d.Company)
                .Include(d => d.Cars)
                .Include(d => d.DeliveryArea)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);

            if (driver == null)
                return NotFound();
            else
                return driver;
        }

        // GET: api/Drivers/5/requests
        [HttpGet("{id}/{detail}")]
        public async Task<ActionResult<IEnumerable<object>>> GetDriverDetails(int id, string detail, int limit = 10, int offset = 0)
        {
            if (DriverExists(id))
            {
                if (detail.ToLower() == "requests")
                    return await _context.Requests
                       .Include(r => r.SendingAddress)
                       .Include(r => r.ReceivingAddress)
                       .Include(r => r.Goods)
                       .Where(r => r.DriverId == id)
                       .Skip(offset)
                       .Take(limit)
                       .AsNoTracking()
                       //.DefaultIfEmpty()
                       .ToListAsync();
                if (detail.ToLower() == "cars")
                    return await _context.Cars
                       .Where(c => c.DriverId == id)
                       .Skip(offset)
                       .Take(limit)
                       .AsNoTracking()
                       //.DefaultIfEmpty()
                       .ToListAsync();
            }

            return NotFound();
        }

        /* put
        // PUT: api/Drivers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriver(int id, Driver driver)
        {
            if (driver == null || id != driver.Id)
            {
                return BadRequest();
            }

            _context.Entry(driver).State = EntityState.Modified;

            if (!DriverExists(id))
            {
                return NotFound();
            }

            _context.Update(driver);
            await _context.SaveChangesAsync();
            return Ok(driver);

        }
        */

        // POST: api/Drivers
        [HttpPost]
        public async Task<ActionResult<Driver>> PostDriver([FromBody] Driver driver)
        {
            if (driver == null)
            {
                return BadRequest();
            }

            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriver", new { id = driver.Id }, driver);
        }

        // POST: api/Drivers/5/car
        [HttpPost("{id}/{detail}")]
        public async Task<ActionResult<Driver>> PostDriverDetail(int id, string detail, [FromBody] JObject obj)
        {
            if (!DriverExists(id))
                return NotFound();

            if (obj == null)
                return BadRequest();

            if (detail.ToLower() == "car")
            {
                Car car = null;
                try { car = obj.ToObject<Car>(); } //if (obj != null) car = JsonConvert.DeserializeObject<Car>(obj); }
                catch { return BadRequest(); }

              //  var driver = await _context.Drivers.Include(x => x.Car).FirstOrDefaultAsync(x => x.Id == id);

                if (car == null || car.Number == null)
                    return BadRequest();

                car.Number = car.Number.ToUpper();
                car.DriverId = id;
                _context.Cars.Add(car);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCar", new { id = car.Id, }, car);
            }

            if (detail.ToLower() == "deliveryArea")
            {
                DeliveryArea area = null;
                try { area = obj.ToObject<DeliveryArea>(); }
                catch { return BadRequest(); }

                var driver = await _context.Drivers
                    .Include(x => x.DeliveryArea)
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (driver.DeliveryArea != null || area == null)
                    return BadRequest();

                driver.DeliveryArea = area;
                _context.Entry(driver).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetDriver", new { id = driver.Id, }, driver);
            }

            return NotFound();
        }

        // PATCH api/drivers/5
        [HttpPatch("{id}")]
        public async Task<ActionResult<Client>> PatchDriver(int id, [FromBody] Driver driver)
        {
            if (driver == null || id != driver.Id)
                return BadRequest();

            Driver x = await _context.Drivers
                .Include(d => d.RegData)
                //.Include(d => d.Cars)
                .Include(d => d.DeliveryArea)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (x == null) 
                return NotFound();

            if (driver.RegData != null)
            {
                if (driver.RegData.Name != null)
                    x.RegData.Name = driver.RegData.Name;
                if (driver.RegData.Password != null)
                    x.RegData.Password = driver.RegData.Password;

            }
            if (driver.PhoneNumber != null)
                x.PhoneNumber = driver.PhoneNumber;
            if (driver.DeliveryArea != null)
            {
                if (x.DeliveryArea == null)
                    x.DeliveryArea = new DeliveryArea();
                
                if (driver.DeliveryArea.Latitude != null)
                    x.DeliveryArea.Latitude = driver.DeliveryArea.Latitude;
                if (driver.DeliveryArea.Longitude != null)
                    x.DeliveryArea.Longitude = driver.DeliveryArea.Longitude;
                /*
                if (driver.DeliveryArea.RealLatitude != null)
                    x.DeliveryArea.Latitude = (int)(driver.DeliveryArea.RealLatitude * 1000000);
                else if (driver.DeliveryArea.Latitude != null)
                    x.DeliveryArea.Latitude = driver.DeliveryArea.Latitude;

                if (driver.DeliveryArea.RealLongitude != null)
                    x.DeliveryArea.Longitude = (int)(driver.DeliveryArea.RealLongitude * 1000000);
                else if (driver.DeliveryArea.Longitude != null)
                    x.DeliveryArea.Longitude = driver.DeliveryArea.Longitude;
                */
                if (driver.DeliveryArea.Radius != null)
                    x.DeliveryArea.Radius = driver.DeliveryArea.Radius;
            }

            if (driver.CompanyId != 0)
                x.CompanyId = driver.CompanyId;

            /*
            if (driver.Car != null && driver.Car.Number != null)
            {
                if (x.Car == null)
                    x.Car = new Car();

                if (driver.Car.Number != null)
                    x.Car.Number = driver.Car.Number;
                if (driver.Car.Model != null)
                    x.Car.Model = driver.Car.Model;
                if (driver.Car.Volume != null)
                    x.Car.Volume = driver.Car.Volume;
                if (driver.Car.Carrying != null)
                    x.Car.Carrying = driver.Car.Carrying;
            }
            */

            _context.Entry(x).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return Ok(await _context.Drivers
                .Include(d => d.RegData)
                //.Include(d => d.Cars)
                .Include(d => d.DeliveryArea)
                .FirstOrDefaultAsync(d => d.Id == id));
        }

        // PATCH api/drivers/5/deliveryarea
        [HttpPatch("{id}/{detail}")]
        public async Task<ActionResult<Client>> PatchDriverDetail(int id, string detail, [FromBody] JObject obj)
        {
            if (!DriverExists(id))
                return NotFound();
            if (obj == null)
                return BadRequest();

            if (detail.ToLower() == "deliveryarea")
            {
                DeliveryArea deliveryArea = null;
                try { deliveryArea = obj.ToObject<DeliveryArea>(); }
                catch { return BadRequest(); }

                Driver driver = await _context.Drivers
                    .Include(d => d.DeliveryArea)
                    .FirstOrDefaultAsync(d => d.Id == id);

                //DeliveryArea x = driver.DeliveryArea;
                //if (x == null) return NotFound();

                if (driver.DeliveryArea == null)
                    driver.DeliveryArea = new DeliveryArea();

               // if (deliveryArea.RealLatitude != null)
                 //   driver.DeliveryArea.Latitude = (int)(deliveryArea.RealLatitude * 1000000);
               // else 
                if (deliveryArea.Latitude != null)
                    driver.DeliveryArea.Latitude = deliveryArea.Latitude;

               // if (deliveryArea.RealLongitude != null)
                 //   driver.DeliveryArea.Longitude = (int)(deliveryArea.RealLongitude * 1000000);
                //else 
                if (deliveryArea.Longitude != null)
                    driver.DeliveryArea.Longitude = deliveryArea.Longitude;

                if (deliveryArea.Radius != null)
                    driver.DeliveryArea.Radius = deliveryArea.Radius;

                _context.Entry(driver).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return Ok(await _context.Drivers
                    .Include(d => d.DeliveryArea)
                    .FirstOrDefaultAsync(d => d.Id == id));
            }            

            return NotFound();
        }

        // PATCH api/drivers/5/car/1
        [HttpPatch("{id}/{detail}/{detailId}")]
        public async Task<ActionResult<Client>> PatchDriverDetailWithId(int id, string detail, int detailId, [FromBody] JObject obj)
        {
            if (!DriverExists(id))
                return NotFound();
            if (obj == null)
                return BadRequest();

            if (detail.ToLower() == "car")
            {
                Car car = null;
                try { car = obj.ToObject<Car>(); }
                catch { return BadRequest(); }

                Car x = await _context.Cars
                    .Where(c => c.DriverId == id)
                    .FirstOrDefaultAsync(c => c.Id == detailId);

                if (x == null)
                    return NotFound();

                if (car.Number != null)
                    x.Number = car.Number;
                if (car.Model != null)
                    x.Model = car.Model;
                if (car.Length != null)
                    x.Length = car.Length;
                if (car.Height != null)
                    x.Height = car.Height;
                if (car.Width != null)
                    x.Width = car.Width;
                if (car.Carrying != null)
                    x.Carrying = car.Carrying;

                _context.Entry(x).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return Ok(await _context.Cars
                    .FirstOrDefaultAsync(d => d.DriverId == id));
            }

            return NotFound();
        }

        // DELETE: api/Drivers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Driver>> DeleteDriver(int id)
        {
            var driver = await _context.Drivers
                .Include(d => d.RegData)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (driver == null)
                return NotFound();

            var requests = await _context.Requests
                .Where(r => r.DriverId == id)
                .ToListAsync();
            foreach(Request r in requests)
            {
                r.DriverId = null;
                _context.Entry(r).State = EntityState.Modified;
            }

            _context.UserRegData.Remove(driver.RegData);

            //var driver = await _context.Drivers.FindAsync(id);
            //if (driver == null) return NotFound();
            //_context.Drivers.Remove(driver);

            await _context.SaveChangesAsync();

            return Ok(driver);
        }

        // DELETE: api/Drivers/5/car
        [HttpDelete("{id}/{detail}/{detailId}")]
        public async Task<ActionResult<object>> DeleteDriverDetail(int id, string detail, int detailId)
        {
            if (!DriverExists(id))
                return NotFound();
            
            if (detail.ToLower() == "car")
            {
                Car car = await _context.Cars
                    .Where(c => c.DriverId == id)
                    .FirstOrDefaultAsync(c => c.Id == detailId);

                if (car == null)
                    return NotFound();

                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();

                return car;
            }

            return NotFound();
        }

        private bool DriverExists(int id)
        {
            return _context.Drivers.Any(e => e.Id == id);
        }
    }
}
