using System;
using System.Collections.Generic;
using CitiesInfo.Models;

namespace CitiesInfo.DataStorage
{
    public class CityDataStorage
    {
        public static CityDataStorage Currect = new CityDataStorage();

        public List<CityDto> Cities;

        public CityDataStorage()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                     id = 1,
                     name = "New York City",
                     description = "The one with that big park.",
                     pointofinterestdto = new List<PointOfInterestDto>()
                     {
                         new PointOfInterestDto() {
                             id = 1,
                             name = "Central Park",
                             description = "The most visited urban park in the United States." },
                          new PointOfInterestDto() {
                             id = 2,
                             name = "Empire State Building",
                             description = "A 102-story skyscraper located in Midtown Manhattan." },
                     }
                },
                new CityDto()
                {
                    id = 2,
                    name = "Antwerp",
                    description = "The one with the cathedral that was never really finished.",
                    pointofinterestdto = new List<PointOfInterestDto>()
                     {
                         new PointOfInterestDto() {
                             id = 1,
                             name = "Cathedral of Our Lady",
                             description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans." },
                          new PointOfInterestDto() {
                             id = 2,
                             name = "Antwerp Central Station",
                             description = "The the finest example of railway architecture in Belgium." },
                            new PointOfInterestDto() {
                             id = 3,
                             name = "Prueba numero 1",
                             description = "Descripcion de la prueba numero 1." },
                            new PointOfInterestDto() {
                             id = 4,
                             name = "Prueba numero 2",
                             description = "Descripcion de la prueba numero 2." },
                            new PointOfInterestDto() {
                             id = 5,
                             name = "Prueba numero 3",
                             description = "Descripcion de la prueba numero 3." },
                     }
                },
                new CityDto()
                {
                    id= 3,
                    name = "Paris",
                    description = "The one with that big tower.",
                    pointofinterestdto = new List<PointOfInterestDto>()
                     {
                         new PointOfInterestDto() {
                             id = 1,
                             name = "Eiffel Tower",
                             description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel." },
                          new PointOfInterestDto() {
                             id = 2,
                             name = "The Louvre",
                             description = "The world's largest museum." },
                            new PointOfInterestDto() {
                             id = 3,
                             name = "Prueba numero 1",
                             description = "Descripcion de la prueba numero 1." },
                     }
                }
            };
        }
    }
}
