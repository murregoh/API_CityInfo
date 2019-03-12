using Microsoft.AspNetCore.Mvc;
using CitiesInfo.Models;
using CitiesInfo.DataStorage;
using System.Linq;
using System.Collections.Generic;


namespace CitiesInfo.Controllers
{
    [Route("api/cities")]
    public class PointOfInterestController : Controller
    {
        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {

            ICollection<PointOfInterestDto> pointsofinterest = CityDataStorage.Currect.Cities.FirstOrDefault(c => c.id == cityId).pointofinterestdto;

            return Ok(pointsofinterest);

        }

        [HttpGet("{cityId}/pointsofinterest/{point}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int point)
        {

            CityDto city = CityDataStorage.Currect.Cities.FirstOrDefault(c => c.id == cityId);
            PointOfInterestDto pointsofinterest = city.pointofinterestdto.FirstOrDefault(p => p.id == point);

            if (city == null || pointsofinterest == null)
            {
                return NotFound();
            }

            return Ok(pointsofinterest);

        }

        [HttpPost("{cityId}/pointofinterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromForm] PointOfInteresForCreationDto pointofinterest) 
        {
            if (pointofinterest == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            CityDto city = CityDataStorage.Currect.Cities.FirstOrDefault(c => c.id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            int maxPointsOfInterest = city.pointofinterestdto.Count;

            PointOfInterestDto finalPointOfInterest = new PointOfInterestDto() 
            {
                id = ++maxPointsOfInterest,
                name = pointofinterest.name,
                description = pointofinterest.description
            };

            city.pointofinterestdto.Add(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, finalPointOfInterest.id }, finalPointOfInterest);

        }

    }
}
