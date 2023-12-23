using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BreweryApi.Models;
using BreweryApi.Services;
using BreweryApi.Models.DTOs;

namespace BreweryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeersController : ControllerBase
    {
        private readonly BreweryContext _context;
        private readonly BreweryService _breweryService;

        public BeersController(BreweryContext context, BreweryService breweryService)
        {
            _context = context;
            _breweryService = breweryService;
        }

        // GET: api/Beers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BeerDTO>>> GetBeer()
        {
            var beers = await _context.Beer.ToListAsync();
            var beersDTO = new List<BeerDTO>();

            foreach (var beer in beers)
            {
                var brewery = _context.Brewery.First(b => b.Id == beer.BreweryId);

                beersDTO.Add(new BeerDTO
                {
                    Id = beer.Id,
                    Name = beer.Name,
                    Age = beer.Age,
                    Brewery = brewery,
                    BreweryId = beer.BreweryId,
                    BreweryPrice = beer.BreweryPrice,
                    Flavour = beer.Flavour
                });
            }

            return beersDTO;
        }

        // GET: api/Beers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BeerDTO>> GetBeer(int id)
        {
            var beer = await _context.Beer.FindAsync(id);

            if (beer == null)
            {
                return NotFound();
            }

            var brewery = _context.Brewery.First(b => b.Id == beer.Id);

            var dto = new BeerDTO
            {

                Id = beer.Id,
                Name = beer.Name,
                Age = beer.Age,
                Brewery = brewery,
                BreweryId = beer.BreweryId,
                BreweryPrice = beer.BreweryPrice,
                Flavour = beer.Flavour
            };

            return dto;
        }

        // PUT: api/Beers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBeer(int id, Beer beer)
        {
            if (id != beer.Id)
            {
                return BadRequest();
            }

            _context.Entry(beer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeerExists(id))
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

        // POST: api/Beers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Beer>> PostBeer(Beer beer)
        {

            Brewery brewery = await _context.Brewery.FindAsync(beer.BreweryId);
            if (brewery != null)
            {
                Console.WriteLine(brewery.Name);
                _breweryService.AddBeerToBrewery(brewery, beer);
            }

            _context.Beer.Add(beer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBeer), new { id = beer.Id }, beer);
        }

        // DELETE: api/Beers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBeer(int id)
        {
            var beer = await _context.Beer.FindAsync(id);
            if (beer == null)
            {
                return NotFound();
            }

            _context.Beer.Remove(beer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BeerExists(int id)
        {
            return _context.Beer.Any(e => e.Id == id);
        }
    }
}
