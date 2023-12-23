using BreweryApi.Models;

namespace BreweryApi.Repositories
{
    public class BeerRepository : IBeerRepository
    {

        private BreweryContext _context;

        public BeerRepository(BreweryContext context)
        {
            _context = context;
        }

        public void DeleteBeer( Beer beer )
        {
            throw new NotImplementedException();
        }

        public Brewery getBeerByID( int id )
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Beer> getBeers()
        {
            throw new NotImplementedException();
        }

        public void InsertBeer( Beer beer )
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateBeer( Beer beer )
        {
            throw new NotImplementedException();
        }
    }
}
