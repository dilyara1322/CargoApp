using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CargoApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CargoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public CarsController(ApplicationContext context)
        {
            _context = context;

            if (!_context.Cars.Any<Car>())
            {
                //_context.Drivers.Add(new Driver { RegData = new UserRegData { Login = "dr1", Password = "dr", Salt = "0" },  Login = "dr1", Company = new Company { Name = "company1", Ogrn = "1234567891231", Inn = "123123123123", Kpp = "123123123" } });
                //_context.Drivers.Add(new Driver { RegData = new UserRegData { Login = "dr2", Password = "dr", Salt = "0" }, Login = "dr2", Company = new Company { Name = "company2", Ogrn = "4564564564564", Inn = "456546456456", Kpp = "456456456" } });
                //_context.SaveChanges();
                _context.Cars.Add(new Car { Number = "T123RT116RUS", DriverId = 2 });
                _context.Cars.Add(new Car { Number = "Y879RT116RUS", DriverId = 3 });
                _context.SaveChanges();
            }
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars(int limit = 10, int offset = 0)
        {
            return await _context.Cars
                .Include(c => c.Driver)
                .Skip(offset)
                .Take(limit)
                .DefaultIfEmpty()
                .ToListAsync();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(int id)
        {
            var car = await _context.Cars
                .Include(c => c.Driver)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (car == null)
            {
                return NotFound();
            }
            return car;
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult<Car>> PostCar([FromBody] Car car)
        {
            if (car == null)
            {
                return BadRequest();
            }

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            return Ok(car);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Car>> PutCar(int id, [FromBody] Car car)
        {
            if (car == null)
            {
                return BadRequest();
            }
            if (!CarExists(id))
            {
                return NotFound();
            }

            _context.Update(car);
            await _context.SaveChangesAsync();
            return Ok(car);
        }

        // PATCH api/cars/5yft
        [HttpPatch("{id}")]
        public async Task<ActionResult<Client>> PatchCar([FromRoute] int id, [FromBody] Car car)
        {
            if (car == null)
            {
                return BadRequest();
            }
            if (!CarExists(id))
            {
                return NotFound();
            }

            Car x = _context.Cars.FirstOrDefault<Car>(x => x.Id == id);
            if (x != null)
            {
                if (car.Number != null)
                    x.Number = (car.Number).ToUpper();
                if (car.Model != null)
                    x.Model = car.Model;
                if (car.Carrying != null)
                    x.Carrying = car.Carrying;
                if (car.Volume != null)
                    x.Volume = car.Volume;
                _context.Entry(x).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
            return Ok(car);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Car>> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return Ok(car);
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
