using BreweryApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BreweryApi.Repositories
{
    public interface IBreweryRepository
    {
        IEnumerable<Brewery> getBreweries();
        Brewery getBreweryByID(int id);
        void InsertBrewery(Brewery brewery);
        void DeleteBrewery(Beer beer);
        void UpdateBrewery( Brewery brewery );
        void Save();
    }
}
