using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BreweryApi.Models;
using BreweryApi.Repositories;
using Newtonsoft.Json;

namespace BreweryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WholesalersController : ControllerBase
    {
        private readonly ISalesRepository _salesRepository;
        private readonly IWholesalerRepository _wholesalerRepository;
        private readonly IBeerRepository _beerRepository;

        public WholesalersController( IWholesalerRepository repository, ISalesRepository salesRepository, IBeerRepository beerRepository )
        {
            _wholesalerRepository = repository;
            _salesRepository = salesRepository;
            _beerRepository = beerRepository;
        }

        // GET: api/Wholesalers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Wholesaler>>> GetWholesalers()
        {
            List<Wholesaler> wholesalers = (List<Wholesaler>)_wholesalerRepository.getWholesalers();

            foreach (Wholesaler wholesaler in wholesalers)
            {
                wholesaler.Sales = _salesRepository.GetAll()
                    .Where(s => s.WholeSalerId == wholesaler.Id)
                    .ToList();

                wholesaler.Stocks = _wholesalerRepository.GetWholesalerStocks()
                    .Where(s => s.WholesalerId == wholesaler.Id)
                    .ToList();
            }

            return wholesalers;
        }

        // GET: api/Wholesalers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Wholesaler>> GetWholesaler(int id)
        {
            var wholesaler = _wholesalerRepository.getWholesalerByID(id);

            if (wholesaler == null)
            {
                return NotFound();
            }

            wholesaler.Sales = _salesRepository.GetAll()
                    .Where(s => s.WholeSalerId == wholesaler.Id)
                    .ToList();

            wholesaler.Stocks = _wholesalerRepository.GetWholesalerStocks()
                .Where(s => s.WholesalerId == wholesaler.Id)
                .ToList();

            return wholesaler;
        }
        [HttpGet("/api/Sale/beer={beerId}&quantity={quantity}&seller={wholesalerId}")]
        public async Task<ActionResult<string>> GetQuote(int wholesalerId, int beerId, int quantity)
        {
            var wholesaler = _wholesalerRepository.getWholesalerByID(wholesalerId);
            var beer = _beerRepository.getBeerByID(beerId);

            if (wholesaler == null || beer == null)
            {
                return BadRequest("Beer or wholesaler don't exist");
            }

            if (!_wholesalerRepository.GetBeerWholesalerRelationships()
                    .Where(b => b.WholeSalerId == wholesaler.Id)
                    .Select(table => table.BeerId)
                    .Contains(beerId))
            {
                return BadRequest("Wholesaler can't sell this beer");
            }

            decimal quotePrice = quantity * beer.BreweryPrice;

            if (quantity > 20)
            {
                quotePrice = quotePrice - (quotePrice * (20 / 100));

            } if (quantity > 10)
            {
                quotePrice = quotePrice - (quotePrice * (10 / 100));
            }

            var result = $"The price for the quoted order from {wholesaler.Name} for {quantity} units of {beer.Name} will total at around {quotePrice}";
            result = JsonConvert.SerializeObject(result);


            return Content(result, "application/json");
        }

        // PUT: api/Wholesalers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWholesaler(int id, Wholesaler wholesaler)
        {
            if (id != wholesaler.Id)
            {
                return BadRequest();
            }

            _wholesalerRepository.UpdateWholesaler(wholesaler);

            try
            {
                _wholesalerRepository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_wholesalerRepository.WholesalerExists(id))
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

        // POST: api/Wholesalers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Wholesaler>> PostWholesaler(Wholesaler wholesaler)
        {
            _wholesalerRepository.InsertWholesaler(wholesaler);

            return CreatedAtAction("GetWholesaler", new { id = wholesaler.Id }, wholesaler);
        }

        // DELETE: api/Wholesalers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWholesaler(int id)
        {
            var wholesaler = _wholesalerRepository.getWholesalerByID(id);
            if (wholesaler == null)
            {
                return NotFound();
            }

            _wholesalerRepository.DeleteWholesaler(wholesaler);

            return NoContent();
        }
    }
}
