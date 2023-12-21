using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BreweryApi.Models;

namespace BreweryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly BreweryContext _context;

        public SalesController(BreweryContext context)
        {
            _context = context;
        }

        // GET: api/Sales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sales>>> GetSales()
        {
            return await _context.Sales.ToListAsync();
        }

        // GET: api/Sales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sales>> GetSales(int id)
        {
            var sales = await _context.Sales.FindAsync(id);

            if (sales == null)
            {
                return NotFound();
            }

            return sales;
        }

        // PUT: api/Sales/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSales(int id, Sales sales)
        {
            if (id != sales.Id)
            {
                return BadRequest();
            }

            _context.Entry(sales).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesExists(id))
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

        // POST: api/Sales
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sales>> PostSales(Sales sales)
        {

            var wholesaler = await _context.Wholesalers.FindAsync(sales.WholeSalerId);
            var beer = await _context.Beer.FindAsync(sales.BeerId);
            var brewery = await _context.Brewery.FindAsync(sales.BreweryId);

            if (brewery == null || beer == null)
            {
                return BadRequest("Brewery or beer don't exist");
            }

            if (sales.Quantity <= 0)
            {
                return BadRequest("You can't make a sale without informing the quantity");
            }

            if (wholesaler != null)
            {
                if (!_context.BeerWholesalers
                    .Where(b => b.WholeSalerId == wholesaler.Id)
                    .Select(table => table.BeerId)
                    .Contains(sales.BeerId))
                {
                    return BadRequest("Wholesaler can't buy this beer");
                }

                int stock = _context.WholesalerStocks.Where(stock => stock.WholesalerId == wholesaler.Id).ToList()
                    .Select(stock => stock.StockQuantity)
                    .Aggregate((StockUsed, next) => StockUsed + next);

                if (wholesaler.StockLimit < stock + sales.Quantity)
                {
                    return BadRequest("The current sale exceeds the stock limit of the wholesaler");
                } else
                {
                    var wholesaleStock = _context.WholesalerStocks.FirstOrDefault(w => w.WholesalerId == wholesaler.Id
                    && w.BeerId == sales.BeerId);

                    if (wholesaleStock != null )
                    {
                        wholesaleStock.StockQuantity += sales.Quantity;
                        await _context.SaveChangesAsync();
                    }  
                }
            } else
            {
                return BadRequest("The wholesaler dosn't exist");
            }

            _context.Sales.Add(sales);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSales", new { id = sales.Id }, sales);
        }

        // DELETE: api/Sales/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSales(int id)
        {
            var sales = await _context.Sales.FindAsync(id);
            if (sales == null)
            {
                return NotFound();
            }

            _context.Sales.Remove(sales);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SalesExists(int id)
        {
            return _context.Sales.Any(e => e.Id == id);
        }
    }
}
