using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BreweryApi.Models;
using BreweryApi.Repositories;

namespace BreweryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISalesRepository _salesRepository;
        private readonly IBreweryRepository _breweryRepository;
        private readonly IBeerRepository _beerRepository;
        private readonly IWholesalerRepository _wholesalerRepository;

        public SalesController( ISalesRepository repository, IBeerRepository beerRepository, IBreweryRepository breweryRepository, IWholesalerRepository wholesalerRepository )
        {
            _salesRepository = repository;
            _beerRepository = beerRepository;
            _breweryRepository = breweryRepository;
            _wholesalerRepository = wholesalerRepository;
        }

        // GET: api/Sales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sales>>> GetSales()
        {

            return (List<Sales>)_salesRepository.getSales();

        }

        // GET: api/Sales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sales>> GetSales(int id)
        {
            var sales = _salesRepository.getSaleByID(id);

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

            _salesRepository.UpdateSale(sales);

            try
            {
                _salesRepository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_salesRepository.SaleExists(id))
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

        //TODO: Separate this method in a service class.
        [HttpPost]
        public async Task<ActionResult<Sales>> PostSales(Sales sales)
        {

            var wholesaler = _wholesalerRepository.getWholesalerByID(sales.WholeSalerId);
            var beer = _beerRepository.getBeerByID(sales.BeerId);
            var brewery = _breweryRepository.getBreweryByID(sales.BreweryId);

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
                if (!_wholesalerRepository.GetBeerWholesalerRelationships()
                    .Where(b => b.WholeSalerId == wholesaler.Id)
                    .Select(table => table.BeerId)
                    .Contains(sales.BeerId))
                {
                    return BadRequest("Wholesaler can't buy this beer");
                }

                int stock = _wholesalerRepository.GetWholesalerStocks()
                    .Where(stock => stock.WholesalerId == wholesaler.Id).ToList()
                    .Select(stock => stock.StockQuantity)
                    .Aggregate((StockUsed, next) => StockUsed + next);

                if (wholesaler.StockLimit < stock + sales.Quantity)
                {
                    return BadRequest("The current sale exceeds the stock limit of the wholesaler");

                } else

                {
                    var wholesaleStock = _wholesalerRepository.GetWholesalerStocks()
                    .FirstOrDefault(w => w.WholesalerId == wholesaler.Id
                    && w.BeerId == sales.BeerId);

                    if (wholesaleStock != null )
                    {
                        wholesaleStock.StockQuantity += sales.Quantity;
                        _wholesalerRepository.SaveAsync();
                    }  
                }
            } else
            {
                return BadRequest("The wholesaler dosn't exist");
            }

            _salesRepository.InsertSale(sales);

            return CreatedAtAction("GetSales", new { id = sales.Id }, sales);
        }

        // DELETE: api/Sales/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSales(int id)
        {
            var sales = _salesRepository.getSaleByID(id);
            if (sales == null)
            {
                return NotFound();
            }

            _salesRepository.DeleteSale(sales);

            return NoContent();
        }
    }
}
