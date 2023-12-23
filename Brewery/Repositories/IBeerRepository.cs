using BreweryApi.Models;

namespace BreweryApi.Repositories
{
    public interface IBeerRepository
    {
        IEnumerable<Beer> getBeers();
        Brewery getBeerByID( int id );
        void InsertBeer( Beer beer );
        void DeleteBeer( Beer beer );
        void UpdateBeer( Beer beer );
        void Save();
    }
}
