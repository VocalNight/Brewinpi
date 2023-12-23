using BreweryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BreweryApi.Repositories
{
    public class BreweryRepository : IBreweryRepository
    {

        private BreweryContext _context;

        public BreweryRepository(BreweryContext context)
        {
            _context = context;
        }

        public void DeleteBrewery( Brewery brewery )
        {
            _context.Brewery.Remove(brewery);
            Save();
        }


        public IEnumerable<Brewery> getBreweries()
        {
            return _context.Brewery.ToList();
        }

        public Brewery getBreweryByID( int id )
        {
            return _context.Brewery.Find(id);
        }

        public void InsertBrewery( Brewery brewery )
        {
            _context.Brewery.Add(brewery);
            SaveAsync();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void SaveAsync()
        {
            _context.SaveChangesAsync();
        }

        public void UpdateBrewery( Brewery brewery )
        {
            _context.Entry(brewery).State = EntityState.Modified;
        }
    }
}
