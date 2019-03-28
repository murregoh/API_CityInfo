using System;
using System.Collections.Generic;
using CitiesInfo.Entities;

namespace CitiesInfo.Interfaces
{
    public interface ICityInfoRepository
    {
        bool CityExist(int city);

        City GetCity(int cityId, bool includePointsOfInterest);

        IEnumerable<City> GetCities();

        PointOfInterest GetPointOfInterest(int cityId, int pointOfInterestId);

        IEnumerable<PointOfInterest> GetPointsOfInterest(int cityId);

        void AddPointOfInterest(int cityId, PointOfInterest pointOfInterest);

        void DeletePointOfInterest(PointOfInterest point);

        bool Save();
    }
}
