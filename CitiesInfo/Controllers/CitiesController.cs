using Microsoft.AspNetCore.Mvc;
using CitiesInfo.DataStorage;
using System.Linq;
using CitiesInfo.Models;
using CitiesInfo.Interfaces;
using System.Collections.Generic;
using CitiesInfo.Entities;
using AutoMapper;

namespace CitiesInfo.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository infoRepository;

        public CitiesController(ICityInfoRepository _infoRepository)
        {
            infoRepository = _infoRepository;
        }

        [HttpGet]
        public IActionResult GetCities() 
        {
            IEnumerable<City> cityEntities = infoRepository.GetCities();

            return Ok(Mapper.Map<IEnumerable<CityWithOutPointsOfInterestDto>>(cityEntities));
        }

        [HttpGet("{cityId}")]
        public IActionResult GetCity(int cityId, bool includePointsOfInterest = false) 
        {
            City city = infoRepository.GetCity(cityId, includePointsOfInterest);

            if (city == null)
                return NotFound("City was not found");

            if (includePointsOfInterest) 
                return Ok(Mapper.Map<CityDto>(city));
            else
                return Ok(Mapper.Map<CityWithOutPointsOfInterestDto>(city));
        }
    }
}
