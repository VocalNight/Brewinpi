using BreweryApi.Models;

namespace BreweryApi.Services
{
    public class BreweryService
    {
        public void AddBeerToBrewery(Brewery brewer, Beer beer)
        {
            if (brewer.Beers == null)
            {
                brewer.Beers = new List<Beer>();
            }
            brewer.Beers.Add(beer);
        }
    }
}
