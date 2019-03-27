using Microsoft.EntityFrameworkCore;

namespace CitiesInfo.Entities
{
    public class CityInfoContext : DbContext
    {
        public CityInfoContext(DbContextOptions<CityInfoContext> options) 
        : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<City> cities { get; set; }
        public DbSet<PointOfInterest> pointsOfInterest { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("connectionString");
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}