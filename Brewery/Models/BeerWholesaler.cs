namespace BreweryApi.Models
{
    public class BeerWholesaler
    {
        public int Id { get; set; }
        public int WholeSalerId { get; set; }
        public int BeerId { get; set; }
    }
}
