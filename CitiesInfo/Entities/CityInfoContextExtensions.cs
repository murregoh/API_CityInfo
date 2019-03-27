using System;
using System.Collections.Generic;
using System.Linq;

namespace CitiesInfo.Entities
{
    public static class CityInfoContextExtensions
    {
        public static void EnsureSeedDataForContext(this CityInfoContext context)
        {
            if (context.cities.Any())
            {
                return;
            }

            var Cities = new List<City>
            {
                new City
                {
                     name = "New York City",
                     description = "The one with that big park.",
                     pointsOfInterest = new List<PointOfInterest>
                     {
                         new PointOfInterest {
                             name = "Central Park",
                             description = "The most visited urban park in the United States." },
                          new PointOfInterest {
                             name = "Empire State Building",
                             description = "A 102-story skyscraper located in Midtown Manhattan." }
                     }
                },
                new City
                {
                    name = "Antwerp",
                    description = "The one with the cathedral that was never really finished.",
                    pointsOfInterest = new List<PointOfInterest>
                     {
                         new PointOfInterest {
                             name = "Cathedral of Our Lady",
                             description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans." },
                          new PointOfInterest {
                             name = "Antwerp Central Station",
                             description = "The the finest example of railway architecture in Belgium." }
                     }
                },
                new City
                {
                    name = "Paris",
                    description = "The one with that big tower.",
                    pointsOfInterest = new List<PointOfInterest>
                     {
                         new PointOfInterest {
                             name = "Eiffel Tower",
                             description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel." },
                          new PointOfInterest {
                             name = "The Louvre",
                             description = "The world's largest museum." }
                     }
                }
            };

            context.cities.AddRange(Cities);
            context.SaveChanges();
            
        }
    }
}