using BreweryApi.Models;
using BreweryApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BreweryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreweriesController : ControllerBase
    {
        private IBreweryRepository _breweryRepository;

        public BreweriesController( IBreweryRepository breweryRepository )
        {
            _breweryRepository = breweryRepository;
        }

        // GET: api/Breweries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brewery>>> GetBrewery()
        {
            List<Brewery> Breweries = (List<Brewery>)_breweryRepository.getBreweries();

            foreach(var brewery in Breweries)
            {
                brewery.Beers = (ICollection<Beer>)_breweryRepository.GetBreweryBeers(brewery);
            }

            return Breweries;
        }

        // GET: api/Breweries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Brewery>> GetBrewery(int id)
        {
            var brewery = _breweryRepository.getBreweryByID(id);
            brewery.Beers = (ICollection<Beer>)_breweryRepository.GetBreweryBeers(brewery);

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

            _breweryRepository.UpdateBrewery(brewery);

            try
            {
                _breweryRepository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_breweryRepository.BreweryExists(id))
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
            _breweryRepository.InsertBrewery(brewery);

            return CreatedAtAction("GetBrewery", new { id = brewery.Id }, brewery);
        }

        // DELETE: api/Breweries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrewery(int id)
        {
            var brewery = _breweryRepository.getBreweryByID(id);
            if (brewery == null)
            {
                return NotFound();
            }

            _breweryRepository.DeleteBrewery(brewery);

            return NoContent();
        }
    }
}
