using BreweryApi.Controllers;
using BreweryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BreweryTests
{
    [Collection("ControllerCollection")]
    public class DatabaseControllerTests : IDisposable
    {
        private readonly BreweryContext _dbContext;
        private readonly SalesController _saleController;
        private readonly WholesalersController _wholesalerController;

        public DatabaseControllerTests()
        {
            var options = new DbContextOptionsBuilder<BreweryContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BrewTesting;Trusted_Connection=True;MultipleActiveResultSets=true")
            .Options;

            _dbContext = new BreweryContext(options);
            _saleController = new SalesController(_dbContext);
            _wholesalerController = new WholesalersController(_dbContext);

            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        [Fact]
        public async Task TestCase_Fail_If_WholesalerNotBuyingAssignedBeer()
        {

            var sale = new Sales { BeerId = 1, BreweryId = 1, Quantity = 1, SaleDate = new DateTime(), WholeSalerId = 3 };

            await _saleController.PostSales(sale);

            var updatedEntity = _dbContext.Sales.FirstOrDefault(e => e.Id == 5);

            Assert.Null(updatedEntity);
        }

        [Fact]
        public async Task TestCase_Fail_If_WholesalerBuyingOverStock()
        {

            var sale = new Sales { BeerId = 1, BreweryId = 1, Quantity = 100000, SaleDate = new DateTime(), WholeSalerId = 1 };

            await _saleController.PostSales(sale);

            var updatedEntity = _dbContext.Sales.FirstOrDefault(e => e.Id == 5);

            Assert.Null(updatedEntity);
        }

        [Fact]
        public async Task TestCase_Fail_If_WholesalerDontExist()
        {

            var sale = new Sales { BeerId = 1, BreweryId = 1, Quantity = 1, SaleDate = new DateTime(), WholeSalerId = 5 };

            await _saleController.PostSales(sale);

            var updatedEntity = _dbContext.Sales.FirstOrDefault(e => e.Id == 5);

            Assert.Null(updatedEntity);
        }

        [Fact]
        public async Task TestCase_Fail_If_BeerDontExist()
        {

            var sale = new Sales { BeerId = 99, BreweryId = 1, Quantity = 1, SaleDate = new DateTime(), WholeSalerId = 1 };

            await _saleController.PostSales(sale);

            var updatedEntity = _dbContext.Sales.FirstOrDefault(e => e.Id == 5);

            Assert.Null(updatedEntity);
        }

        [Fact]
        public async Task TestCase_Fail_If_BreweryDontExist()
        {

            var sale = new Sales { BeerId = 1, BreweryId = 99, Quantity = 1, SaleDate = new DateTime(), WholeSalerId = 1 };

            await _saleController.PostSales(sale);

            var updatedEntity = _dbContext.Sales.FirstOrDefault(e => e.Id == 5);

            Assert.Null(updatedEntity);
        }

        [Fact]
        public async Task TestCase_Fail_If_SaleWith0OrNoQuantity()
        {

            var sale = new Sales { BeerId = 1, BreweryId = 1, Quantity = 0, SaleDate = new DateTime(), WholeSalerId = 1 };

            await _saleController.PostSales(sale);

            var updatedEntity = _dbContext.Sales.FirstOrDefault(e => e.Id == 5);

            Assert.Null(updatedEntity);
        }

        [Fact]
        public async Task TestCase_Fail_If_WholesalerDosntExist()
        {

            ActionResult<string> quoteResult = await _wholesalerController.GetQuote(99, 1, 100);
            BadRequestObjectResult q = (BadRequestObjectResult)quoteResult.Result;

            Assert.True(q.Value == "Beer or wholesaler don't exist");
        }

        [Fact]
        public async Task TestCase_Fail_If_BeerDosntExist()
        {

            ActionResult<string> quoteResult = await _wholesalerController.GetQuote(1, 99, 100);
            BadRequestObjectResult q = (BadRequestObjectResult)quoteResult.Result;

            Assert.True(q.Value == "Beer or wholesaler don't exist");
        }

        [Fact]
        public async Task TestCase_Fail_If_WholesalerDosntSellBeer()
        {

            ActionResult<string> quoteResult = await _wholesalerController.GetQuote(1, 3, 100);
            BadRequestObjectResult q = (BadRequestObjectResult)quoteResult.Result;

            Assert.True(q.Value == "Wholesaler can't sell this beer");
        }

        [Fact]
        public async Task TestCase_Pass_If_QuoteValid()
        {

            ActionResult<string> quoteResult = await _wholesalerController.GetQuote(1, 1, 100);
            string q = quoteResult.Value;

            string valid = "The price for the quoted order from Beer Dreams for 100 units of Malt will total at around 1500,00";

            Assert.True(q == valid);
        }
    }
}