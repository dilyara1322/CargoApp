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
        public async Task<ActionResult<IEnumerable<Car>>> Get()
        {
            return await _context.Cars
                .Include(c => c.Driver)
                .DefaultIfEmpty()
                .ToListAsync();
        }

        // GET api/<controller>/5
        [HttpGet("{number}")]
        public async Task<ActionResult<Car>> Get(string number)
        {
            var car = await _context.Cars
                .Include(c => c.Driver)
                .FirstOrDefaultAsync(x => x.Number == number);

            if (car == null)
            {
                return NotFound();
            }
            return new ObjectResult(car);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult<Car>> Post([FromBody] Car car)
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
        [HttpPut("{number}")]
        public async Task<ActionResult<Car>> Put(string number, [FromBody] Car car)
        {
            if (car == null)
            {
                return BadRequest();
            }
            if (!_context.Cars.Any(x => x.Number == number))
            {
                return NotFound();
            }

            _context.Update(car);
            await _context.SaveChangesAsync();
            return Ok(car);
        }

        // PATCH api/cars/5yft
        [HttpPatch("{number}")]
        public async Task<ActionResult<Client>> Patch([FromRoute] string number, [FromBody] Car car)
        {
            if (car == null)
            {
                return BadRequest();
            }
            if (!_context.Cars.Any(x => x.Number == number))
            {
                return NotFound();
            }

            Car x = _context.Cars.FirstOrDefault<Car>(x => x.Number == number);
            if (x != null)
            {
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
        [HttpDelete("{number}")]
        public async Task<ActionResult<Car>> Delete(string number)
        {
            var car = await _context.Cars.FindAsync(number);
            if (car == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return Ok(car);
        }
    }
}
