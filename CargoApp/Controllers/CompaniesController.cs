﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CargoApp.Models;
using CargoApp.Tools;
using Microsoft.Data.SqlClient;
using AutoMapper;

namespace CargoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public CompaniesController(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies(int limit = 10, int offset = 0, string filters = null)
        {
            if (filters == null)
            {
                return await _context.Companies
                    .Include(c => c.Area)
                    .Include(c => c.Address)
                    .Skip(offset)
                    .Take(limit)
                    // .DefaultIfEmpty()
                    .ToListAsync();
            }
            else
            {
                Filters filtersObj = new Filters(filters);
                if (filtersObj.Message != null)
                    return BadRequest(filtersObj.Message);

                string where = filtersObj.GetWhere("Companies");

                List<Company> companies = null;
                try
                {
                    string sql = $"SELECT * FROM Companies {where}";
                    companies = await _context.Companies
                        .FromSqlRaw(sql)
                        .Include(c => c.Area) 
                        .Include(c => c.Address)
                        .Skip(offset)
                        .Take(limit)
                        //.DefaultIfEmpty()
                        .ToListAsync();
                    return companies;
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

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            var company = await _context.Companies
                .Include(c => c.Area)
                .Include(c => c.Address)  //geo api
                .FirstOrDefaultAsync(x => x.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // GET: api/Companies/5/drivers
        [HttpGet("{id}/{detail}")]
        public async Task<ActionResult<Object>> GetCompanyDetails(int id, string detail, int limit = 10, int offset = 0)
        {
            if (CompanyExists(id))
            {
                if (detail.ToLower() == "drivers")
                    return await _context.Drivers
                        .Include(d => d.DeliveryArea)
                        .Include(d => d.Requests)
                        .Where(d => d.CompanyId == id)
                        .Skip(offset)
                        .Take(limit)
                        //.DefaultIfEmpty()
                        .ToListAsync();
                if (detail.ToLower() == "logisticians")
                    return await _context.Logisticians
                        .Where(l => l.CompanyId == id)
                        .Skip(offset)
                        .Take(limit)
                        //.DefaultIfEmpty()
                        .ToListAsync();
                if (detail.ToLower() == "marks")
                    return await _context.Ratings
                        .Where(r => r.CompanyId == id)
                        .Skip(offset)
                        .Take(limit)
                        //.DefaultIfEmpty()
                        .ToListAsync();
                if (detail.ToLower() == "requests")
                {
                    return await _context.Requests
                        .Include(r => r.CurrentStatus)
                        .Include(r => r.SendingAddress)
                        .Include(r => r.ReceivingAddress)
                        .Include(r => r.Goods)
                        //.Include(r => r.Messages)
                        .Skip(offset)
                        .Take(limit)
                        .ToListAsync();

                    /* другой алгоритм
                    List<Driver> companyDrivers = await _context.Drivers
                        .Include(d => d.Requests)
                            .ThenInclude(r => r.CurrentStatus)
                        .Where(d => d.CompanyId == id)
                        .Where(d => d.Requests != null)
                        .OrderBy(d => d.Requests.date) ????
                        .Take(limit + offset)
                        .ToListAsync();
                    List<Request> requests = new List<Request>();
                    int count = 0;
                    foreach (Driver d in companyDrivers)
                    {
                        foreach (Request r in d.Requests)
                        {
                            if (count < offset) { count++; continue; }
                            
                            requests.Add(r);
                            count++;
                            if (count == limit + offset) break;
                        }
                        if (count == limit + offset) break;
                    }
                    return requests;
                    */
                }

                return BadRequest();
            }

            return NotFound();
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, Company company)
        {
            if (company != null && id != company.Id)
            {
                return BadRequest();
            }

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
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

        // PATCH api/companies/5
        [HttpPatch("{id}")]
        public async Task<ActionResult<Company>> PatchCompany([FromRoute] int id, [FromBody] Company company)
        {
            if (company == null)
                return BadRequest();

            Company x = await _context.Companies
                .Include(c => c.Address)
                .Include(c => c.Area)
                .FirstOrDefaultAsync<Company>(x => x.Id == id);

            if (x == null) 
                return NotFound();

            if (company.Name != null)
                x.Name = company.Name;
            if (company.MaxCarrying != null)
                x.MaxCarrying = company.MaxCarrying;
            if (company.MaxVolume != null)
                x.MaxVolume = company.MaxVolume;

            if (company.Area != null)
            {
                if (x.Area == null)
                    x.Area = new DeliveryArea();

                if (company.Area.Radius != null)
                    x.Area.Radius = company.Area.Radius;

                if (company.Area.RealLatitude != null)
                    x.Area.Latitude = (int)(company.Area.RealLatitude * 1000000);
                else if (company.Area.Latitude != null)
                    x.Area.Latitude = company.Area.Latitude;

                if (company.Area.RealLongitude != null)
                    x.Area.Longitude = (int)(company.Area.RealLongitude * 1000000);
                else if (company.Area.Radius != null)
                    x.Area.Longitude = company.Area.Longitude;
            }

            if (company.Address != null)
            {
                if (x.Address == null)
                    x.Address = new Address();

                //if (company.Address.RealLatitude != null)
                // гео апи
                //if (company.Address.RealLongitude != null)
                // гео апи

                if (company.Address.Index != null)
                    x.Address.Index = company.Address.Index;
                if (company.Address.Country != null)
                    x.Address.Country = company.Address.Country;
                if (company.Address.Region != null)
                    x.Address.Region = company.Address.Region;
                if (company.Address.City != null)
                    x.Address.City = company.Address.City;
                if (company.Address.Street != null)
                    x.Address.Street = company.Address.Street;
                if (company.Address.House != null)
                    x.Address.House = company.Address.House;
                if (company.Address.Flat != null)
                    x.Address.Flat = company.Address.Flat;
                if (company.Address.Addition != null)
                    x.Address.Addition = company.Address.Addition;
            }
            // Mapper

            if (company.PhoneNumber != null)
                x.PhoneNumber = company.PhoneNumber;
            if (company.Email != null)
                x.Email = company.Email;
            if (company.RegistrationDate != null)
                x.RegistrationDate = company.RegistrationDate;
            if (company.Inn != null)
                x.Inn = company.Inn;
            if (company.Kpp != null)
                x.Kpp = company.Kpp;
            if (company.Ogrn != null)
                x.Ogrn = company.Ogrn;
            if (company.OgrnDate != null)
                x.OgrnDate = company.OgrnDate;


            _context.Entry(x).State = EntityState.Modified;


            await _context.SaveChangesAsync();
            
            return Ok(await _context.Companies
                .Include(c => c.Address)
                .Include(c => c.Area)
                .FirstOrDefaultAsync<Company>(x => x.Id == id));
        }

        // POST: api/Companies
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany(Company company)
        {
            if (company == null)
                return BadRequest();

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompany", new { id = company.Id }, company);
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Company>> DeleteCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return company;
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}