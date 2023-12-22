using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BreweryApi.Models;
using BreweryApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BreweryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreweriesController : ControllerBase
    {
        private readonly BreweryContext _context;
        private readonly BreweryService _breweryService;

        public BreweriesController(BreweryContext context, BreweryService breweryService)
        {
            _context = context;
            _breweryService = breweryService;
        }

        // GET: api/Breweries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brewery>>> GetBrewery()
        {
            var Breweries = await _context.Brewery.ToListAsync();

            foreach(var brewery in Breweries)
            {
                brewery.Beers = _context.Beer.Where(b => b.BreweryId == brewery.Id).ToList();
            }

            return Breweries;
        }

        // GET: api/Breweries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Brewery>> GetBrewery(int id)
        {
            var brewery = await _context.Brewery.FindAsync(id);
            brewery.Beers = _context.Beer.Where(b => b.BreweryId == id).ToList();
            

            if (brewery == null)
            {
                return NotFound();
            }

            return brewery;
        }

        // PUT: api/Breweries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrewery(int id, Brewery brewery)
        {
            if (id != brewery.Id)
            {
                return BadRequest();
            }

            _context.Entry(brewery).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BreweryExists(id))
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

        // POST: api/Breweries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Brewery>> PostBrewery(Brewery brewery)
        {
            _context.Brewery.Add(brewery);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBrewery", new { id = brewery.Id }, brewery);
        }

        // DELETE: api/Breweries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrewery(int id)
        {
            var brewery = await _context.Brewery.FindAsync(id);
            if (brewery == null)
            {
                return NotFound();
            }

            _context.Brewery.Remove(brewery);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BreweryExists(int id)
        {
            return _context.Brewery.Any(e => e.Id == id);
        }
    }
}
