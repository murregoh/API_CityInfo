using System;
using System.Collections.Generic;
using System.Linq;
using CitiesInfo.Entities;
using CitiesInfo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CitiesInfo.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private CityInfoContext context;

        public CityInfoRepository(CityInfoContext _context)
        {
            context = _context;
        }

        public bool CityExist(int city)
        {
            return context.cities.Any(c => c.id == city);
        }

        public IEnumerable<City> GetCities()
        {
            return context.cities.OrderBy(c => c.name).ToList();
        }

        public City GetCity(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return context.cities.Include(c => c.pointsOfInterest)
                    .Where(c => c.id == cityId).FirstOrDefault();
            }
            else
            {
                return context.cities.Where(c => c.id == cityId).FirstOrDefault();
            }

        }

        public PointOfInterest GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            return context.pointsOfInterest.Where(p => p.cityId == cityId && p.id == pointOfInterestId).FirstOrDefault();
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterest(int cityId)
        {
            return context.pointsOfInterest.Where(p => p.cityId == cityId).ToList();
        }

        public void AddPointOfInterest(int cityId, PointOfInterest pointOfInterest)
        {
            City city = GetCity(cityId, false);

            city.pointsOfInterest.Add(pointOfInterest);
        }

        public void DeletePointOfInterest(PointOfInterest point)
        {
            context.pointsOfInterest.Remove(point);
        }

        public bool Save()
        {
            return (context.SaveChanges() >= 0);
        }
    }
}
