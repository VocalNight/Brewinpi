using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BreweryApi.Models;
using BreweryApi.Models.DTOs;
using BreweryApi.Repositories;

namespace BreweryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeersController : ControllerBase
    {
        private readonly IBeerRepository _beerRepository;
        private readonly IBreweryRepository _breweryRepository;

        public BeersController(IBeerRepository beerRepository, IBreweryRepository breweryRepository)
        {
            _beerRepository = beerRepository;
            _breweryRepository = breweryRepository;
        }

        // GET: api/Beers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BeerDTO>>> GetBeer()
        {
            var beers = _beerRepository.getBeers();
            var beersDTO = new List<BeerDTO>();

            foreach (var beer in beers)
            {
                var brewery = _breweryRepository.getBreweryByID(beer.BreweryId);

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
            var beer = _beerRepository.getBeerByID(id);

            if (beer == null)
            {
                return NotFound();
            }

            var brewery = _breweryRepository.getBreweryByID(beer.BreweryId);

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

            _beerRepository.UpdateBeer(beer);

            try
            {
                _beerRepository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_beerRepository.BeerExists(id))
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

            Brewery brewery = _breweryRepository.getBreweryByID(beer.BreweryId);

            _beerRepository.InsertBeer(beer);
            _beerRepository.SaveAsync();

            return CreatedAtAction(nameof(GetBeer), new { id = beer.Id }, beer);
        }

        // DELETE: api/Beers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBeer(int id)
        {
            var beer = _beerRepository.getBeerByID(id);
            if (beer == null)
            {
                return NotFound();
            }

            _beerRepository.DeleteBeer(beer);
            _beerRepository.SaveAsync();

            return NoContent();
        }
    }
}
