using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BreweryApi.Models;

namespace BreweryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WholesalersController : ControllerBase
    {
        private readonly BreweryContext _context;

        public WholesalersController(BreweryContext context)
        {
            _context = context;
        }

        // GET: api/Wholesalers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Wholesaler>>> GetWholesalers()
        {
            var wholesalers = await _context.Wholesalers.ToListAsync();

            foreach (Wholesaler wholesaler in wholesalers)
            {
                wholesaler.Sales = _context.Sales.Where(s => s.WholeSalerId == wholesaler.Id).ToList();
                wholesaler.Stocks = _context.WholesalerStocks.Where(s => s.WholesalerId == wholesaler.Id).ToList();
            }

            return wholesalers;
        }

        // GET: api/Wholesalers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Wholesaler>> GetWholesaler(int id)
        {
            var wholesaler = await _context.Wholesalers.FindAsync(id);

            if (wholesaler == null)
            {
                return NotFound();
            }

            wholesaler.Sales = _context.Sales.Where(s => s.WholeSalerId == wholesaler.Id).ToList();
            wholesaler.Stocks = _context.WholesalerStocks.Where(s => s.WholesalerId == wholesaler.Id).ToList();

            return wholesaler;
        }
        [HttpGet("/Sale/beer={beerId}&quantity={quantity}&seller={wholesalerId}")]
        public async Task<ActionResult<string>> GetQuote(int wholesalerId, int beerId, int quantity)
        {

            
            var wholesaler = await _context.Wholesalers.FindAsync(wholesalerId);
            var beer = await _context.Beer.FindAsync(beerId);

            if (wholesaler == null || beer == null)
            {
                return BadRequest("Beer or wholesaler don't exist");
            }

            if (!_context.BeerWholesalers
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

            return $"The price for the quoted order from {wholesaler.Name} for {quantity} units of {beer.Name} will total at around {quotePrice}";
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

            _context.Entry(wholesaler).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WholesalerExists(id))
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
            _context.Wholesalers.Add(wholesaler);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWholesaler", new { id = wholesaler.Id }, wholesaler);
        }

        // DELETE: api/Wholesalers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWholesaler(int id)
        {
            var wholesaler = await _context.Wholesalers.FindAsync(id);
            if (wholesaler == null)
            {
                return NotFound();
            }

            _context.Wholesalers.Remove(wholesaler);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WholesalerExists(int id)
        {
            return _context.Wholesalers.Any(e => e.Id == id);
        }
    }
}
