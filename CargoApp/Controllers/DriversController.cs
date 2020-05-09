using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CargoApp.Models;

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

        // GET: api/Drivers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Driver>>> GetDrivers()
        {
            return await _context.Drivers
                .Include(d => d.RegData)
                .Include(d => d.Company)
                .Include(d => d.Car)
                .Include(d => d.DeliveryArea)
                .DefaultIfEmpty()
                .ToListAsync();
        }

        // GET: api/Drivers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Driver>> GetDriver(int id)
        {
            var driver = _context.Drivers
                .Include(d => d.RegData)
                .Include(d => d.Company)
                .Include(d => d.Car)
                .Include(d => d.DeliveryArea)
                .FirstOrDefault(d => d.Id == id);

            if (driver == null)
            {
                return NotFound();
            }

            return driver;
        }

        // PUT: api/Drivers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriver(int id, Driver driver)
        {
            if (driver == null || id != driver.Id)
            {
                return BadRequest();
            }

            //_context.Entry(driver).State = EntityState.Modified;

            if (!DriverExists(id))
            {
                return NotFound();
            }

            _context.Update(driver);
            await _context.SaveChangesAsync();
            return Ok(driver);

        }

        // POST: api/Drivers
        [HttpPost]
        public async Task<ActionResult<Driver>> PostDriver(Driver driver)
        {
            if (driver == null)
            {
                return BadRequest();
            }

            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriver", new { id = driver.Id }, driver);
            //return Ok(driver);
        }

        // PATCH api/drivers/5
        [HttpPatch("{id}")]
        public async Task<ActionResult<Client>> Patch([FromRoute] int id, [FromBody] Driver driver)
        {
            if (driver == null || id != driver.Id)
            {
                return BadRequest();
            }
            if (!DriverExists(id))
            {
                return NotFound();
            }

            Driver x = _context.Drivers.FirstOrDefault<Driver>(x => x.Id == id);
            if (x != null)
            {
                if (driver.RegData != null)
                {
                    if (driver.RegData.Name != null)
                        x.RegData.Name = driver.RegData.Name;
                    if (driver.RegData.Password != null)
                        x.RegData.Password = driver.RegData.Password;
                  
                }
                if (driver.DeliveryArea != null)
                {
                    /*
                    if (driver.DeliveryArea.Latitude != null)
                        x.DeliveryArea.Latitude = driver.DeliveryArea.Latitude;
                    if (driver.DeliveryArea.Longitude != null)
                        x.DeliveryArea.Longitude = driver.DeliveryArea.Longitude;
                    */
                    if (driver.DeliveryArea.RealLatitude != null)
                        x.DeliveryArea.Latitude = (int)(driver.DeliveryArea.RealLatitude * 1000000);
                    else if (driver.DeliveryArea.Latitude != null)
                        x.DeliveryArea.Latitude = driver.DeliveryArea.Latitude;

                    if (driver.DeliveryArea.RealLongitude != null)
                        x.DeliveryArea.Longitude = (int)(driver.DeliveryArea.RealLongitude * 1000000);
                    else if (driver.DeliveryArea.Longitude != null)
                        x.DeliveryArea.Longitude = driver.DeliveryArea.Longitude;

                    if (driver.DeliveryArea.Radius != null)
                        x.DeliveryArea.Radius = driver.DeliveryArea.Radius;
                }
                if (driver.CompanyId != 0)
                    x.CompanyId = driver.CompanyId;
               // if (driver.Car != null && driver.Car.Number != null)
                 //   x.Car.Number = driver.Car.Number;

                _context.Entry(x).State = EntityState.Modified;
                //_context.SaveChanges();
            }

            await _context.SaveChangesAsync();
            return Ok(driver);
        }

        // DELETE: api/Drivers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Driver>> DeleteDriver(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();

            return driver;
        }

        private bool DriverExists(int id)
        {
            return _context.Drivers.Any(e => e.Id == id);
        }
    }
}
